using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public Animator anim;
    public float movementSpeed, startTurnSpeed, turnSpeed;
    public float startAngle, target, ny;
    public bool canMove,canRotate, isWalking, rotLeft, rotRight, canAct;
    public bool tempmove, temprotate, tempwalking, tempLeft, tempRight;
    Vector3 pos, walkTo;
    NavMeshAgent nv;
    Rigidbody rb;
    int layer_mask;

    public static bool endPhaseOne;
    public bool isSelected;
    public GameObject automoton, fog, footObject, fistObject, headObject, dustCloud, explosion;
    public Vector3 phaseOnePos, phaseTwoPos;
    public Animation climbOut;
    public AutomotonAction aa;
    public UnitManager unitManager;
    public AudioSource robotSources;
    public AudioHandler audioHandler;
    public SpawningPoolController spawnPool;

    KeyCode move_q, move_w, move_e, move_r;
    Collider footCollider, fistCollider;
    GameObject jobObject;

    public int autoHealth;
    public int startingAutoHealth;


    private void Start()
    {
        layer_mask = LayerMask.GetMask("Ignore Raycast");
        robotSources.enabled = true;
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canMove = canRotate = isWalking = isSelected = false;

        phaseTwoPos = phaseOnePos = automoton.transform.position;
        phaseTwoPos -= new Vector3(13.2f, -41.49f, 12.3f);

        anim = automoton.GetComponent<Animator>();
        aa = automoton.GetComponent<AutomotonAction>();
        //aa.enabled = false;
        endPhaseOne = canAct = true;

        footCollider = footObject.GetComponent<BoxCollider>();
        fistCollider = fistObject.GetComponent<BoxCollider>();
        footCollider.enabled = false;
        fistCollider.enabled = false;

        DefaultControls();
        //unitManager.PhaseTwoUnits();
        autoHealth = startingAutoHealth;
        StartCoroutine(RaiseAuto());
    }

    private void LateUpdate()
    {
        if (endPhaseOne && isSelected && autoHealth>0)
        {
            RightClick();
            Controls();
        }
        Movement();
    }

    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ~layer_mask))//, 1<<8,layer_mask
        {
            return hit;
        }
        return hit;
    }


    void RightClick()
    {
        if(Input.GetMouseButtonDown(1) && Hit().point != null)
        {
            if (Hit().collider.gameObject.tag == "Metal" ||
                Hit().collider.gameObject.tag == "Electronics" ||
                Hit().collider.gameObject.tag == "Encampment")
            {
                SetJob(Hit().collider.gameObject);
            }
            else
            {
                jobObject = null;
            }
            StartMovement();
        }
    }

    void SetJob(GameObject target)
    {
        jobObject = target;
        Debug.Log("new job");
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
        //Debug.Log("Angle: " + Mathf.Abs(target - transform.rotation.eulerAngles.y));
        float angle = Mathf.Abs(target - transform.rotation.eulerAngles.y);
        if (startAngle < 180)
        {
            anim.SetBool("isWalking", false);
            if (startAngle < target && target < ny)
            {
                //Debug.Log("clockwise");
                anim.SetBool("isRotatingLeft", false);
                anim.SetBool("isRotatingRight", true);
                isWalking = false;
                rotRight = true;
                rotLeft = false;
 //               canMove = false;
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //Debug.Log("counter clockwise");
                anim.SetBool("isRotatingRight", false);
                anim.SetBool("isRotatingLeft", true);
                isWalking = false;
                rotRight = false;
                rotLeft = true;
 //               canMove = false;
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
        else
        {
            if (startAngle < target && target < 360 || target < ny)
            {
               // Debug.Log("clockwise");
                anim.SetBool("isRotatingLeft", false);
                anim.SetBool("isRotatingRight", true);
                isWalking = false;
                rotRight = true;
                rotLeft = false;
//               canMove = false;
                transform.Rotate(Vector3.up * turnSpeed * Time.deltaTime);
            }
            else
            {
                //Debug.Log("counter clockwise");
                anim.SetBool("isRotatingRight", false);
                anim.SetBool("isRotatingLeft", true);
                isWalking = false;
                rotRight = false;
                rotLeft = true;
 //               canMove = false;
                transform.Rotate(-Vector3.up * turnSpeed * Time.deltaTime);
            }
        }
        if(Mathf.Abs(target - transform.rotation.eulerAngles.y) < 1.5f)
        {
            turnSpeed /= 1.5f;
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
        transform.position = Vector3.MoveTowards(transform.position, position, movementSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, position)<1)
        {
            canMove = false;
            anim.SetBool("isWalking", false);
            return;
        }

    }

    void StartMovement()
    {
        pos = transform.position;
            audioHandler.PlayClip(gameObject, "robotConfirm1");
            anim.SetBool("isWalking", false);
            canMove = false;
            canRotate = true;
            turnSpeed = startTurnSpeed;
            walkTo = Hit().point;
            startAngle = transform.rotation.eulerAngles.y;
            SetUpRotate(walkTo);
    }

    void Movement()
    {
        if (jobObject != null && Vector3.Distance(automoton.transform.position, jobObject.transform.position) < 15f
            + automoton.transform.position.y - jobObject.transform.position.y)
        {
            StartCoroutine(GroundPound());
            jobObject = null;
            
        }
        else
        {
            if (canRotate && walkTo != null)
            {
                Rotate();
            }
            if (canMove && walkTo != null)
            {
                Walk(walkTo);
            }
        }
    }


    //========================================================================================


    IEnumerator RaiseAuto()
    {
        audioHandler.PlayClip(gameObject, "robotConfirm2");
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
        if (canAct)
        {
            if (Input.GetKeyDown(move_q))
            {
                StartCoroutine(GroundPound());
            }
            if (Input.GetKeyDown(move_w))
            {
                StartCoroutine(Punch());
            }
            if (Input.GetKeyDown(move_e))
            {
                StartCoroutine(Laser());
            }
        }
    }

    IEnumerator GroundPound()
    {
        canAct = false;
        audioHandler.PlayClip(gameObject, "robotConfirm2");
        ContinueAnimations(false);
        anim.SetBool("GroundPound", true);
        yield return new WaitForSeconds(2f);
        SpawnDust(footObject);
        audioHandler.PlayClip(gameObject, "robotGound");
        //hit ground
        footCollider.enabled = true;
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;
        footCollider.enabled = false;
        anim.SetBool("GroundPound", false);
        ContinueAnimations(true);
        canAct = true;

    }

    IEnumerator Punch()
    {
        canAct = false;
        audioHandler.PlayClip(gameObject, "robotConfirm2");
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
        canAct = true;
    }

    IEnumerator Laser()
    {
        canAct = false;
        audioHandler.PlayClip(gameObject, "robotConfirm2");
        ContinueAnimations(false);
        anim.SetBool("inLaser", true);
        audioHandler.PlayClip(gameObject, "bigLaz");

        //      while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        //          yield return null;
        yield return new WaitForSeconds(2f);

        //shoot lazer [here]
        ShootLaser();

        yield return new WaitForSeconds(2f);
        anim.SetBool("inLaser", false);

        yield return new WaitForSeconds(3f);
        ContinueAnimations(true);
        canAct = true;
    }

    void ShootLaser()
    {
        Vector3 head = headObject.transform.position;
        Vector3 shootAt = walkTo;
        if (jobObject != null)
            shootAt = jobObject.transform.position;
        StartCoroutine(TrailOff(0.5f, head, shootAt));
    }

    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary["trails"].Enqueue(t);
        t.SetActive(false);
    }

    public GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);

        GameObject trail = spawnPool.poolDictionary["trails"].Dequeue();
        trail.transform.position = start - dif;
        trail.transform.rotation = angle * offset;
        trail.SetActive(true);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
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
        else if(autoHealth == startingAutoHealth*0.5f)
        {
            audioHandler.PlayClip(gameObject, "robotAlarm");
        }
        else if (autoHealth == startingAutoHealth * 0.25f)
        {
            audioHandler.PlayClip(gameObject, "robotAlarm");
        }
        else if(autoHealth == 0)
        {
            audioHandler.PlayClip(gameObject, "robotAlarm");
        }
        else
        {
            SpawnExplosion(gameObject);
            audioHandler.PlayClip(gameObject, "explosion");
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

    public void SetSeleted(bool set,Color c)
    {
        isSelected = set;
        transform.Find("Ring").GetComponent<SpriteRenderer>().color = c;
    }
}
