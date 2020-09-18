using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private int health;
    private int id;
    private GameObject target;
    private bool justShot;

    public Enemy()
    {
        health = 100;
        id = -1;
        target = null;
        justShot = false;
    }

    public Enemy(int newID)
    {
        health = 100;
        id = newID;
        target = null;
        justShot = false;
    }


    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    public bool JustShot
    {
        get { return justShot; }
        set { justShot = value; }
    }

}
