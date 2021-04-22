using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCloneryClass : NewRoomClass
{

	private Unit storedUnit;

	public Button doButton;
	public Button storeButton;

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
	}

	public NewCloneryClass()
	{

	}

	public Unit StoredUnit { get => storedUnit; set => storedUnit = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "clonery";
		nameText.text = "clo";
		Debug.Log("replaced old room");
		AddButtonEvents();
	}

	public void TurnOnButtons()
	{

	}

	public void AddButtonEvents()
	{
		storeButton.onClick.AddListener(delegate { roomManager.ChooseUnit(0); });
		doButton.onClick.AddListener(delegate { roomManager.UseClonery(roomManager.ChooseUnit(0)); });
	}
	//assign a unit
	//make a copy of that unit
	//spawn in copy

}
