﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomatonUI : MonoBehaviour
{

	public GameObject auto_main;
	public List<GameObject> tabs = new List<GameObject>();

    void Start()
    {
        
    }

    void Update()
    {
		Debug.Log(auto_main.activeSelf);
    }

	private void OnMouseDown()
	{
		auto_main.SetActive(!auto_main.activeSelf);
	}

	public void OpenTab1()
	{
		foreach (GameObject t in tabs)
		{
			t.SetActive(false);
		}
		tabs[0].SetActive(true);
	}

	public void OpenTab2()
	{
		foreach (GameObject t in tabs)
		{
			t.SetActive(false);
		}
		tabs[1].SetActive(true);
	}

	public void OpenTab3()
	{
		foreach (GameObject t in tabs)
		{
			t.SetActive(false);
		}
		tabs[2].SetActive(true);
	}
}