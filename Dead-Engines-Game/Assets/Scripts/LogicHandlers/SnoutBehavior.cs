using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoutBehavior : MonoBehaviour
{
    public GameObject pointTo;
    public GameObject[] joints;

    void Start()
    {
        joints = new GameObject[6];
        string name;
        GameObject check = gameObject;

        for(int i=1;i<7;i++)
        {
            name = "joint" + i;
            joints[i-1] = check.transform.Find(name).gameObject;
            check = check.transform.Find(name).gameObject;
        }
    }

    void Update()
    {
        var relativePos = pointTo.transform.position - transform.position;
        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward) - 90;//the -90 is for correcting weird angles, 0 should be infront of it
        Debug.Log(angle);
        Quaternion newAngle;
        for (int i=0;i<6;i++)
        {
            newAngle = joints[i].transform.rotation;
            if (Mathf.Abs(-angle * i / 2) < 30)
                newAngle = Quaternion.Euler(0, -angle * i / 2, 0);

            joints[i].transform.rotation = newAngle;
        }
    }
}
