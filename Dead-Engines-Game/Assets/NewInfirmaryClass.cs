using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewInfirmaryClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;

	public Text recoveryText;

	public void Start()
	{
		WorkerCapacity = 3;
		Workers = new List<GameObject>();
	}

	public void Update()
	{
		capacityText.text = Workers.Count + " / " + WorkerCapacity;
		recoveryText.text = "recovery time: " + EffectConnector.unitRecoveryTime/2 + "s";
	}

	public NewInfirmaryClass()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		this.Slot = oldSlot;
		this.Type = "infirmary";
		nameText.text = "infirmary";
		this.Health = 100;
		this.Defense = -5;
		this.Other = "none";
		Debug.Log("replaced old room");
	}

}
