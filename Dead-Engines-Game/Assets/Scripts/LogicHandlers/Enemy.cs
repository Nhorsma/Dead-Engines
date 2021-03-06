﻿using System.Collections;
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
	private int health = 5;
	private int damage = 1;
	private bool armored = false;

	private float fireSpeed = 1f; // fishy
	private bool justShot = false;
    private string job = "";

	private GameObject target = null;
	private GameObject resource = null;
	private GameObject campObj = null;
    private Encampment campData = null;

	public Enemy(int hp, int dmg, float shootspeed, bool isArmored)
    {
        armored = isArmored;
        damage = dmg;
		health = hp;
        id = -1;

		justShot = false;
		fireSpeed = shootspeed;

		target = null;
		campObj = null;
        campData = null;
		resource = null;
    }

    public Enemy()
    {
        armored = false;
        damage = 1;
        health = 1;
        id = -1;

        justShot = false;
        fireSpeed = 1f;

        target = null;
        campObj = null;
        campData = null;
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

    public bool Armored
    {
        get { return armored; }
        set { armored = value; }
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
