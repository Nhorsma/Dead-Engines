using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
	private string unitName;
    private string job;
    private GameObject jobPos;
    private int health;
    private bool justDroppedOff, justShot;
    private int id;
    private bool canSpawn;

    public Unit()
    {
        canSpawn = false;
        job = "none";
        jobPos = null;
        health = 5;
        justDroppedOff = false;
        id = -1;
        justShot = false;
    }

    public Unit(int dec)
    {
        job = "none";
        jobPos = null;
        health = 3;
        justDroppedOff = false;
        id = dec;
		unitName = "U" + id.ToString();
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

}
