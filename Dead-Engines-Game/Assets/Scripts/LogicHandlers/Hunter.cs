using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Enemy
{
	private bool canWalk = true;
	private bool walkingForward = false;
	private bool walkingBackward = false;
	private bool firing = false;
	private bool isDead = false;
    private float speed = 10f;
    private int range = 1;

    public Hunter()
    {
		//inherited from enemy
        Health = 1;
        Id = -1;
        Target = null;
		JustShot = false;

		canWalk = true;
		range = 1; //not implemented yet
        speed = 10f;
    }

    public bool CanWalk
    {
        get { return canWalk; }
        set { canWalk = value; }
    }

    public bool WalkingForward
    {
        get { return walkingForward; }
        set { walkingForward= value; }
    }

    public bool WalkingBackward
    {
        get { return walkingBackward; }
        set { walkingBackward = value; }
    }

    public bool Firing
    {
        get { return firing; }
        set { firing = value; }
    }

    public bool IsDead
    {
        get { return isDead; }
        set { isDead = value; }
    }

    public float Speed
    {
        get { return speed; }
        set { speed = value; }
    }

    public int Range
    {
        get { return range; }
        set { range = value; }
    }


}
