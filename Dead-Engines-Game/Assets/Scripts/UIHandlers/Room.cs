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
		//this.Workers = new List<Unit>();
	}

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
	//public List<Unit> Workers { get => workers; set => workers = value; }
}
