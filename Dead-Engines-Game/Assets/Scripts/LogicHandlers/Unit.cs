using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour
{

	private string unitName = "u";			// unit's individual name
    private string job = "none";			// what the unit is doing
    private GameObject jobPos = null;		// where the unit is going
    private int health = 10;					// how much damage the unit can take
	private bool justDroppedOff = false;	// is the unit on the way to or from the automaton?
	private bool justShot = false;			// has the unit fired its gun?
	private int id = -1;                    // which unit is this?
	private bool canSpawn = false;          // can the unit respawn?
    private bool armorPiercing = false;

	public Unit()
	{
		//canSpawn = false;
		//job = "none";
		//jobPos = null;
		//health = 3;
		//justDroppedOff = false;
		//justShot = false;
		//id = -1;
		//justShot = false;
		//unitName = "U";
	}

	//  public Unit(int unit_id)
	//  {
	//canSpawn = false;
	//job = "none";
	//      jobPos = null;
	//      health = 3;
	//      justDroppedOff = false;
	//justShot = false;
	//id = unit_id;
	//unitName = "U" + id.ToString();
	//  }

	public string Job
    {
        get { return job; }   
        set { job = value; }
    }

    public GameObject JobPos
    {
        get { return jobPos; }   
        set { jobPos = value; }
    }

    public int Health
    {
        get { return health; }   
        set { health = value; }
    }

    public bool JustDroppedOff
    {
        get { return justDroppedOff; }
        set { justDroppedOff = value; }
    }

    public bool JustShot
    {
        get { return justShot; }
        set { justShot = value; }
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

	public string UnitName
	{
		get { return unitName; }
		set { unitName = value; }
	}

    public bool CanSpawn
    {
        get { return canSpawn; }
        set { canSpawn = value; }
    }

    public bool Piercing
    {
        get { return armorPiercing; }
        set { armorPiercing = value; }
    }

}
