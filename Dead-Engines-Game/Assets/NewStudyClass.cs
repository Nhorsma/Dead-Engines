using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewStudyClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;
	private string activeEffect;

	public NewStudyClass()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "study";
		nameText.text = "stu";
		Debug.Log("replaced old room");
	}

}
