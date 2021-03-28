using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : Room
{

	public Barracks(int slot, int level)
	{
		this.Type = "barracks";
		this.Slot = slot;
		this.Level = level;
	}

	public override void DestroyRoom()
	{
		return; // ugh do later
	}
}
