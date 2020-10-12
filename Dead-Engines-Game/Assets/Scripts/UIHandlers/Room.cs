using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

	private string type;
	private int slot;
	private int level;
	private List<Unit> workers;
	private int workMultiplier;
	private string activeEffect;
	private bool canRefine;

	public Room (string t, int s, int l)
	{
		this.Type = t;
		this.Slot = s;
		this.Level = l;
		this.WorkMultiplier = 0;
		this.Workers = new List<Unit>();
		this.ActiveEffect = "none";
		this.CanRefine = false;
	}

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
	public List<Unit> Workers { get => workers; set => workers = value; }
	public int WorkMultiplier { get => workMultiplier; set => workMultiplier = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }
	public bool CanRefine { get => canRefine; set => canRefine = value; }
}
