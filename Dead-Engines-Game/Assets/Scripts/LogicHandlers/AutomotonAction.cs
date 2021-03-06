﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public Animator anim;
    public AudioClip hitgroundClip, robotSounds, confirmClip1, confirmClip2, alarmClip;
    public AudioSource audioSource;
    public float movementSpeed, turnSpeed;
    public float startAngle, target, ny;
    public bool canMove,canRotate, isWalking, rotLeft, rotRight;
    public bool tempmove, temprotate, tempwalking, tempLeft, tempRight;
    Vector3 pos, walkTo;
    NavMeshAgent nv;
    Rigidbody rb;

    public static bool endPhaseOne;
    public GameObject automoton, fog, footObject, fistObject, dustCloud,explosion;
    public Vector3 phaseOnePos, phaseTwoPos;
    public Animation climbOut;
    public AutomotonAction aa;
    public UnitManager unitManager;

    KeyCode move_q, move_w, move_e, move_r;
    Collider footCollider, fistCollider;

    public int autoHealth;
    public int startingAutoHealth;


    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.Play();
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canMove = canRotate = isWalking = false;

        phaseTwoPos = phaseOnePos = automoton.transform.position;
        phaseTwoPos -= new Vector3(13.2f, -41.49f, 12.3f);

        anim = automoton.GetComponent<Animator>();
        aa = automoton.GetComponent<AutomotonAction>();
        //aa.enabled = false;
        endPhaseOne = true;

        footCollider = footObject.GetComponent<BoxCollider>();
        fistCollider = fistObject.GetComponent<BoxCollider>();
        footCollider.enabled = false;
        fistCollider.enabled = false;

        StartCoroutine(RaiseAuto());
        DefaultControls();
        unitManager.PhaseTwoUnits();

        autoHealth = startingAutoHealth;
    }

    private void LateUpdate()
    {
        if (endPhaseOne && autoHealth>0)
        {
            Movement();
            Controls();
        }
    }

    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, 1<<8))
        {
            return hit;
        }
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
                anim.SetBool("isRotatingLeft", false);
                anim.SetBool("isRotatingRight", true);
                anim.SetBool("isWalking", false);
                isWalking = false;
                rotRight = true;
                rotLeft = false;
                canMove = false;
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //counter clockwise
                anim.SetBool("isRotatingRight", false);
                anim.SetBool("isRotatingLeft", true);
                anim.SetBool("isWalking", false);
                isWalking = false;
                rotRight = false;
                rotLeft = true;
                canMove = false;
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (startAngle < target && target < 360 || target < ny)
            {
                //clockwise
                anim.SetBool("isRotatingLeft", false);
                anim.SetBool("isRotatingRight", true);
                anim.SetBool("isWalking", false);
                isWalking = false;
                rotRight = true;
                rotLeft = false;
                canMove = false;
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //counter clockwise
                anim.SetBool("isRotatingRight", false);
                anim.SetBool("isRotatingLeft", true);
                anim.SetBool("isWalking", false);
                isWalking = false;
                rotRight = false;
                rotLeft = true;
                canMove = false;
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
        if (Mathf.Abs(target - transform.rotation.eulerAngles.y) < 0.5f)
        {
            canRotate = rotLeft = rotRight = false;
            anim.SetBool("isRotatingRight", false);
            anim.SetBool("isRotatingLeft", false);
            canMove = true;
            anim.SetBool("isWalking", true);
            isWalking = true;
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
        if (nv != null && canMove)
        {
            nv.SetDestination(place);
        }

    }

    void Movement()
    {
        pos = transform.position;
        if (Input.GetMouseButtonDown(1) && Hit().point != null)
        {
            PlayClip("confirm1");
            walkTo = Hit().point;
            canMove = false;
            anim.SetBool("isWalking", false);
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


    //========================================================================================


    IEnumerator RaiseAuto()
    {
        PlayClip("confirm2");
        automoton.transform.position = phaseTwoPos;
        anim.SetBool("StartPhaseTwo", true);
        yield return new WaitForSeconds(1f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        endPhaseOne = true;
        anim.SetBool("StartPhaseTwo", false);
    }


    void DefaultControls()
    {
        move_q = KeyCode.Q;
        move_w = KeyCode.W;
        move_e = KeyCode.E;
        move_r = KeyCode.R;
    }


    void Controls()
    {
        if(Input.GetKeyDown(move_q))
        {
            StartCoroutine(GroundPound());
        }
        if(Input.GetKeyDown(move_w))
        {
            StartCoroutine(Punch());
        }
    }

    IEnumerator GroundPound()
    {
        //play ground pound animation
        PlayClip("confirm2");
        ContinueAnimations(false);
        anim.SetBool("GroundPound", true);
        yield return new WaitForSeconds(2f);
        SpawnDust(footObject);
        PlayClip("hit");
        //hit ground
        footCollider.enabled = true;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        footCollider.enabled = false;
        anim.SetBool("GroundPound", false);
        ContinueAnimations(true);

    }

    IEnumerator Punch()
    {
        PlayClip("confirm2");
        ContinueAnimations(false);
        anim.SetBool("Punch", true);
        yield return new WaitForSeconds(1f);
        fistCollider.enabled = true;

        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        Debug.Log("Punch");
        anim.SetBool("Punch", false);
        fistCollider.enabled = false;
        fistCollider.enabled = false;
        ContinueAnimations(true);
    }

    void Laser()
    {

    }

    void GunBattery()
    {

    }

    void ContinueAnimations(bool a)
    {
        if(!a)
        {
            tempmove = canMove;
            temprotate = canRotate;
            tempwalking = isWalking;
            tempLeft = rotLeft;
            tempRight = rotRight;
            canMove = canRotate = isWalking = false;
            anim.SetBool("isRotatingLeft", false);
            anim.SetBool("isRotatingRight", false);
            anim.SetBool("isWalking", false);
        }
        else
        {
            canMove = tempmove;
            canRotate = temprotate;
            isWalking = tempwalking;
            rotLeft = tempLeft;
            rotRight = tempRight;
            anim.SetBool("isRotatingLeft", tempLeft);
            anim.SetBool("isRotatingRight", tempRight);
            anim.SetBool("isWalking", tempmove);
        }
    }


    public void RecieveDamage(int amount)
    {
        autoHealth -= amount;

        if(autoHealth >= startingAutoHealth*0.75f)
        {
            
        }
        else if(autoHealth >= startingAutoHealth*0.5f)
        {
            audioSource.PlayOneShot(alarmClip);
        }
        else if (autoHealth >= startingAutoHealth * 0.25f)
        {
            audioSource.PlayOneShot(alarmClip);
        }
        else if(autoHealth >= 0)
        {
            audioSource.PlayOneShot(alarmClip);
        }
        else
        {
            SpawnExplosion(gameObject);
            PlayClip("hit");
            Debug.Log("auto is Dead");
        }

    }

    void SpawnDust(GameObject obj)
    {
        var expl = (GameObject)Instantiate(Resources.Load(dustCloud.name), obj.transform.position+new Vector3(0,2,0), Quaternion.Euler(90,0,0));
        StartCoroutine(TrailOff(5, expl));
    }

    void SpawnExplosion(GameObject obj)
    {
        var expl = (GameObject)Instantiate(Resources.Load(explosion.name), obj.transform.position + new Vector3(0, 50, 0), Quaternion.Euler(90, 0, 0));
        StartCoroutine(TrailOff(5, expl));
    }

    IEnumerator TrailOff(float time, GameObject explosion)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time);
        Destroy(explosion);
    }

    public void PlayClip(string str)
    {
        if (str == "hit")
            audioSource.PlayOneShot(hitgroundClip);
        else if (str == "confirm1")
            audioSource.PlayOneShot(confirmClip1);
        else if (str == "confirm2")
            audioSource.PlayOneShot(confirmClip2);
        else if (str == "alarm")
            audioSource.PlayOneShot(alarmClip);
    }
}
