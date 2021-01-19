using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrine: Room
{
	private bool canFunction;

	public Shrine(int slot, int level)
	{
		this.Type = "shrine";
		this.Slot = slot;
		this.Level = level;
		this.Workers = new List<GameObject>();
		this.WorkerCapacity = 3;
		this.ActiveEffect = "none";
		this.CanFunction = false;
	}

	public bool CanFunction { get => canFunction; set => canFunction = value; }

	public override void DestroyRoom()
	{
		return; // ugh do later
	}
}
