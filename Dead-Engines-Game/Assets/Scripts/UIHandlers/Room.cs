using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

	private string type;
	private int slot;
	private int level;
	private List<GameObject> workers;
	private int workMultiplier;
	private string activeEffect;
	private bool canRefine;
	private List<int[]> cost;

	public Room()
	{

	}

	public Room (string type, int slot, int level)
	{
		this.Type = type;
		this.Slot = slot;
		this.Level = level;
		this.WorkMultiplier = 0;
		this.Workers = new List<GameObject>();
		this.ActiveEffect = "none";
		this.CanRefine = false;
		this.Cost = new List<int[]>();
	}

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }
	public int WorkMultiplier { get => workMultiplier; set => workMultiplier = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }
	public bool CanRefine { get => canRefine; set => canRefine = value; }
	public List<int[]> Cost { get => cost; set => cost = value; }

	public virtual void Setup()
	{
		return;
	}

	public virtual void DestroyRoom()
	{
		return;
	}

	public virtual void ModifyCost(int costLevel)
	{
		return;
	}

	public virtual void UpgradeLevel()
	{
		this.Level++;
		return;
	}
}
