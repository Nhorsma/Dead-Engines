using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public float movementSpeed, turnSpeed;
    public float startAngle, target, ny;
    public bool canMove,canRotate;
    Vector3 pos, walkTo;
    NavMeshAgent nv;
    Rigidbody rb;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canMove = canRotate = false;
    }

    private void Update()
    {
        pos = transform.position;
        if(Input.GetMouseButtonDown(1) && Hit().point != null)
        {
            walkTo = Hit().point;
            canRotate = true;
            startAngle = transform.rotation.eulerAngles.y;
            SetUpRotate(walkTo);
        }
        if(canRotate && walkTo!= null)
        {
            Rotate();
        }
        if(canMove && walkTo!=null)
        {
            Walk(walkTo);
        }
        if(Hit().point!=null)
            Debug.DrawLine(transform.position, Hit().point);
        //Debug.DrawLine(transform.position, transform.position*transform.forward);
    }


    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            return hit;
        return hit;
    }

    public void SetUpRotate(Vector3 hit)
    {
        transform.LookAt(hit, Vector3.up);
        target = transform.rotation.eulerAngles.y;  //target angle;
        transform.rotation = Quaternion.Euler(0, startAngle, 0);

        ny = startAngle - 180;
        if (ny < 0)
            ny += 360;
    }

    public void Rotate()
    {

        if(startAngle > ny)
        {
            if(target > startAngle && target<ny)
            {
                //clockwise
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //counter clockwise
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
        else
        {
            if(target > startAngle && target<ny)
            {
                //clockwise
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //counter clockwise
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }

        /*
            if (target < startAngle && target > ny)
            {
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime); //counter clock
            }
            else
            {
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime); //counter clock
            }


            if (target < startAngle || target < ny)
            {
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime); //counter clock
            }
            else
            {
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime); //counter clock
            }
*/

        if (Mathf.Abs(target - transform.rotation.eulerAngles.y) < 0.5f)
            canRotate = false;
    }


    public void Walk(Vector3 position)
    {
        //rb.MovePosition(position * movementSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, position, movementSpeed*Time.deltaTime);

        Debug.Log("walking");
        if (Vector3.Distance(pos,position)<1)
        {
            canMove = false;
            return;
        }

    }

    void TravelTo(Vector3 place)
    {
        if (nv != null)
        {
            nv.SetDestination(place);
        }

    }
}
