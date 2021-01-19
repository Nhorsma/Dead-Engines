using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refinery : Room
{

	public Refinery(int slot, int level, int metalCost, int electronicsCost)
	{
		this.Type = "refinery";
		this.Slot = slot;
		this.Level = level;
		this.WorkMultiplier = 0;
		this.Workers = new List<GameObject>();
		this.ActiveEffect = "none";
		this.CanRefine = false;
		this.Cost = new List<int[]>(2);
		this.Cost[0][0] = metalCost;
		this.Cost[1][0] = electronicsCost;
	}

	public void AdjustWorkMultiplier()
	{
		this.WorkMultiplier = this.Workers.Count;
	}

	public override void ModifyCost(int costLevel)
	{
		this.Cost[0][1] = 15; // metal
		this.Cost[1][1] = 0; // electronics
	}

	public override void DestroyRoom()
	{
		return; // ugh do later
	}

	public override void Setup()
	{
		return; // ugh do later also
	}

}
