using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hunter : Enemy
{
    protected GameObject obj;
    protected bool canWalk, isWalking;

    public Hunter(GameObject g)
    {
        canWalk = true;
        obj = g;
        health = 5;
        id = -1;
        target = null;
        justShot = false;
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

    public bool IsWalking
    {
        get { return isWalking; }
        set { isWalking = value; }
    }


}
