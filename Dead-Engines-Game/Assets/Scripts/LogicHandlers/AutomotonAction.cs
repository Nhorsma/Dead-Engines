using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public float movementSpeed, turnSpeed;
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
        }
        if(canRotate && walkTo!= null)
        {
            Rotate(walkTo);
        }
        if(canMove && walkTo!=null)
        {
            Walk(walkTo);
        }
    }


    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            return hit;
        return hit;
    }


    public void Rotate(Vector3 position)
    {
        if(transform.rotation.eulerAngles != pos - position)
        {
            //Vector3 dir = Vector3.RotateTowards(transform.rotation.eulerAngles, pos - position, movementSpeed * Time.deltaTime,0.0f);
            //float num = Vector3.Angle(transform.position,position);
            //transform.eulerAngles = new Vector3(transform.eulerAngles.x,transform.eulerAngles.y + num,transform.eulerAngles.z);

            //Vector3 direction = position - transform.position;
            //Quaternion rotation = Quaternion.LookRotation(direction);
            //transform.up = Vector3.RotateTowards(transform.rotation.eulerAngles,direction,movementSpeed*Time.deltaTime,0.0f);
            //canRotate = false;
            Debug.Log(Vector3.Angle(transform.position, position));
            transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
        }
        else
        {
            canMove = true;
            canRotate = false;
            Debug.Log("start walking");
            return;
        }
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
