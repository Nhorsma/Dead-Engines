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
    public UnitManager unitManager;

    public SpawnRes spawn;
    public ResourceHandling rh;
    public float startSpawnTime,spawnTime, spawnDistance;
    public bool startSpawning;

    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, deathClip, destroyClip;
    public float volume;

    string[] depRec43 = { "gunner", "gunner", "gunner" };
    string[] depRec33 = { "gunner", "APC_2", "gunner" };
    string[] depRec23 = { "gunner", "gunner", "APC_2", "APC_2" };
    string[] depRec13 = { "gunner", "APC_2", "gunner", "APC_2" };

    string[] depRec42 = { "gunner", "gunner", "APC_2" };
    string[] depRec32 = { "gunner", "APC_2", "gunner", "APC_2" };
    string[] depRec22 = { "APC_2", "APC_2", "Mech 2" };
    string[] depRec12 = { "Mech 2", "APC_2", "Mech 2" };

    string[] depRec41 = { "gunner", "gunner", "gunner", "APC_2" };
    string[] depRec31 = { "gunner", "gunner", "APC_2", "APC_2", "Mech 2" };
    string[] depRec21 = { "gunner", "gunner", "gunner", "APC_2", "APC_2", "Mech 2", "Mech 2" };
    string[] depRec11 = { "Mech 2", "gunner", "gunner", "APC_2", "APC_2", "Mech 2", "APC_2" };

    void Start()
    {
        rh = GetComponent<ResourceHandling>();
        eh = GetComponent<EnemyHandler>();
        eGM = GameObject.FindGameObjectsWithTag("Encampment");
        encamps = new Encampment[eGM.Length];
        SetUpCamps(); //
        startSpawning = true;
        startSpawnTime = spawnTime;
    }

    void Update()
    {
        if (startSpawning)
            for (int i = 0; i < encamps.Length; i++)
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


    public void BeDestroyed()
    {
        for (int i = 0; i < encamps.Length; i++)
        {
            if (encamps[i].Health <= 0)
            {
                PlayClip(encamps[i], "death");
                startSpawning = false;
                eGM[i].SetActive(false);
                encamps[i] = null;
            }
            else
            {
                if (encamps[i].Health >= startSpawnTime * 0.75f)
                {
                    spawnTime--;
                }
                if (encamps[i].Health >= startSpawnTime * 0.5f)
                {
                    spawnTime --;
                }
                else if (encamps[i].Health >= startSpawnTime * 0.25f)
                {
                    spawnTime --;
                }
            }
        }
    }


    void CheckUnitNear(Encampment e)
    {
        foreach (GameObject u in unitManager.unitsGM)
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
            if (e.OnField < e.Deployment.Length && hit < e.Chance)
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


    void SpawnEnemy(Encampment e)
    {
        PlayClip(e, "attack");
        CheckDeployment(e);
        Vector3 spawnPlace = eGM[e.Id].transform.position + new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5));

        // for(int i=0;i<e.Deployment.Length;i++)
        // Debug.Log(e.Deployment[i] +" : "+e.OnField);
        var gm = Instantiate(Resources.Load(e.Deployment[e.OnField]), spawnPlace, transform.rotation);

        Enemy enemy = new Enemy();
        enemy.Rec = e.ClosestRec;
        enemy.Camp = GetEncampmentGM(e);
        enemy.Id = eh.enemiesGM.Count;
        enemy.Obj = (GameObject)gm;

        eh.enemiesGM.Add((GameObject)gm);
        eh.enemies.Add(enemy);

        eh.FindSpot(enemy);
        e.OnField++;
    }


    //adds "gunner", "APC", or "Mech"
    void CheckDeployment(Encampment e)
    {
        int quant = rh.resQuantities[rh.GetNumber(e.ClosestRec)];

        if (quant <= 0)
        {
            e.Deployment = new string[50];
            //Debug.Log("whoops");
            return;
        }

        if (rh.recsLeft == 3)
        {
            if (e.Health < 25 || quant < rh.startQuantity / 4)
            {
                e.Deployment = depRec13;
            }
            if (e.Health < 50 || quant < (rh.startQuantity / 2))
            {
                e.Deployment = depRec23;
            }
            else if (e.Health < 75 || quant < rh.startQuantity * 0.25f)
            {
                e.Deployment = depRec33;
            }
            else
            {
                e.Deployment = depRec43;
            }
        }
        else if (rh.recsLeft == 2)
        {
            if (e.Health < 25 || quant < rh.startQuantity / 4)
            {
                e.Deployment = depRec12;
            }
            if (e.Health < 50 || quant < (rh.startQuantity / 2))
            {
                e.Deployment = depRec22;
            }
            else if (e.Health < 75 || quant < rh.startQuantity * 0.25f)
            {
                e.Deployment = depRec32;
            }
            else
            {
                e.Deployment = depRec42;
            }
        }
        else if (rh.recsLeft == 1)
        {
            if (e.Health < 25 || quant < rh.startQuantity / 4)
            {
                e.Deployment = depRec11;
            }
            if (e.Health < 50 || quant < (rh.startQuantity / 2))
            {
                e.Deployment = depRec21;
            }
            else if (e.Health < 75 || quant < rh.startQuantity * 0.25f)
            {
                e.Deployment = depRec31;
            }
            else
            {
                e.Deployment = depRec41;
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
        foreach (GameObject rec in spawn.GetResources())
        {
            if (Vector3.Distance(rec.transform.position, GetEncampmentGM(camp).transform.position) < distance)
            {
                distance = Vector3.Distance(rec.transform.position, GetEncampmentGM(camp).transform.position);
                chosen = rec;
            }
        }
        return chosen;
    }


    public void PlayClip(Encampment encampment, string str)
    {
        AudioSource tempSource = GetEncampmentGM(encampment).GetComponent<AudioSource>();
        Debug.Log(tempSource);
        //tempSource.volume = volume;
        if (str.Equals("attack"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(attackClip1);
            else
                tempSource.PlayOneShot(attackClip2);
        }
        else if (str.Equals("death"))
        {
            tempSource.PlayOneShot(deathClip);
            tempSource.PlayOneShot(destroyClip);
        }
    }
}
