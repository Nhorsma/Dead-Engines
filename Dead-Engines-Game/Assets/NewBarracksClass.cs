using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBarracksClass : NewRoomClass
{

	private Unit storedUnit;

	public Button doButton;
	public Button storeButton;

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
	}

	public NewBarracksClass()
	{

	}

	public Unit StoredUnit { get => storedUnit; set => storedUnit = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "barracks";
		nameText.text = "bar";
		Debug.Log("replaced old room");
		AddButtonEvents();
	}

	public void TurnOnButtons()
	{

	}

	public void AddButtonEvents()
	{
		storeButton.onClick.AddListener(delegate { roomManager.ChooseUnit(0); });
		doButton.onClick.AddListener(delegate { roomManager.UseBarracks(roomManager.ChooseUnit(0), "turtle"); });
	}
	//assign a unit
	//make a copy of that unit
	//spawn in copy

}
