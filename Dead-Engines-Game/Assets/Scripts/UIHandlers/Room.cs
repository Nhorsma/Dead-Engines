using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{

	private string type;
	private int slot;
	private int level;

	private List<GameObject> workers; //should this be an array? i think using lists is easier >;-0
	private int workerCapacity;

	private string activeEffect;

	//private List<GameObject> workers;
	//private string activeEffect;
	//private bool canFunction;

	public Room()
	{

	}

	public Room (string type, int slot, int level)
	{
		this.Type = type;
		this.Slot = slot;
		this.Level = level;
	}

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }
	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }

	public virtual void DestroyRoom() // give back % of resources used to build room -> so I guess Cost[] should be cost to upgrade? and then do the math based on that
	{
		return;
	}


}
