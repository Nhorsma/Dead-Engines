using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Storage : Room
{

	private int storageCapacity;
	private List<string> stored;

	public Storage(int slot, int level)
	{
		this.Type = "storage";
		this.Slot = slot;
		this.Level = level;
		this.StorageCapacity = 250;
		this.Stored = new List<string>(); // maybe
	}

	public int StorageCapacity { get => storageCapacity; set => storageCapacity = value; }
	public List<string> Stored { get => stored; set => stored = value; }

	public override void DestroyRoom()
	{
		return; // ugh do later
	}

}
