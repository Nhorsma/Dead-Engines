using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewStorageClass : NewRoomClass
{

	private List<GameObject> stored;

	public NewStorageClass()
	{

	}

	public List<GameObject> Stored { get => stored; set => stored = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "storage";
		nameText.text = "sto";
		Debug.Log("replaced old room");
	}

}
