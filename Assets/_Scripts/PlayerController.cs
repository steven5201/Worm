using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0F;
    public GameObject apple;
    public GameObject playerBody;
    //public GameObject moveLeft;
    //public GameObject moveRight;
    //public GameObject moveUp;
    //public GameObject moveDown;
    public Text scoreText;
    public GameObject spriteDisplay;
    public GameObject bodyPartHolder;

    private bool dieing = false;
    private int score = 0;

    public int Score { get { return score; } }

    [HideInInspector]
    public float nextActionTime = 0.0f;

    [HideInInspector]
    public bool isTurning = false;

    private bool goLeft = false;
    private bool goDown = false;
    private bool goRight = false;
    private bool goUp = false;
    private bool allowInput = true;

    private List<BodyController> bodyControllers = new List<BodyController>();

    // Use this for initialization
    void Start()
    {
        //Sets the apple position randomly
        Vector3 newPos = new Vector3(Random.Range(-7.2f, 7.2f), Random.Range(-5.3f, 5.3f), apple.transform.position.z);
        Instantiate(apple, newPos, new Quaternion());
    }

    void Update()
    {
        if (Time.timeSinceLevelLoad > nextActionTime)
        {
            nextActionTime += speed;

            if (goLeft)
            {
                // Instantiate(moveLeft, transform.position, new Quaternion());
                transform.Translate(-.25f, 0, 0);
                isTurning = false;
                allowInput = true;
            }

            if (goRight)
            {
                //  Instantiate(moveRight, transform.position, new Quaternion());
                transform.Translate(.25f, 0, 0);
                isTurning = false;
                allowInput = true;
            }

            if (goDown)
            {
                //  Instantiate(moveDown, transform.position, new Quaternion());
                transform.Translate(0, -.25f, 0);
                isTurning = false;
                allowInput = true;
            }

            if (goUp)
            {
                //  Instantiate(moveUp, transform.position, new Quaternion());
                transform.Translate(0, .25f, 0);
                isTurning = false;
                allowInput = true;
            }
        }

        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !goRight && allowInput)
        {
            isTurning = true;
            allowInput = false;
            ClearMoveBools();
            //Instantiate(moveLeft, transform.position, new Quaternion());

            AddToMovementList(new MovementHandler(transform.position, false, false, false, true));

            spriteDisplay.transform.rotation = Quaternion.identity;
            spriteDisplay.transform.Rotate(new Vector3(0, 0, 90));
            goLeft = true;
        }

        if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && !goLeft && allowInput)
        {
            isTurning = true;
            allowInput = false;
            ClearMoveBools();
            //Instantiate(moveRight, transform.position, new Quaternion());

            AddToMovementList(new MovementHandler(transform.position, true, false, false, false));

            spriteDisplay.transform.rotation = Quaternion.identity;
            spriteDisplay.transform.Rotate(new Vector3(0, 0, -90));
            goRight = true;
        }

        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) && !goDown && allowInput)
        {
            isTurning = true;
            allowInput = false;
            ClearMoveBools();
            //Instantiate(moveUp, transform.position, new Quaternion());

            AddToMovementList(new MovementHandler(transform.position, false, true, false, false));

            spriteDisplay.transform.rotation = Quaternion.identity;
            goUp = true;
        }

        if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) && !goUp && allowInput)
        {
            isTurning = true;
            allowInput = false;
            ClearMoveBools();
            //  Instantiate(moveDown, transform.position, new Quaternion());

            AddToMovementList(new MovementHandler(transform.position, false, false, true, false));

            spriteDisplay.transform.rotation = Quaternion.identity;
            spriteDisplay.transform.Rotate(new Vector3(0, 0, 180));
            goDown = true;
        }

        if (dieing)
        {
            gameObject.SetActive(!dieing);
            bodyPartHolder.SetActive(!dieing);
            PlayerPrefs.SetInt("Score", score);
            SceneManager.LoadScene("GameOver");
        }
    }

    public void Die()
    {
        dieing = true;
    }

    private void AddToMovementList(MovementHandler newMovement)
    {
        foreach (BodyController controller in bodyControllers)
        {
            controller.movement.Add(newMovement);
        }
    }

    private void ClearMoveBools()
    {
        goLeft = false;
        goDown = false;
        goRight = false;
        goUp = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Apple")
        {
            //Moves the apple to a random position
            Vector3 newPos = new Vector3(Random.Range(-7.2f, 7.2f), Random.Range(-5.3f, 5.3f), collider.transform.position.z);
            collider.transform.position = newPos;
            score++;
            scoreText.text = "Score: " + score.ToString();

            AddBody();
        }
    }

    private void AddBody()
    {
        Vector3 newPos;

        if (bodyPartHolder.transform.childCount != 0)
        {
            newPos = bodyPartHolder.transform.GetChild(bodyPartHolder.transform.childCount - 1).position;
        }
        else
        {
            newPos = transform.position;
        }

        Instantiate(playerBody, newPos, new Quaternion(), bodyPartHolder.transform);

        foreach (Transform bodyPart in bodyPartHolder.transform)
        {
            if (!bodyPart.GetComponent<BodyController>().addedToMainList)
            {
                bodyControllers.Add(bodyPart.GetComponent<BodyController>());
                bodyPart.GetComponent<BodyController>().addedToMainList = true;

                if (bodyPartHolder.transform.childCount >= 2)
                {
                    bodyPart.GetComponent<BodyController>().SetMove(bodyControllers[bodyPartHolder.transform.childCount - 2].GoRight, bodyControllers[bodyPartHolder.transform.childCount - 2].GoLeft, bodyControllers[bodyPartHolder.transform.childCount - 2].GoUp, bodyControllers[bodyPartHolder.transform.childCount - 2].GoDown);
                    bodyPart.GetComponent<BodyController>().movement.AddRange(bodyControllers[bodyPartHolder.transform.childCount - 2].movement);

                    if (bodyPartHolder.transform.childCount <= 3)
                    {
                        bodyPart.GetComponent<BodyController>().isFirst3BodyParts = true;
                    }
                }
                else
                {
                    bodyPart.GetComponent<BodyController>().SetMove(goRight, goLeft, goUp, goDown);
                    bodyPart.GetComponent<BodyController>().isFirst3BodyParts = true;
                }
            }
        }
    }
}
