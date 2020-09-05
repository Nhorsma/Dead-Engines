using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{
    private string job;
    private int health;
    private bool selected;

    public Unit()
    {
        job = "";
        health = 3;
        selected = false;
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

}
