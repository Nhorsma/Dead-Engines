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

	public void SaveClickedRoom(int clickedSlot)
	{
		roomSlotClicked = clickedSlot;
		auto.OpenTab3();
	}

	public void Build(string room)
	{
		if (room == "refinery")
		{
			rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1);
		}
		else if (room == "storage")
		{
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
		}
		if (room == "shrine")
		{
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
		}
		if (room == "study")
		{
			rooms[roomSlotClicked] = new Room("study", roomSlotClicked, 1);
		}
		UpdateRoomDisplay();
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
