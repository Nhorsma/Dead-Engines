using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDormitoryClass : NewRoomClass
{

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
	}

	public NewDormitoryClass()
	{

	}

	public void Update()
	{
		capacityText.text = roomManager.unitManager.units.Count.ToString() + " / " + roomManager.housingMax.ToString(); //not updating enough *shrug*
	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "dormitory";
		this.Level = 1;
		Debug.Log("replaced old room");
	}

	//allows for more units to live in robot

}
