﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawn;
    UnitManager um;
    public List<GameObject> enemiesGM;
    public List<Enemy> enemies;
    public float stoppingDistance, shootingRange, tresspassingRange;


    private void Start()
    {
        um = GetComponent<UnitManager>();
        enemiesGM = new List<GameObject>();
        enemies = new List<Enemy>();
        spawn = GetComponent<SpawnRes>();

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
            TravelTo(GetEnemyObject(e), e.Rec.transform.position + rand, true);
        }
        else
        {
            Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
            TravelTo(GetEnemyObject(e), e.Camp.transform.position + rand, true);
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
                TravelTo(GetEnemyObject(e), um.GetUnitObject(u).transform.position, true);
                break;
            }
        }
    }

    void Persue(Enemy e)
    {
        float dis = Vector3.Distance(GetEnemyObject(e).transform.position, e.Target.transform.position);

        if(dis<shootingRange && !e.JustShot)
        {
            Fire(e);
            StartCoroutine(FireCoolDown(e));
        }
        else if(dis>shootingRange && dis<tresspassingRange)
        {
            TravelTo(GetEnemyObject(e), e.Target.transform.position, true);
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
        Vector3 direction = ene.Target.transform.position - GetEnemyObject(ene).transform.position;

        RaycastHit hit;
        if (Physics.Raycast(GetEnemyObject(ene).transform.position, direction, out hit, 100f))
        {
            if (hit.collider.tag == "Friendly")
                Debug.Log("Bang!");
        }
    }

    void TravelTo(GameObject a, Vector3 place, bool stop)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshAgent nv = a.GetComponent<NavMeshAgent>();
            if (stop)
                nv.stoppingDistance = stoppingDistance;

            nv.SetDestination(place);
        }
        else if(a.GetComponent<NavMeshAgent>() == null)
        {
            //code for the APC since it bugs out

//            Vector3 d = a.transform.position - place;
            a.transform.position = Vector3.MoveTowards(a.transform.position, place, Time.deltaTime * 2f);

        }
    }
}