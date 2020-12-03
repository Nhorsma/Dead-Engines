using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawn;
    UnitManager um;
    EncampmentHandler eh;
    public List<GameObject> enemiesGM;
    public List<Enemy> enemies;
    public float stoppingDistance, shootingRange, tresspassingRange;
    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, dieClip1, dieClip2, shootClip;

    private void Start()
    {
        um = GetComponent<UnitManager>();
        enemiesGM = new List<GameObject>();
        enemies = new List<Enemy>();
        spawn = GetComponent<SpawnRes>();
        eh = GetComponent<EncampmentHandler>(); //
    }

    private void Update()
    {
        RunJobs();
    }


    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------



    public GameObject GetEnemyObject(Enemy ene)
    {
        return enemiesGM[ene.Id];
    }


    public  Enemy GetEnemy(GameObject gm)
    {
        for (int i = 0; i < enemiesGM.Count; i++)
        {
            if (enemiesGM[i] == gm)
                return enemies[i];
        }
        return null;
    }


    void RunJobs()
    {
        foreach (Enemy e in enemies)
        {
            if(e.Target!=null)
            {
                Persue(e);
            }
            else
            {
                Protect(e);
            }
        }
    }


    public void FindSpot(Enemy e)
    {
        int chance = Random.Range(0, 2);
        if(chance==1)
        {
            Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            TravelTo(GetEnemyObject(e), e.Rec.transform.position + rand, true, false);
        }
        else
        {
            Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            TravelTo(GetEnemyObject(e), e.Camp.transform.position + rand, true, false);
        }
    }


    //i was hoping i wouldn't have to loop through every friendly game object to see if they were in range...
    //...but you leave me no choice

    /*
    * if tresspassing, move to firing range
    * if in firing range and still in tresspassing, shoot and follow
    * if in firing range but not tresspassing, shoot
    * if not in firing range nor tresspassing, find spot
    */


    void Protect(Enemy e)
    {
        foreach (Unit u in um.units)
        {
            if (Vector3.Distance(um.GetUnitObject(u).transform.position, e.Rec.transform.position) < tresspassingRange
                || Vector3.Distance(um.GetUnitObject(u).transform.position, e.Camp.transform.position) < tresspassingRange)
            {
                e.Target = um.GetUnitObject(u);
                TravelTo(GetEnemyObject(e), um.GetUnitObject(u).transform.position, true, true);
                break;
            }
        }
    }

    void Persue(Enemy e)
    {
        float dis = Vector3.Distance(GetEnemyObject(e).transform.position, e.Target.transform.position);

        if(e.Target != null)
        if ( dis < shootingRange && !e.JustShot)
        {
            Fire(e);
            StartCoroutine(FireCoolDown(e));
        }
        else if(dis>shootingRange && dis<tresspassingRange)
        {
            TravelTo(GetEnemyObject(e), e.Target.transform.position, true, true);
        }
        else if(dis>tresspassingRange && dis>shootingRange)
        {
            FindSpot(e);
            e.Target = null;
        }
    }


    IEnumerator FireCoolDown(Enemy ene)
    {
        ene.JustShot = true;
        yield return new WaitForSeconds(1f);
        ene.JustShot = false;
    }

    void Fire(Enemy ene)
    {
        if (ene.Target != null)
        {
            Vector3 direction = ene.Target.transform.position - GetEnemyObject(ene).transform.position;

            RaycastHit hit;
            if (Physics.Raycast(GetEnemyObject(ene).transform.position, direction, out hit, 100f))
            {
                PlayClip(ene.Camp,"shoot");
                StartCoroutine(TrailOff(0.05f, GetEnemyObject(ene).transform.position, ene.Target.transform.position));
                if (hit.transform.tag == "Friendly")
                {
                    Debug.Log("hit");
                    um.GetUnit(hit.collider.gameObject).Health--;
                    um.UnitDown(um.GetUnit(hit.collider.gameObject));
                }
            }
        }
    }

    void TravelTo(GameObject a, Vector3 place, bool stop, bool randomize)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshAgent nv = a.GetComponent<NavMeshAgent>();
            if (stop)
                nv.stoppingDistance = stoppingDistance;
            if (randomize)
                place += new Vector3(Random.Range(-stoppingDistance, stoppingDistance), 0, Random.Range(-stoppingDistance, stoppingDistance));

            nv.SetDestination(place);
        }
    }

    GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);
        GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }

    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
    }

    public void EnemyDead(Enemy e)
    {
        if (e.Health <= 0)
        {
            Destroy(GetEnemyObject(e));
            eh.GetEncampment(e.Camp).OnField--; //
            enemies.Remove(e);
            enemiesGM.Remove(GetEnemyObject(e));
        }
    }

    void PlayClip(GameObject encamp, string str)
    {
        AudioSource tempSource = encamp.GetComponent<AudioSource>();
        if (str.Equals("attack"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(attackClip1);
            else
                tempSource.PlayOneShot(attackClip2);
        }
        else if (str.Equals("die"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(dieClip1);
            else
                tempSource.PlayOneShot(dieClip2);
        }
        else if (str.Equals("shoot"))
            tempSource.PlayOneShot(shootClip);
    }
}
