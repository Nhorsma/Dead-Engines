using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encampment
{
    private bool canSpawn, playerInRange;
    private int id, health, chance;
    private GameObject closestRec;
    private string[] deployment;

    public Encampment(int newid)
    {
        canSpawn = true;
        chance = 0;
        id = newid;
        health = 100;
        playerInRange = false;
        deployment = new string[]{"gunner", "gunner", "gunner"};
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
