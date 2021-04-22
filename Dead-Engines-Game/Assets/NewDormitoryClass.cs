using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewDormitoryClass : NewRoomClass
{

	public NewDormitoryClass()
	{

	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "dormitory";
		this.Level = 1;
		nameText.text = "dor";
		Debug.Log("replaced old room");
	}

	//allows for more units to live in robot

}
