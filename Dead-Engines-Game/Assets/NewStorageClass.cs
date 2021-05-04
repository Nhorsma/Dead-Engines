using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewStorageClass : NewRoomClass
{

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
	}

	public NewStorageClass()
	{

	}

	public void Update()
	{
		capacityText.text = ResourceHandling.storageUsed.ToString() + " / " + roomManager.storageMax.ToString();
	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "storage";
		this.Health = 25;
		this.Defense = 5;
		this.Other = "moreStorage";
		Debug.Log("replaced old room");
	}

	//allows for more inventory space in the robot

}
