using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

	private string type;
	private int slot;
	private int level;
	//private List<Unit> workers;

	public Room (string t, int s, int l)
	{
		this.Type = t;
		this.Slot = s;
		this.Level = l;
	}

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
}
