using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encampment
{
    private bool canSpawn;
    private int chance;
    private int id;
    private int health;

    public Encampment(int newid)
    {
        canSpawn = true;
        chance = 0;
        id = newid;
        health = 100;
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


}
