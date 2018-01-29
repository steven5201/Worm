using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuControls : MonoBehaviour
{
    public Text pause;
    public Text helpText;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Play");
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            togglePause();
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.UpArrow)|| Input.GetKeyDown(KeyCode.DownArrow)|| Input.GetKeyDown(KeyCode.LeftArrow)|| Input.GetKeyDown(KeyCode.RightArrow))
        {
            helpText.gameObject.SetActive(false);
        }
    }

    void togglePause()
    {
        if (Time.timeScale == 0f)
        {
            Time.timeScale = 1f;
            pause.gameObject.SetActive(false);
        }
        else
        {
            Time.timeScale = 0f;
            pause.gameObject.SetActive(true);
        }
    }
}
