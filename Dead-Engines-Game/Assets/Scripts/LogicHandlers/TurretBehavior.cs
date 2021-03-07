using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretBehavior : MonoBehaviour
{
    public GameObject target;
    public float turnSpeed;

    // Update is called once per frame
    void Update()
    {
        var relativePos = target.transform.position - transform.position;
        var forward = transform.forward;
        var angle = Vector3.Angle(relativePos, forward) - 90; //the +30 is for correcting weird angles, 0 should be infront of it 
        if(angle>0)
        {
            //right
            transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
        }
        else if(angle<0)
        {
            //left
            transform.Rotate(Vector3.up * -turnSpeed * Time.deltaTime);
        }
    }
}
