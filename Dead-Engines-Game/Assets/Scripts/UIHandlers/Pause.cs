using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{
	public static bool paused = false;
	public GameObject pausePanel;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
		{
			paused = !paused;
			pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
		}

		if (paused)
		{
			Time.timeScale = 0;
		}
		else if (!paused)
		{
			Time.timeScale = 1;
		}
    }

	public void Resume()
	{
		paused = false;
		pausePanel.gameObject.SetActive(!pausePanel.gameObject.activeSelf);
	}

	public void MainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void Quit()
	{
		Application.Quit();
	}

}
