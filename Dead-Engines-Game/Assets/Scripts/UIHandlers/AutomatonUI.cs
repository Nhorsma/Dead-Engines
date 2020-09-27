using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomatonUI : MonoBehaviour
{

	public GameObject auto_main;
	public List<GameObject> tabs = new List<GameObject>();
	public RoomManager roomManager;

    public Text metalText;
	public Text electronicsText;

	void Start()
    {
		metalText.text = " ";
		electronicsText.text = " ";
    }

    void Update()
    {
		metalText.text = "Metal: " + ResourceHandling.metal;
		electronicsText.text = "Electronics: " + ResourceHandling.electronics;
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
		foreach (GameObject m in roomManager.miniTabs)
		{
			m.SetActive(false);
		}
		roomManager.ctrlMiniTab.gameObject.SetActive(false);
		roomManager.genMiniTab.gameObject.SetActive(false);
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
