﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRefinery : NewRoomClass
{
	private int workerCapacity;
	private List<GameObject> workers;

	public NewRefinery()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "refinery";
		nameText.text = "ref";
		Debug.Log("replaced old room");
	}

}
