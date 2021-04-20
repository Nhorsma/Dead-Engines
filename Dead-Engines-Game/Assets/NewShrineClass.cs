using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewShrineClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;
	private string activeEffect;

	public NewShrineClass()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "shrine";
		nameText.text = "shr";
		Debug.Log("replaced old room");
	}

}
