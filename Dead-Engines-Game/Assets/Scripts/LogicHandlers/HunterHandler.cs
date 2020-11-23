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
                if (h == null)
                    break;

                GameObject ho = h.Obj;
                Transform hm = ho.GetComponentInChildren<Transform>();

                if (Vector3.Distance(ho.transform.position,automoton.transform.position)>stoppingDistance*1.5f)
                    TravelTo(ho, automoton.transform.position, true);

                if(Vector3.Distance(ho.transform.position,automoton.transform.position)<stoppingDistance/2
                    && h.CanWalk)
                {
                    
                    // mid = 2xy - xy
                    float x1 = ho.transform.position.x;
                    float y1 = ho.transform.position.z;
                    float x2 = automoton.transform.position.x;
                    float y2 = automoton.transform.position.z;
                    Vector3 position = new Vector3(2*(2 * x1) - x2, ho.transform.position.y, 2*(2 * y1) - y2);
                    TravelTo(ho, position, true);              
                }
                hm.forward = automoton.transform.position - ho.transform.position;
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
            Hunter h = SetHunter();
            GameObject gm = (GameObject)Instantiate(Resources.Load(h.Obj.name), spawnPlace, transform.rotation);

            h.Obj = gm;
            deployed[i] = h;            
            h.Id = i;
            h.Obj.GetComponent<NavMeshAgent>().speed = h.Speed;
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

    Hunter SetHunter()
    {
        Hunter h = new Hunter(h1);
        int a = Random.Range(1, 4);
        switch (a)
        {
            case 1:
                h = new Hunter(h1);
                h.Speed = 7f;
                h.Health = 3;
                h.Damage = 2;
                break;
            case 2:
                h = new Hunter(h2);
                h.Speed = 10f;
                h.Health = 3;
                h.Damage = 1;
                break;
            case 3:
                h = new Hunter(h3);
                h.Speed = 3f;
                h.Health = 5;
                h.Damage = 3;
                break;
        }
        return h;
        
    }

    void TravelTo(GameObject a, Vector3 place, bool stop)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
            //Debug.Log("traveling");
            NavMeshAgent nv = a.GetComponent<NavMeshAgent>();
            if (stop)
            {
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
            if (h!=null && h.Obj.Equals(ho))
            {
                hunter = h;
                break;
            }
        }

        hunter.Health--;
        Debug.Log("shots recieved");
        HunterDeath(hunter);

    }

    public void HunterDeath(Hunter h)
    {
        if(h.Health<=0)
        {
            int i = -1;
            for(int j = 0;j<deployed.Length;j++)
            {
                if (deployed[j]==(h))
                {
                    i = j;
                    break;
                }
            }

            if (i == -1)
                return;

            deployed[i] = null;
            Destroy(h.Obj);

            if(deployed.Equals(new Hunter[]{null,null,null}))
            {
                isDeployed = false;
                canSpawn = false;
            }
        }
    }
}
