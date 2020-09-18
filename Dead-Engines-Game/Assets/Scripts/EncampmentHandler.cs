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
    public float spawnTime;

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
            CheckSpawnEnemy(e);
        }
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------

    void SetUpCamps()
    {
        for (int i = 0; i < encamps.Length; i++)
        {
            encamps[i] = new Encampment(i);
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


    void CheckSpawnEnemy(Encampment e)
    {
        if(e.CanSpawn)
        {
            int hit = Random.Range(1, 10);
            if(hit < e.Chance)
            {
            //    Debug.Log("Spawn");
                SpawnEnemy(e);
                e.Chance = 0;
            }
            else
            {
            //    Debug.Log(e.Chance + "0%");
                e.Chance++;
            }
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
        Vector3 spawnPlace = eGM[e.Id].transform.position + new Vector3(Random.Range(1, 3), 0, Random.Range(1, 3));
        var gm = Instantiate(Resources.Load("Enemy"), spawnPlace, transform.rotation);
        Enemy enemy = new Enemy();
        enemy.Protect = GetClosestResource(e);
        enemy.Id = eh.enemiesGM.Count;
        eh.enemiesGM.Add((GameObject)gm);
        eh.enemies.Add(enemy);
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

    //as the player explores and finds new encampments, new encampments should be entered into
    //the array, we might need to make the array a list if it gets too big.
    void ReplaceEncampment()
    {

    }
}
