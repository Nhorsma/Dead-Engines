using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Enemy
{
	private bool canWalk = true;
	private bool walkingForward = false;
	private bool walkingBackward = false;
    private bool canRetreat = false;
	private bool firing = false;
	private bool isDead = false;
    private bool nextMove = true;
    private float speed = 10f;
    private float firingSpeed = 2f;
    private int range = 1;

    public Hunter(int hp, int dmg, float shootspeed)
    {
        Health = hp;
        Id = -1;
        Target = null;
		JustShot = false;
        Armored = true;
        Damage = 1;

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

    public bool CanRetreat
    {
        get { return canRetreat; }
        set { canRetreat = value; }
    }

    public bool NextMove
    {
        get { return nextMove; }
        set { nextMove = value; }
    }

    public float FiringSpeed
    {
        get { return firingSpeed; }
        set { firingSpeed = value; }
    }

}
