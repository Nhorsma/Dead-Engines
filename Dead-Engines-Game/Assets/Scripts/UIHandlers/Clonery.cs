using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clonery : Room
{

	public Clonery(int slot, int level)
	{
		this.Type = "clonery";
		this.Slot = slot;
		this.Level = level;
	}

	public override void DestroyRoom()
	{
		return; // ugh do later
	}
}
