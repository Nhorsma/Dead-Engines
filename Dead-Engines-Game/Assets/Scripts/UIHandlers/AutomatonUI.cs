using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutomatonUI : MonoBehaviour
{
	
	public GameObject auto_main;
	public GameObject top_hud;
	public List<GameObject> tabs = new List<GameObject>();
	public RoomManager roomManager;

	//tab 1 stuff
    public Text metalText;
	public Text electronicsText;
	public Text boltText;
	public Text plateText;
	public Text partText;
	public Text wireText;
	public Text chipText;
	public Text boardText;

	// also handles HUD now
	public Text hudMetal;
	public Text hudElectronics;

	void Start()
    {
		metalText.text = " ";
		electronicsText.text = " ";
		hudMetal.text = " ";
		hudElectronics.text = " ";
    }

    void Update()
    {
		metalText.text = "Metal: " + ResourceHandling.metal;
		electronicsText.text = "Electronics: " + ResourceHandling.electronics;
		boltText.text = "Bolts: " + ResourceHandling.bolt;
		plateText.text = "Plates: " + ResourceHandling.plate;
		partText.text = "Parts: " + ResourceHandling.part;
		wireText.text = "Wires: " + ResourceHandling.wire;
		chipText.text = "Chips: " + ResourceHandling.chip;
		boardText.text = "Boards: " + ResourceHandling.board;

		hudMetal.text = "Metal: " + ResourceHandling.metal;
		hudElectronics.text = "Electronics: " + ResourceHandling.electronics;
	}

	private void OnMouseDown()
	{
		auto_main.SetActive(!auto_main.activeSelf);
		top_hud.SetActive(!top_hud.activeSelf);
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
