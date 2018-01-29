using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementObjectController : MonoBehaviour
{
    public int timeToWaitBeforeDestroy = 0;
    public int disableWaitTime = 0;

  //  [HideInInspector]
    public bool isCurrentlyActive = true;
    //[HideInInspector]
    public bool isNewest = true;

    private int bodyCount = 0;
    private int triggeredCount = 0;
    private int timeWaited = 0;
    private int scoreOnDestroy;
    private bool triggeredFinishedOnce = false;

    // Use this for initialization
    void Start()
    {
        foreach (GameObject item in GameObject.FindGameObjectsWithTag("PlayerBody"))
        {
            bodyCount++;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GoRight"))
        {
            item.GetComponent<MovementObjectController>().isNewest = false;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GoLeft"))
        {
            item.GetComponent<MovementObjectController>().isNewest = false;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GoUp"))
        {
            item.GetComponent<MovementObjectController>().isNewest = false;
        }

        foreach (GameObject item in GameObject.FindGameObjectsWithTag("GoDown"))
        {
            item.GetComponent<MovementObjectController>().isNewest = false;
        }

        isNewest = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(transform.position + "      " + bodyCount.ToString() + " || " + triggeredCount.ToString());

        if ((triggeredCount >= bodyCount && isCurrentlyActive) || (triggeredFinishedOnce && isCurrentlyActive))
        {
            timeWaited++;

            if (!triggeredFinishedOnce)
            {
                scoreOnDestroy = GameObject.Find("PlayerHead").GetComponent<PlayerController>().Score;
            }
            
            if (timeWaited >= disableWaitTime)
            {
                isCurrentlyActive = false;
                triggeredFinishedOnce = true;
                timeWaited = 0;
            }
        }

        if (isCurrentlyActive == false)
        {
            timeWaited++;

            //Debug.Log(transform.position + "      " + scoreOnDestroy.ToString() + " || " + GameObject.Find("PlayerHead").GetComponent<PlayerController>().Score.ToString());

            if (scoreOnDestroy != GameObject.Find("PlayerHead").GetComponent<PlayerController>().Score && isNewest)
            {
                Debug.Log("Works");
                bodyCount++;
                timeWaited = 0;
                isCurrentlyActive = true;
            }
            else if (timeWaited >= timeToWaitBeforeDestroy)
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        triggeredCount++;
    }
}
