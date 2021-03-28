using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dormitory : Room
{

	public Dormitory(int slot, int level)
	{
		this.Type = "dormitory";
		this.Slot = slot;
		this.Level = level;
	}

	public override void DestroyRoom()
	{
		return; // ugh do later
	}

}
