using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private string job;
    private GameObject posOfJob;
    private int health;
    private bool justDroppedOff;
    private int id;

    public Unit()
    {
        job = "none";
        posOfJob = null;
        health = 3;
        justDroppedOff = false;
        id = -1;
    }

    public Unit(int dec)
    {
        job = "none";
        posOfJob = null;
        health = 3;
        justDroppedOff = false;
        id = dec;
    }

    public string GetJob()
    {
        return job;
    }

    public int GetHealth()
    {
        return health;
    }

    public void SetJob(string newJob)
    {
        job = newJob;
    }

    public void SetHealth(int newhealth)
    {
        health = newhealth;
    }

     public GameObject GetJobPos()
    {
        return posOfJob;
    }

    public void SetJobPos(GameObject pos)
    {
        posOfJob = pos;
    }

    public bool GetDroppedOff()
    {
        return justDroppedOff;
    }

    public void SetDroppedOff(bool b)
    {
        justDroppedOff = b;
    }

    public int ID()
    {
        return id;
    }

    public void Setid(int newone)
    {
        id = newone;
    }

}
