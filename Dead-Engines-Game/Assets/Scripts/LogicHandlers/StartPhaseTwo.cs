using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhaseTwo : MonoBehaviour
{
    public static bool endPhaseOne;
    public static AutomotonAction aa;
    public static HunterHandler hh;

    void Start()
    {
        aa = GameObject.FindGameObjectWithTag("Robot").GetComponent<AutomotonAction>();
        hh = GetComponent<HunterHandler>();
        aa.enabled = false;
        hh.enabled = false;
        endPhaseOne = false;
    }

    public static void PhaseTwo()
    {
        aa.enabled = true;
        hh.enabled = true;
        endPhaseOne = true;

    }

}
