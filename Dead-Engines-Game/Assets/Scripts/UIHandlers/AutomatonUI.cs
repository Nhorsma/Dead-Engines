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

	public Button activationButton;

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
		UpdateInfoTab();
	}

	//private void OnMouseDown()
	//{
	//	if (!Pause.paused)
	//	{
	//		auto_main.SetActive(!auto_main.activeSelf);
	//		tabs[0].SetActive(!tabs[0].activeSelf);
	//		tabs[1].SetActive(false);								// ------------------------------------------> for some reason the room tab forces itself open at start, this is the *official* workaround
	//	}
	//}

	public void OpenInfoTab()
	{
		for (int i = 0; i < tabs.Count; i++)
		{
			if (i != 0) // info tab index
			{
				tabs[i].SetActive(false);
			}
		}

		if (auto_main.activeSelf == false && tabs[0].activeSelf == false) //if neither are turned on, turn both on
		{
			tabs[0].SetActive(true);
			auto_main.SetActive(true);
		}
		else if (auto_main.activeSelf == true && tabs[0].activeSelf == false) //if main content is on but tab is off, turn on tab only
		{
			Debug.Log("hit");
			tabs[0].SetActive(true);
		}
		else if (auto_main.activeSelf == true) // last - if main content and tab are both on, turn both off
		{
			tabs[0].SetActive(false);
			auto_main.SetActive(false);
		}

	}

	public void OpenRoomsTab()
	{
		for (int i = 0; i < tabs.Count; i++)
		{
			if (i != 1) // room tab index
			{
				tabs[i].SetActive(false);
			}
		}
		foreach (GameObject t in roomManager.roomTabs)
		{
			t.SetActive(false);
		}
		roomManager.controllerTab.gameObject.SetActive(false);
		roomManager.generatorTab.gameObject.SetActive(false);

		if (auto_main.activeSelf == false && tabs[1].activeSelf == false)
		{
			tabs[1].SetActive(true);
			auto_main.SetActive(true);
		}
		else if (auto_main.activeSelf == true && tabs[1].activeSelf == false)
		{
			Debug.Log("hit");
			tabs[1].SetActive(true);
		}
		else if (auto_main.activeSelf == true)
		{
			tabs[1].SetActive(false);
			auto_main.SetActive(false);
		}
	}

	public void OpenBuildTab()
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

		if (RoomManager.generatorRepaired && RoomManager.controllerRepaired)
		{
			activationButton.enabled = true;
		}
		else
		{
			activationButton.enabled = false;
		}

		//if (unitManager.units != null)
		//{
		//	if (unitManager.units.Length > 0)
		//	{
		//		for (int i = 0; i < unitManager.units.Length; i++)
		//		{
		//			unitViewport[i].GetComponentInChildren<Text>().text = unitManager.units[i].UnitName;
		//		}
		//	}
		//}
	}

	public void FindUnit(int i)
	{
		Vector3 unitPos = unitManager.units[i].transform.position;
		Debug.Log(unitPos);
		lastClickedID = i;
        Teleport(unitPos, 10f);
	}

    public void Teleport(Vector3 pos, float mod)
    {
        Camera.main.GetComponent<Rigidbody>().transform.position = pos + new Vector3(0f, mod, -mod);
    }
}
