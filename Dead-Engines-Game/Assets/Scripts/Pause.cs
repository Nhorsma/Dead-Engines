using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseControl : MonoBehaviour
{
    public static bool gamePaused;

    void Start()
    {
        gamePaused = false;
    }

    // Update is called once per frame
    public static void PauseGame()
    {
        gamePaused = true;
        Time.timeScale = 0;
    }

    public static void ResumeGame()
    {
        gamePaused = false;
        Time.timeScale = 1;
    }

    public static void TogglePause()
    {
        gamePaused = !gamePaused;

        if(gamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }


}
