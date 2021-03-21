﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	private string unitName = "u";			// unit's individual name
    private string job = "none";			// what the unit is doing
    private GameObject jobPos = null;		// where the unit is going
    private int health = 10;					// how much damage the unit can take
	private bool justDroppedOff = false;	// is the unit on the way to or from the automaton?
	private bool justShot = false;			// has the unit fired its gun?
	private int id = -1;                    // which unit is this?
	private bool canSpawn = false;          // can the unit respawn?
    private bool armorPiercing = false;     //whether it can pierce armor
    private int attack = 2;
    private int defense = 1;
    private float firingSpeed = 1f;

	public Unit()
	{

	}

	public string Job
    {
        get { return job; }   
        set { job = value; }
    }

    public GameObject JobPos
    {
        get { return jobPos; }   
        set { jobPos = value; }
    }

    public int Health
    {
        get { return health; }   
        set { health = value; }
    }

    public bool JustDroppedOff
    {
        get { return justDroppedOff; }
        set { justDroppedOff = value; }
    }

    public bool JustShot
    {
        get { return justShot; }
        set { justShot = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

	public string UnitName
	{
		get { return unitName; }
		set { unitName = value; }
	}

    public bool CanSpawn
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    public bool Piercing
    {
        get { return armorPiercing; }
        set { armorPiercing = value; }
    }

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public int Defense
    {
        get { return defense; }
        set { defense = value; }
    }

    public float FiringSpeed
    {
        get { return firingSpeed; }
        set { firingSpeed = value; }
    }

}
