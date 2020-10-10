using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    private int health;
    private int attack;
    private float fireSpeed;

    private int id;
    private GameObject target;
    private GameObject rec, camp;
    private bool justShot;

    public Enemy()
    {
        health = 5;
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

    public int Attack
    {
        get { return attack; }
        set { attack = value; }
    }

    public float FireSpeed
    {
        get { return fireSpeed; }
        set { fireSpeed = value; }
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

    public GameObject Rec
    {
        get { return rec; }
        set { rec = value; }
    }

    public GameObject Camp
    {
        get { return camp; }
        set { camp = value; }

    }

}
