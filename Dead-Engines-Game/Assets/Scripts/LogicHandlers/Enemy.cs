using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy
{
    protected int health, damage;
    protected int attack;
    protected float fireSpeed;

    protected int id;
    protected GameObject target;
    protected GameObject rec, camp;
    protected bool justShot;

    public Enemy()
    {
        health = 5;
        id = -1;
        target = null;
        justShot = false;
        damage = 1;
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

    public int Damage
    {
       get { return damage; }
       set { damage = value; }
    }

}
