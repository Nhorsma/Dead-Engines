using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Enemy
{
    protected GameObject obj;
    protected bool canWalk, walkingForward, walkingBackward, firing, isDead;
    protected float speed;
    protected int range;

    public Hunter(GameObject g)
    {
        canWalk = true;
        obj = g;
        health = 1;
        id = -1;
        target = null;
        justShot = false;
        range = 1; //not implemented yet
        speed = 10f;
    }

    public GameObject Obj
    {
        get { return obj; }
        set { obj = value; }
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
