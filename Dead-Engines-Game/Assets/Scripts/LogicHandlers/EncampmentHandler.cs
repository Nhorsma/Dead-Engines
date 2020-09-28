using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncampmentHandler : MonoBehaviour
{
    public static int metal;
    public static int electronics;
    public int startingHealth;

    public GameObject[] eGM;
    public Encampment[] encamps;
    public EnemyHandler eh;

    public SpawnRes spawn;
    public float spawnTime, spawnDistance;

    string[] depRec3 = { "gun", "gun", "gun" };
    string[] depRec2 = { "gun", "gun", "APC" };
    string[] depRec1 = { "gun", "gun", "APC", "APC" };

    void Start()
    {
        eh = GetComponent<EnemyHandler>();
        eGM = GameObject.FindGameObjectsWithTag("Encampment");
        encamps = new Encampment[eGM.Length];
        SetUpCamps();
    }

    void Update()
    {
        foreach(Encampment e in encamps)
        {
            CheckUnitNear(e);
        }
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------



    //this will establish that spawned enemies travel to this resource...
    //as well as will attack player units that get too close to it.
    void SetUpCamps()
    {
        for (int i = 0; i < encamps.Length; i++)
        {
            encamps[i] = new Encampment(i);
            encamps[i].ClosestRec = GetClosestResource(encamps[i]);
        }
    }


    public Encampment GetEncampment(GameObject gm)
    {
        for (int i = 0; i < eGM.Length; i++)
        {
            if (eGM[i] == gm)
                return encamps[i];
        }
        return null;
    }


    public GameObject GetEncampmentGM(Encampment e)
    {
        return eGM[e.Id];
    }


    void BeDestroyed()
    {
        for (int i = 0; i < encamps.Length; i++)
        {
            if (encamps[i].Health <= 0)
            {
                Destroy(eGM[i]);
                encamps[i] = null;
            }
        }
    }


    void CheckUnitNear(Encampment e)
    {
        foreach (GameObject u in GetComponent<UnitManager>().unitsGM)
        {
            if (Vector3.Distance(u.transform.position, GetEncampmentGM(e).transform.position) < eh.tresspassingRange ||
                Vector3.Distance(u.transform.position, e.ClosestRec.transform.position) < eh.tresspassingRange)
            {
                CheckSpawnEnemy(e);
            }
        }
    }


    void CheckSpawnEnemy(Encampment e)
    {
            if (e.CanSpawn)
            {
                int hit = Random.Range(1, 5);
                if (e.OnField<e.Deployment.Count && hit < e.Chance)
                {
                    SpawnEnemy(e);
                    e.Chance = 0;
                }
                else
                {
                    e.Chance++;
                }
            //Debug.Log(e.Chance + "0%");
                StartCoroutine(ChangeSpawnChance(e));
            }
    }

    IEnumerator ChangeSpawnChance(Encampment e)
    {
        e.CanSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        e.CanSpawn = true;
    }


    //implement Deployment at some point
    void SpawnEnemy(Encampment e)
    {
        CheckDeployment(e);
        Vector3 spawnPlace = eGM[e.Id].transform.position + new Vector3(Random.Range(1, 3), 0, Random.Range(1, 3));
        var gm = Instantiate(Resources.Load(e.Deployment[e.OnField]), spawnPlace, transform.rotation);

        Enemy enemy = new Enemy();
        enemy.Rec = e.ClosestRec;
        enemy.Camp = GetEncampmentGM(e);
        enemy.Id = eh.enemiesGM.Count;

        eh.enemiesGM.Add((GameObject)gm);
        eh.enemies.Add(enemy);

        eh.FindSpot(enemy);
        e.OnField++;
    }


    //adds "gun", "APC", or "Mech"
    void CheckDeployment(Encampment e)
    {
        ResourceHandling r = GetComponent<ResourceHandling>();
        int recAmount = r.GetNumber(e.ClosestRec);

        if(recAmount<=0)
        {
            e.Deployment.Clear();
            return;
        }

        if (r.recsLeft == 3)
        {
            if (recAmount <= 76)
            {
                e.Deployment.Add("gun");

                if (recAmount <= 51)
                {
                    e.Deployment.Add("APC");

                    if (recAmount <= 26)
                    {
                        e.Deployment.Add("gun");
                    }
                }
            }
        }
        else if(r.recsLeft==2)
        {
            if (recAmount <= 76)
            {
                e.Deployment.Add("APC");

                if (recAmount <= 51)
                {
                    e.Deployment.Remove("gun");

                    if (recAmount <= 26)
                    {
                        e.Deployment.Remove("APC");
                        e.Deployment.Remove("APC");
                        e.Deployment.Add("Mech");
                    }
                }
            }
        }
        else if(r.recsLeft==1)
        {
            e.Deployment.Add("APC");

            if (recAmount <= 76)
            {
                e.Deployment.Add("APC");

                if (recAmount <= 51)
                {
                    e.Deployment.Remove("gun");

                    if (recAmount <= 26)
                    {
                        e.Deployment.Remove("APC");
                        e.Deployment.Remove("APC");
                        e.Deployment.Add("Mech");
                    }
                }
            }
        }
    }


    GameObject GetClosestResource(Encampment camp)
    {
        float distance = Mathf.Infinity;
        GameObject chosen = new GameObject();
        foreach(GameObject rec in GetComponent<SpawnRes>().GetResources())
        {
            if(Vector3.Distance(rec.transform.position, GetEncampmentGM(camp).transform.position) < distance)
            {
                distance = Vector3.Distance(rec.transform.position, GetEncampmentGM(camp).transform.position);
                chosen = rec;
            }
        }
        return chosen;
    }


    void SetDeployment(Encampment e, int recsLeft)
    {
        if(recsLeft==2)
        {
            e.Deployment.Clear();
            for(int i=0;i<depRec2.Length;i++)
            {
                e.Deployment.Add(depRec2[i]);
            }
        }
        else if (recsLeft == 1)
        {
            e.Deployment.Clear();
            for (int i = 0; i < depRec2.Length; i++)
            {
                e.Deployment.Add(depRec1[i]);
            }
        }
        else
        {
            Debug.Log("huh, weird");
        }
    }


    //as the player explores and finds new encampments, new encampments should be entered into
    //the array, we might need to make the array a list if it gets too big.
    void ReplaceEncampment()
    {

    }
}
