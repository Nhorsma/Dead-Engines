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
    public ResourceHandling rh;
    public float spawnTime, spawnDistance;

    string[] depRec33 = { "gun", "gun", "gun" };
    string[] depRec23 = { "APC", "APC", "APC" };
    string[] depRec13 = { "gun", "gun", "APC", "APC" };

    string[] depRec32 = { "gun", "gun", "APC" };
    string[] depRec22 = { "gun", "gun", "APC", "APC"};
    string[] depRec12 = { "APC", "APC", "Mech" };

    string[] depRec31 = { "gun", "gun", "gun", "APC"};
    string[] depRec21 = { "gun", "gun", "APC", "APC", "Mech" };
    string[] depRec11 = { "gun", "gun", "gun", "APC", "APC", "Mech", "Mech" };

    void Start()
    {
        rh = GetComponent<ResourceHandling>();
        eh = GetComponent<EnemyHandler>();
        eGM = GameObject.FindGameObjectsWithTag("Encampment");
        encamps = new Encampment[eGM.Length];
        SetUpCamps();
    }

    void Update()
    {
        for(int i =0;i<encamps.Length;i++)
        {
            CheckUnitNear(encamps[i]);
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
            encamps[i].Deployment = depRec33;
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
                if (e.OnField<e.Deployment.Length && hit < e.Chance)
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

        for(int i=0;i<e.Deployment.Length;i++)
            Debug.Log(e.Deployment[i] +" : "+e.OnField);
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
        int quant = rh.resQuantities[rh.GetNumber(e.ClosestRec)];

        if (quant<=0)
        {
            e.Deployment = new string[50];
            Debug.Log("whoops");
            return;
        }

        if (rh.recsLeft == 3)
        {
            if(e.Health>75 || quant>rh.startQuantity-(rh.startQuantity/4))
            {
                e.Deployment = depRec33;
            }
            else if(e.Health > 50 || quant > (rh.startQuantity / 2))
            {
                e.Deployment = depRec23;
            }
            else if(e.Health > 25 || quant > rh.startQuantity/4)
            {
                e.Deployment = depRec13;
            }
        }
        else if(rh.recsLeft == 2)
        {
            if (e.Health > 75 || quant > rh.startQuantity - (rh.startQuantity / 4))
            {
                e.Deployment = depRec32;
            }
            else if (e.Health > 50 || quant > (rh.startQuantity / 2))
            {
                e.Deployment = depRec22;
            }
            else if (e.Health > 25 || quant > rh.startQuantity / 4)
            {
                e.Deployment = depRec12;
            }
        }
        else if(rh.recsLeft == 1)
        {
            if (e.Health > 75 || quant > rh.startQuantity - (rh.startQuantity / 4))
            {
                e.Deployment = depRec31;
            }
            else if (e.Health > 50 || quant > (rh.startQuantity / 2))
            {
                e.Deployment = depRec21;
            }
            else if (e.Health > 25 || quant > rh.startQuantity / 4)
            {
                e.Deployment = depRec11;
            }
        }
        else
        {
            return;
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
        
    }
    


    //as the player explores and finds new encampments, new encampments should be entered into
    //the array, we might need to make the array a list if it gets too big.
    void ReplaceEncampment()
    {

    }
}
