using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInfirmaryClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;

	public void Start()
	{
		WorkerCapacity = 3;
		Workers = new List<GameObject>();
	}

	public void Update()
	{
		if (Workers.Count > 0)
		{
			capacityText.text = "unit : " + Workers[0].GetComponent<Unit>().Id;
		}

	}

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
		nameText.text = "infirmary";
		this.Health = 100;
		this.Defense = -5;
		this.Other = "none";
		Debug.Log("replaced old room");
	}

}
