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

	protected int id = -1;
	protected int health = 5;
	protected int attack = 1;
    protected int defense = 1;
	protected bool armored = false;

	protected float firingSpeed = 1f; // fishy
	protected bool justShot = false;
    protected string job = "";

	protected GameObject target = null;
	protected GameObject resource = null;
	protected GameObject campObj = null;
    protected Encampment campData = null;

	public Enemy(int hp, int att, int def, float shootspeed, bool isArmored)
    {
        armored = isArmored;
        attack = att;
        defense = def;
		health = hp;
        id = -1;

		justShot = false;
		firingSpeed = shootspeed;

		target = null;
		campObj = null;
        campData = null;
		resource = null;
    }

    public Enemy()
    {
        armored = false;
        attack = 1;
        defense = 1;
        health = 1;
        id = -1;

        justShot = false;
        firingSpeed = 1f;

        target = null;
        campObj = null;
        campData = null;
        resource = null;
    }

    public int Health
    {
        get { return health; }
        set { health = value; }
    }

    public bool Armored
    {
        get { return armored; }
        set { armored = value; }
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

    public GameObject CampObj
    {
        get { return campObj; }
        set { campObj = value; }
    }

    public Encampment CampData
    {
        get { return campData; }
        set { campData = value; }
    }

    public string Job
    {
        get { return job; }
        set { job = value; }
    }

    protected void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Friendly" || other.gameObject.tag=="Robot")
        {
            target = other.gameObject;
        }
    }

    protected void OnTriggerUpdate(Collider other)
    {
        if (other.gameObject.tag == "Friendly" || other.gameObject.tag == "Robot")
        {
            target = other.gameObject;
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Friendly" || other.gameObject.tag == "Robot")
        {
            target = null;
        }
    }


}
