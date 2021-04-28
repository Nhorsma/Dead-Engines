using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnoutBehavior : MonoBehaviour
{
    public GameObject pointTo;
    public GameObject[] joints;

    private void Start()
    {
        pointTo = GameObject.FindGameObjectWithTag("Robot");
    }

    void Update()
    {
        var relativePos = pointTo.transform.position - transform.position;
        var forward = transform.right;
        var angle = Vector3.Angle(relativePos, forward);//the -90 is for correcting weird angles, 0 should be infront of it
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

    public GameObject GetJointSix()
    {
        return joints[5];
    }
}
