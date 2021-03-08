using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refinery : Room
{
	private bool canFunction;

	public Refinery(int slot, int level)
	{
		this.Type = "refinery";
		this.Slot = slot;
		this.Level = level;
		this.Workers = new List<GameObject>();
		this.WorkerCapacity = 3;
		this.CanFunction = false;
	}

	public bool CanFunction { get => canFunction; set => canFunction = value; }

	public override void DestroyRoom()
	{
		return; // ugh do later
	}

}
