﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewStorageClass : NewRoomClass
{



	public NewStorageClass()
	{

	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "storage";
		nameText.text = "sto";
		Debug.Log("replaced old room");
	}

	//allows for more inventory space in the robot

}
