using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInfirmaryClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;

	public NewInfirmaryClass()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "infirmary";
		nameText.text = "inf";
		Debug.Log("replaced old room");
	}

}
