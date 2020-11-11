using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public Animator anim;
    public float movementSpeed, turnSpeed;
    public float startAngle, target, ny;
    public bool canMove,canRotate, isWalking, isRotating;
    Vector3 pos, walkTo;
    NavMeshAgent nv;
    Rigidbody rb;


    private void Start()
    {
        gameObject.layer = 1;
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canMove = canRotate = isWalking = isRotating = false;

    }

    private void LateUpdate()
    {
        Movement();
        QWER();
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
        if(startAngle < 180)
        {
            if(startAngle < target && target < ny)
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
            if (startAngle < target && target < 360 || target < ny)
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
        if (Mathf.Abs(target - transform.rotation.eulerAngles.y) < 0.5f)
        {
            canRotate = false;
            anim.SetBool("isRotating", false);
            canMove = true;
            anim.SetBool("isWalking", true);
        }
    }

    public void Walk(Vector3 position)
    {
        //rb.MovePosition(position * movementSpeed * Time.deltaTime);
        transform.position = Vector3.MoveTowards(transform.position, position, movementSpeed*Time.deltaTime);
        
        if (Vector3.Distance(pos,position)<1)
        {
            canMove = false;
            anim.SetBool("isWalking", false);
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

    void Movement()
    {
        pos = transform.position;
        if (Input.GetMouseButtonDown(1) && Hit().point != null)
        {
            walkTo = Hit().point;
            if (Hit().collider == gameObject)
            {
                Physics.IgnoreLayerCollision(1, 1);
                RaycastHit hit2;
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit2, Mathf.Infinity))
                    walkTo = hit2.point;
                Physics.IgnoreLayerCollision(1, 1, false);

            }
            canRotate = true;
            startAngle = transform.rotation.eulerAngles.y;
            SetUpRotate(walkTo);
        }
        if (canRotate && walkTo != null)
        {
            Rotate();
        }
        if (canMove && walkTo != null)
        {
            Walk(walkTo);
        }

    }

    void QWER()
    {

    }

    void GroundPound()
    {

    }

    void Punch()
    {

    }
}
