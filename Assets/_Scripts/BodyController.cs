using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BodyController : MonoBehaviour
{
    private float speed = 10.0F;
    private float nextActionTime = 0.0f;

    private bool goLeft = false;
    private bool goDown = false;
    private bool goRight = false;
    private bool goUp = false;

    private Vector3 movePos;
    public List<MovementHandler> movement = new List<MovementHandler>();
    [HideInInspector]
    public bool addedToMainList = false;
    private bool isNew = true;
    [HideInInspector]
    public bool isFirst3BodyParts = false;

    [HideInInspector]
    public bool GoLeft { get { return goLeft; } }
    [HideInInspector]
    public bool GoDown { get { return goDown; } }
    [HideInInspector]
    public bool GoRight { get { return goRight; } }
    [HideInInspector]
    public bool GoUp { get { return goUp; } }

    // public float objectNum;

    // Use this for initialization
    void Start()
    {
        PlayerController playerController = GameObject.Find("PlayerHead").GetComponent<PlayerController>();

        speed = playerController.speed;
        nextActionTime = playerController.nextActionTime;

        //objectNum = Random.Range(0, 50000000);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.timeSinceLevelLoad > nextActionTime && !isNew)
        {
            nextActionTime += speed;

            if (goLeft)
            {
                movePos = new Vector3(-.25f, 0, 0);
            }

            if (goRight)
            {
                movePos = new Vector3(.25f, 0, 0);
            }

            if (goDown)
            {
                movePos = new Vector3(0, -.25f, 0);
            }

            if (goUp)
            {
                movePos = new Vector3(0, .25f, 0);
            }

            //if ((transform.position + movePos) != GameObject.Find("PlayerHead").GetComponent<PlayerController>().transform.position)
            //{
            transform.Translate(movePos);
            //}

            //  Debug.Log("Num: " + objectNum.ToString() + " || Up: " + goUp.ToString() + " || Down: " + goDown.ToString() + " || Right: " + goRight.ToString() + " || Left: " + goLeft.ToString());
        }
        else if (Time.timeSinceLevelLoad > nextActionTime && isNew)
        {
            //used to prevent the box from moving right away
            nextActionTime += speed;
            isNew = false;

            //Enables the boxcollider for each body part except the first one
            if (!isFirst3BodyParts)
            {
                GetComponent<BoxCollider2D>().enabled = true;
            }
        }

        if (isFirst3BodyParts && GetComponent<BoxCollider2D>().enabled == true)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }

        if (gameObject.GetComponent<Renderer>().bounds.Contains(movement[0].turnPos))
        {
            goDown = movement[0].turnDown;
            goLeft = movement[0].turnLeft;
            goRight = movement[0].turnRight;
            goUp = movement[0].turnUp;

            movement.Remove(movement[0]);
        }
    }

    public void SetMove(bool right, bool left, bool up, bool down)
    {
        if (isNew)
        {
            goRight = right;
            goLeft = left;
            goUp = up;
            goDown = down;
        }
    }
}

