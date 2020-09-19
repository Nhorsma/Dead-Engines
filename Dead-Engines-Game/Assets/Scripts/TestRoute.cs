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
        if(nv.remainingDistance==0 || Vector3.Distance(center.transform.position,nv.gameObject.transform.position)<=range)
        {
            Debug.Log("Turn");
            nv.SetDestination(SetRoute(center, range));
            Instantiate(Resources.Load(pivot.name), SetRoute(center, range), transform.rotation);
        }
    }

    Vector3 SetRoute(GameObject gm, float range)
    {
        
        Vector3 f = gm.transform.forward;
        float x = Mathf.Cos(60) * range;
        float z = Mathf.Sin(60) * range;
        return new Vector3(x*f.x, 0, z*f.z);
        
    //    nv.gameObject.transform.rotation = Quaternion.AngleAxis(60, Vector3.up);
    //    nv.gameObject.transform.Translate(nv.gameObject.transform.forward * range, Space.World);
    }

    void StartRoute()
    {
        nv.SetDestination(center.transform.position);
    }
}
