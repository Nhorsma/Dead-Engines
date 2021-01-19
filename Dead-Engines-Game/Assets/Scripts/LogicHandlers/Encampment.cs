using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encampment
{
    private string[] deployment;
	private bool canSpawn = true;
	private bool playerInRange = false;
	private int id = -1;
	private int health = 100;
	private int chance = 0;
	private int onField = 0;
    private GameObject closestResource = null; 

    public Encampment(int newid)
    {
        canSpawn = true;
        chance = 0;
        id = newid;
        health = 100;
        onField = 0;
        playerInRange = false;
        deployment = new string[] { "hellow" };
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public int Chance
    {
        get { return chance; }
        set { chance = value; }
    }

    public bool CanSpawn
    {
        get { return canSpawn;}
        set { canSpawn = value; }
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int OnField
    {
        get { return onField; }
        set { onField = value; }
    }

    public GameObject ClosestResource
    {
        get { return closestResource;}
        set { closestResource = value; }
    }

    public bool PlayerInRange
    {
        get { return playerInRange;}
        set { playerInRange = value; }
    }

    public string[] Deployment
    {
        get { return deployment; }
        set { deployment = value; }
    }


}
