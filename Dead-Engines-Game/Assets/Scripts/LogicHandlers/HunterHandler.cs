using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterHandler : MonoBehaviour
{
    public GameObject automoton, h1, h2, h3;
    public Hunter[] deployed;
    public float spawnRadius, stoppingDistance, movementSpeed;
    public bool canSpawn, isDeployed;
    public float chance, spawnTime;

    Vector3 last;

    void Start()
    {
        last = new Vector3();
        deployed = new Hunter[3];
    }

    
    void Update()
    {
        CheckSpawnHunter();
        HappyHunting();
    }


    void HappyHunting()
    {
        if (isDeployed)
        foreach (Hunter h in deployed)
        {
                GameObject ho = h.Obj;

                if(Vector3.Distance(ho.transform.position,automoton.transform.position)>stoppingDistance*1.5f)
                    TravelTo(ho, automoton.transform.position, true);

                if(Vector3.Distance(ho.transform.position,automoton.transform.position)<stoppingDistance/2
                    && h.CanWalk)
                {
                    Transform hm = ho.GetComponentInChildren<Transform>();
                    // mid = 2xy - xy
                    float x1 = ho.transform.position.x;
                    float y1 = ho.transform.position.z;
                    float x2 = automoton.transform.position.x;
                    float y2 = automoton.transform.position.z;
                    Vector3 position = new Vector3((2 * x1) - x2, ho.transform.position.y, (2 * y1) - y2);

                    Debug.DrawLine(ho.transform.position, position);
                    TravelTo(ho, position, false);

                    //hm.eulerAngles = automoton.transform.position;
                    //ho.transform.position = Vector3.MoveTowards(ho.transform.position, position, movementSpeed * Time.deltaTime);
                }
                last = ho.transform.position;
            }
    }

    void CheckSpawnHunter()
    {
        if (canSpawn && !isDeployed)
        {
            int hit = Random.Range(1, 10);
            if (hit < chance)
            {
                SpawnHunter();
                chance = 0;
                isDeployed = true;
            }
            else
            {
                chance++;
            }
            //Debug.Log(e.Chance + "0%");
            StartCoroutine(ChangeSpawnChance());
        }
    }

    IEnumerator ChangeSpawnChance()
    {
        canSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        canSpawn = true;
    }

    void SpawnHunter()
    {
        for (int i = 0; i < 3; i++)
        {
            Vector3 spawnPlace = automoton.transform.position + RandomSpawnPoint();
            var gm = Instantiate(Resources.Load(RandomHunter().name), spawnPlace, transform.rotation);

            Hunter h = new Hunter((GameObject)gm);
            deployed[i] = h;            
            h.Id = i;
            h.Obj.GetComponent<NavMeshAgent>().speed = movementSpeed;
        }
    }

    Vector3 RandomSpawnPoint()
    {
        int a = Random.Range(1, 4);

        if (a < 3)
        {
            if (a == 1)
                return new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, spawnRadius);
            else
                return new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, -spawnRadius);
        }
        else
        {
            if (a == 3)
                return new Vector3(spawnRadius,0, Random.Range(-spawnRadius, spawnRadius));
            else
                return new Vector3(-spawnRadius, 0, Random.Range(-spawnRadius, spawnRadius));
        }
    }

    GameObject RandomHunter()
    {
        int a = Random.Range(1, 3);
        GameObject g = h1;
        switch (a)
        {
            case 1:
                g= h1;
                break;
            case 2:
                g= h2;
                break;
            case 3:
                g= h3;
                break;
        }
        return g;
    }

    void TravelTo(GameObject a, Vector3 place, bool stop)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
            //Debug.Log("traveling");
            NavMeshAgent nv = a.GetComponent<NavMeshAgent>();
            if (stop)
            {
                Debug.Log("stop");
                nv.stoppingDistance = stoppingDistance;
            }

            nv.SetDestination(place);
        }
        else
        {
            Debug.Log("not working");
        }
    }

    public void DealHunterDamage(GameObject ho)
    {
        Hunter hunter = deployed[0];
        foreach (Hunter h in deployed)
        {
            if (h.Obj.Equals(ho))
            {
                hunter = h;
                break;
            }
        }

        hunter.Health--;
        Debug.Log("shots recieved");

    }
}
