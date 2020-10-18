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
	public UnitManager unitManager;

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

	public GameObject debugPanel;
	public static List<Text> debugText= new List<Text>();
	public Text debug1;
	public Text debug2;
	public Text debug3;

	public List<GameObject> unitViewport = new List<GameObject>();

	public int lastClickedID = -1;

	void Start()
    {
		metalText.text = " ";
		electronicsText.text = " ";
		hudMetal.text = " ";
		hudElectronics.text = " ";

		debugText.Add(debug1);
		debugText.Add(debug2);
		debugText.Add(debug3);
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			debugPanel.SetActive(!debugPanel.activeSelf);
		}
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

	public void UpdateInfoTab()
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

		if (unitManager.units.Length > 0)
		{
			for (int i = 0; i < unitManager.units.Length; i++)
			{
				unitViewport[i].GetComponentInChildren<Text>().text = unitManager.units[i].UnitName;
			}
		}
	}

	public void FindUnit(int i)
	{
		Vector3 unitPos = unitManager.unitsGM[i].transform.position;
		Debug.Log(unitPos);
		lastClickedID = i; ////////////////////////////////////////////////
	}
}
