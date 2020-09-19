using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TestRoute : MonoBehaviour
{

    public GameObject center, pivot;
    public NavMeshAgent nv;
    public float range;

    void Start()
    {
        StartRoute();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetRoute(GameObject gm, float range)
    {

    }

    void StartRoute()
    {
        nv.SetDestination(center.transform.position);
    }
}
