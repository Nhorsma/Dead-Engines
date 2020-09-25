using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

	public List<Room> rooms = new List<Room>();
	public List<Text> display; //change to gameObject later

	public int roomSlotClicked = 0;
	public AutomatonUI auto;

	public List<GameObject> miniTabs = new List<GameObject>();

	void Start()
    {
		for (int i = 0; i < 7; i++)
		{
			rooms.Add(new Room("empty", i, 0));
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OpenMiniTab(int clickedSlot)
	{
		miniTabs[clickedSlot].gameObject.SetActive(true);
		Debug.Log("Current tab " + clickedSlot);
	}

	public void TakeToBuild(int clickedSlot)
	{
		roomSlotClicked = clickedSlot;
		auto.OpenTab3();
	}

	public void Build(string room)
	{
		if (room == "refinery")
		{
			rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1);
			SetupRefinery(roomSlotClicked);
		}
		else if (room == "storage")
		{
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
			//SetupStorage(roomSlotClicked);
		}
		if (room == "shrine")
		{
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
			//SetupShrine(roomSlotClicked);
		}
		if (room == "study")
		{
			rooms[roomSlotClicked] = new Room("study", roomSlotClicked, 1);
			//SetupStudy(roomSlotClicked);
		}
		UpdateRoomDisplay();
	}

	void SetupRefinery(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Refinery";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Refine This";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Refine That";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Refine Other";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.gameObject.SetActive(true);
	}

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
	}

	public void UpdateRoomDisplay()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			display[i].text = rooms[i].Type;
		}
		Debug.Log("Updated Rooms");
		auto.OpenTab2();
	}
}
