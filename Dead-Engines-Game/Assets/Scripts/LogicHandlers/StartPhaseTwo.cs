using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhaseTwo : MonoBehaviour
{
    public static bool endPhaseOne;
    public static AutomotonAction aa;

    void Start()
    {
        aa = GameObject.FindGameObjectWithTag("Robot").GetComponent<AutomotonAction>();
        aa.enabled = false;
        endPhaseOne = false;
    }

    public static void PhaseTwo()
    {
        aa.enabled = true;
        endPhaseOne = true;
    }

}
