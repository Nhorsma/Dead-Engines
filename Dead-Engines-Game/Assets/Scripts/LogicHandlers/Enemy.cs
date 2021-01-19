using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	//protected int health, damage;
	//protected int attack;
	//protected float fireSpeed;

	//protected int id;
	//protected GameObject target, obj;
	//protected GameObject rec, camp;
	//protected bool justShot;

	private int id = -1;
	private int health = 100;
	private int damage = 1;
	private int attack = 1;

	private float fireSpeed = 1f; // fishy
	private bool justShot = false;

	private GameObject target = null;
	private GameObject resource = null;
	private GameObject camp = null;

	public Enemy()
    {
		attack = 1;
		damage = 1;
		health = 100;
        id = -1;

		justShot = false;
		fireSpeed = 1f;

		target = null;
		camp = null;
		resource = null;
    }

    //public Enemy(int newID)
    //{
    //    health = 100;
    //    id = newID;
    //    target = null;
    //    justShot = false;
    //}

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

	public int Damage
	{
		get { return damage; }
		set { damage = value; }
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

    public GameObject Resource
    {
        get { return resource; }
        set { resource = value; }
    }

    public GameObject Camp
    {
        get { return camp; }
        set { camp = value; }
    }


}
