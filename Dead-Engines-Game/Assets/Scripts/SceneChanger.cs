﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

	public void StartButton()
	{
		SceneManager.LoadScene("Zone1");
	}

	public void ExitButton()
	{
		Application.Quit();
	}

    public static void ReturnToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

}
