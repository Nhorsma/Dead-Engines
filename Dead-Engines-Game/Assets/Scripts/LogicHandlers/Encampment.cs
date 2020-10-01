using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encampment
{
    private string[] deployment;
    private bool canSpawn, playerInRange;
    private int id, health, chance, onField;
    private GameObject closestRec;


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

    public GameObject ClosestRec
    {
        get { return closestRec;}
        set { closestRec = value; }
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
