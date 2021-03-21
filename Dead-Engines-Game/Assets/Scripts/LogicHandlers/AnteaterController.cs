using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnteaterController : MonoBehaviour
{
    public int antHealth, startingAntHealth, closeRange, tooCloseRange;
    public float movementSpeed, startTurnSpeed, turnSpeed, startAngle, target, ny;
    public bool canMove, canRotate, isWalking, rotLeft, rotRight, 
        tempmove, temprotate, tempwalking, tempLeft, tempRight,
        nextMove;

    public GameObject automoton, fistObject, dustCloud, explosion, shootingObject;
    public Collider fistCollider;
    public AutomotonAction aa;
    public Animator anim;
    public SpawningPoolController spawnPool;

    public NavMeshAgent nv;
    public Rigidbody rb;

    Vector3 robotPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        nv = GetComponent<NavMeshAgent>();
        nv.speed = movementSpeed;
        canMove = canRotate = isWalking = false;
        nextMove = true;

        aa = automoton.GetComponent<AutomotonAction>();
        fistCollider = fistObject.GetComponent<BoxCollider>();
        fistCollider.enabled = false;

        antHealth = startingAntHealth;
    }

    void HappyHunting()
    {
        /*
         * make it so that when the anteater retreats, it doesn't infintaly run away,
         * it choses one poitn to retreat to, shoots at robot, then retreats again if necessary,
         * It doesn't chose a new combat-maneuver until it has finsihed the maneuver it is already
         * performing
         */ 


        //determine robot position
        robotPos = automoton.transform.position;
        float distance = Vector3.Distance(robotPos, transform.position);

        if (!checkIfAtDestination())
            nextMove = false;

        if (nextMove)
        {
            if (distance > closeRange) //if very far, walk towards until close
            {
                TravelTo(robotPos);
            }
            else if (distance < tooCloseRange)//if very close, walk backwards
            {
                BackUp(robotPos);
            }
            else
            {
                FindFlank(robotPos);
            }
        }
        //if close, be evasive and walk to beside them
    }

    void Fire(GameObject enemy, Enemy enemy_data)
    {
        nextMove = false;
            float hitChance = 1;
            Vector3 targetPos = new Vector3(enemy_data.Target.transform.position.x, 1, enemy_data.Target.transform.position.z);
            Vector3 direction = targetPos - enemy.transform.position;
            StartCoroutine(TrailOff(0.5f, shootingObject.transform.position, enemy_data.Target.transform.position));
            StartCoroutine(FireCoolDown(hitChance, enemy));
    }
    IEnumerator FireCoolDown(float extratime, GameObject enemy)
    {
        if (enemy != null)
        {
            enemy.GetComponent<Enemy>().JustShot = true;
            yield return new WaitForSeconds(enemy.GetComponent<Enemy>().FiringSpeed + extratime / 2);
            enemy.GetComponent<Enemy>().JustShot = false;
            nextMove = true;
        }
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
        //GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle * offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary["trails"].Enqueue(t);
        t.SetActive(false);
    }

    void ContinueAnimations(bool a)
    {
        if (!a)
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

    void TravelTo(Vector3 place)
    {
        if (nv != null && canMove)
        {
            nv.SetDestination(place);
        }

    }

    bool checkIfAtDestination()
    {
        return nv.remainingDistance < 1f;
    }

    void BackUp(Vector3 backFrom)
    {
        /*
        Vector3 difference = transform.position - backFrom;
        Vector3 a = difference*2;
        Vector3 b = a + backFrom;
        //move to b
        */

        Vector3 backUp = ((transform.position - backFrom) * 2) + backFrom;
        TravelTo(backFrom);
    }

    void FindFlank(Vector3 robot)
    {
        /*
        Vector3 difference = transform.position - backFrom;
        Vector3 a = (difference/z,y,difference.x)
        Vector3 b = a + backFrom;
        //move to b
        */

        Vector3 diff = transform.position - robot;
        Vector3 flank = new Vector3(diff.z, transform.position.y, diff.x);
        robot += flank;
        TravelTo(robot);
    }
}
