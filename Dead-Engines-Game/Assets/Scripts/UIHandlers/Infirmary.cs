using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Infirmary : Room
{

	public Infirmary(int slot, int level)
	{
		this.Type = "infirmary";
		this.Slot = slot;
		this.Level = level;
		this.WorkerCapacity = 3; //how many sick can be here?
		this.Workers = new List<GameObject>();
	}

	public override void DestroyRoom()
	{
		return; // ugh do later
	}

}
