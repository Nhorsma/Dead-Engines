using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutomotonAction : MonoBehaviour
{
    public Animator anim;

    public float movementSpeed, startMovementSpeed, fastMovementSpeed, slowMovementSpeed, startTurnSpeed, turnSpeed;
    private float startAngle, target, ny;
    private bool canMove, canRotate, isWalking, rotLeft, rotRight, hasEnoughFuel;
    private bool tempmove, temprotate, tempwalking, tempLeft, tempRight;
    Vector3 pos, walkTo;
    NavMeshAgent nv;
    Rigidbody rb;
    int layer_mask;

    public bool endPhaseOne;
    public bool isSelected, isCrouched, canAct, canLazer, canBarrage, overclocked, wellOiled, reinforced;
    public GameObject automoton, fog, footObject, fistObject, headObject, dustCloud, explosion, lazer, crossHair;
    public Vector3 phaseOnePos, phaseTwoPos;
    public Animation climbOut;
    public AutomotonAction aa;
    public UnitManager unitManager;
    public AudioSource robotAmbientSource;
    public AudioHandler audioHandler;
    public SpawningPoolController spawnPool;
    public HunterHandler hunterHandler;
    public EncampmentHandler encampHandler;
    public EnemyHandler enemyHandler;
 //   public ControllerUpgrades controlUpgrades;

    KeyCode move_q, move_w, move_e, move_r, move_f;
    Collider footCollider, fistCollider;
    GameObject jobObject;

    public int autoHealth;
    public int startingAutoHealth;
    public int lazerDamage, gunDamage, meleeDamage, lazerCost, stompCost, punchCost, barrageCost;

    private void Start()
    {
        endPhaseOne = canAct = false;
        canMove = canRotate = isWalking = isSelected = isCrouched = false;
        canLazer = canBarrage = overclocked = wellOiled = reinforced = false;
        movementSpeed = startMovementSpeed;
        phaseTwoPos = phaseOnePos = automoton.transform.position;

        anim = automoton.GetComponent<Animator>();
        aa = automoton.GetComponent<AutomotonAction>();

        footCollider = footObject.GetComponent<BoxCollider>();
        fistCollider = fistObject.GetComponent<BoxCollider>();
        footCollider.enabled = false;
        fistCollider.enabled = false;
        autoHealth = startingAutoHealth;
    }

    private void LateUpdate()
    {
        if (endPhaseOne && isSelected && autoHealth>0 && canAct)
        {
            RightClick();
            Controls();
        }
        CrossHairControl();
        Movement();
        HaveEnoughOil();

        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            movementSpeed = startMovementSpeed * 10;
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            autoHealth = 1000;
        }
    }

    public void StartAuto()
    {
        layer_mask = LayerMask.GetMask("Ignore Raycast");
        robotAmbientSource.Play();
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canLazer = canBarrage = true;

        phaseTwoPos = phaseOnePos = automoton.transform.position;
        phaseTwoPos -= new Vector3(13.2f, -41.49f, 12.3f);
        endPhaseOne = canAct = hasEnoughFuel = true;

        DefaultControls();
        //unitManager.PhaseTwoUnits();
        autoHealth = startingAutoHealth;
        StartCoroutine(RaiseAuto());
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
                Hit().collider.gameObject.tag == "Encampment"   ||
                Hit().collider.gameObject.tag == "Hunter"   ||
                Hit().collider.gameObject.tag == "Enemy" ||
                Hit().collider.gameObject.tag == "Oil")
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

    public void WalkStraight(Vector3 position)
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
            audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm1");
            anim.SetBool("isWalking", false);
            canMove = false;
            canRotate = true;
            turnSpeed = startTurnSpeed;
            walkTo = Hit().point;
            startAngle = transform.rotation.eulerAngles.y;
            SetUpRotate(walkTo);
    }

    void JobManagement()
    {
        if (jobObject != null)
        {
            Vector3 jobPos = jobObject.transform.position;
            if ((jobObject.tag == "Metal" || jobObject.tag == "Electronics" || jobObject.tag == "Encampment") &&
                Vector3.Distance(automoton.transform.position, jobPos) < 20f + automoton.transform.position.y - jobPos.y)
            {
                StartCoroutine(GroundPound());
                jobObject = null;
            }
            else if (jobObject.tag == "Enemy" || jobObject.tag == "Hunter")
            {
                walkTo = new Vector3(jobPos.x, phaseTwoPos.y, jobPos.z);
            }
            else if(jobObject.tag == "Oil" && Vector3.Distance(automoton.transform.position, jobPos) < 15f + automoton.transform.position.y - jobPos.y)
            {
                StartCoroutine(CrouchDown(7));
                jobObject = null;
            }
        }
    }

    void Movement()
    {
            walkTo = new Vector3(walkTo.x, phaseTwoPos.y, walkTo.z);
            JobManagement();
            if (canRotate && walkTo != null)
            {
                Rotate();
            }
            if (canMove && walkTo != null)
            {
                WalkStraight(walkTo);
            }
    }


    //========================================================================================


    IEnumerator RaiseAuto()
    {
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
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
        move_f = KeyCode.F;
    }


    void Controls()
    {
        if (canAct)
        {
            int fuel = ResourceHandling.oil;
            if (Input.GetKeyDown(move_q))
            {
                if (fuel >= stompCost)
                    StartCoroutine(GroundPound());
                else
                    audioHandler.PlayClip(Camera.main.gameObject, "error");
            }
            if (Input.GetKeyDown(move_w))
            {
                if (fuel >= punchCost)
                    StartCoroutine(Punch());
                else
                    audioHandler.PlayClip(Camera.main.gameObject, "error");
            }
            if (canLazer && Input.GetKeyDown(move_e))
            {
                if (fuel >= lazerCost)
                    StartCoroutine(Laser());
                else
                    audioHandler.PlayClip(Camera.main.gameObject, "error");
            }
            if(canBarrage && Input.GetKeyDown(move_r))
            {
                if (fuel >= barrageCost)
                    StartCoroutine(GunBarrage());
                else
                    audioHandler.PlayClip(Camera.main.gameObject, "error");
            }
            if(Input.GetKeyDown(move_f))
            {
                if (!isCrouched)
                    StartCoroutine(CrouchDown());
                else
                    StartCoroutine(CrouchUp());
            }
        }
    }

    IEnumerator GroundPound()
    {
        SpendOil(stompCost);
        canAct = false;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
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
        SpendOil(punchCost);
        canAct = false;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
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

    IEnumerator CrouchDown()
    {
        canAct = false;
        isCrouched = true;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
        ContinueAnimations(false);
        anim.SetBool("isCrouching", true);
        yield return new WaitForSeconds(3f);
        canAct = true;
    }

    IEnumerator CrouchDown(float time)
    {
        canAct = false;
        isCrouched = true;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
        ContinueAnimations(false);
        anim.SetBool("isCrouching", true);
        fistCollider.enabled = true;
        yield return new WaitForSeconds(time);
        StartCoroutine(CrouchUp());
        fistCollider.enabled = false;
    }

    IEnumerator CrouchUp()
    {
        canAct = false;
        isCrouched = false;
        anim.SetBool("isCrouching", false);
        yield return new WaitForSeconds(3f);
        ContinueAnimations(true);
        canAct = true;
    }

    IEnumerator Laser()
    {
        SpendOil(lazerCost);
        canAct = false;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
        ContinueAnimations(false);
        anim.SetBool("inLaser", true);
        audioHandler.PlayClip(gameObject, "bigLaz");
        yield return new WaitForSeconds(2.7f);
        ShootLaser();

        yield return new WaitForSeconds(2f);
        anim.SetBool("inLaser", false);

        yield return new WaitForSeconds(3f);
        canAct = true;
    }

    void ShootLaser()
    {
        Vector3 head = headObject.transform.position;
        Vector3 shootAt = walkTo;
        if (jobObject != null)
        {
            shootAt = jobObject.GetComponent<BoxCollider>().bounds.center;
        }
        StartCoroutine(TrailOff("autoLaz",0.5f, head, shootAt));
    }

    IEnumerator TrailOff(string type, float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(type, start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary[type].Enqueue(t);
        t.SetActive(false);
    }

    public GameObject BulletTrail(string type, Vector3 start, Vector3 end)
    {
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);

        GameObject trail;
        trail = spawnPool.poolDictionary[type].Dequeue();

        trail.transform.position = start - dif;
        trail.transform.rotation = angle;
        trail.SetActive(true);

        trail.transform.localScale = new Vector3(1f, 1f, Vector3.Distance(start, end));
        return trail;
    }

    IEnumerator GunBarrage()
    {
        SpendOil(barrageCost);
        canAct = false;
        audioHandler.PlayClip(Camera.main.gameObject, "robotConfirm2");
        yield return new WaitForSeconds(0.75f);
        StartCoroutine(ShootGun());
        yield return new WaitForSeconds(0.75f);

        canAct = true;
    }

    IEnumerator ShootGun()
    {
        float x, y, z;
        x = Random.Range(-10f, 10f);
        y = Random.Range(-10f, 10f);
        z = Random.Range(-10f, 10f);
        Vector3 offset = new Vector3(x, y, z);

        Vector3 shootAt = walkTo + offset;
        if (jobObject != null)
            shootAt = jobObject.transform.position;
        audioHandler.PlayClip(automoton, "machineGun");
        for (int i = 0; i < 10; i++)
        {
            yield return new WaitForSeconds(0.15f);
            StartCoroutine(TrailOff("unitLaz",0.05f, transform.position + new Vector3(0, 50f, 0), shootAt));
            if(jobObject!=null)
            {
                if(jobObject.tag == "Hunter")
                {
                    hunterHandler.DealHunterDamage(jobObject, gunDamage);
                }
                else if(jobObject.tag == "Encampment")
                {
                    jobObject.GetComponent<Encampment>().Health -= 5;
                    encampHandler.BeDestroyed();
                }
                else if(jobObject.tag == "Enemy")
                {
                    enemyHandler.TakeDamage(gunDamage,jobObject.GetComponent<Enemy>());
                }
            }
        }
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
            audioHandler.PlayClip(gameObject, "robotConfirm1");
        }
    }


    public void RecieveDamage(int amount)
    {
        autoHealth -= amount;

        if(autoHealth == startingAutoHealth*0.5f)
        {
            audioHandler.PlayClip(gameObject, "robotAlarm");
        }
        else if (autoHealth == startingAutoHealth * 0.25f)
        {
            audioHandler.PlayClip(gameObject, "robotAlarm");
        }
        else if(autoHealth <= 0)
        {
            SpawnExplosion(gameObject);
            audioHandler.PlayClipIgnore(gameObject, "explosion");
            StartCoroutine(DeathSequence(3));
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

    IEnumerator DeathSequence(float time)
    {
        yield return new WaitForSeconds(time);
        SceneChanger.ReturnToMenu();
    }

    public void SpendOil(float amount)
    {
        if(wellOiled)
        {
            amount *= 0.5f;
        }

        if(ResourceHandling.oil > amount)
        {
            ResourceHandling.oil -= (int)amount;
        }
        else
        {
            ResourceHandling.oil = 0;
        }
    }

    public void HaveEnoughOil()
    {
        if(ResourceHandling.oil <= 0)
        {
            hasEnoughFuel = false;
            movementSpeed = slowMovementSpeed;
            anim.speed = slowMovementSpeed / startMovementSpeed;
        }
        else
        {
            hasEnoughFuel = true;
            if(overclocked)
            {
                movementSpeed = fastMovementSpeed;
                anim.speed = 1;
            }
            else
            {
                movementSpeed = startMovementSpeed;
                anim.speed = fastMovementSpeed / startMovementSpeed;
            }
        }
    }

    void CrossHairControl()
    {
        if(jobObject!=null)
        {
            crossHair.SetActive(true);
            Vector3 job = jobObject.transform.position;
            crossHair.transform.position = new Vector3(job.x, -5, job.z);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }
}
