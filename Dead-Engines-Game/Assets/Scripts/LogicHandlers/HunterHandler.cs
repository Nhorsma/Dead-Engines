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
    public AudioClip attack1Clip, attack2Clip, shootClip, hitClip, destroyClip;


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

                if (Vector3.Distance(ho.transform.position, automoton.transform.position) > stoppingDistance * 1.5f)
                {
                    TravelTo(ho, automoton.transform.position, true);
                    ho.GetComponent<Animator>().SetBool("isShooting", false);
                }
                else if (Vector3.Distance(ho.transform.position, automoton.transform.position) < stoppingDistance)
                {
                    ho.GetComponent<Animator>().SetBool("isShooting", true);
                    if (!h.JustShot)
                    {
                        Fire(h);
                        StartCoroutine(FireCoolDown(h));
                    }
                }

                /*
                if (Vector3.Distance(ho.transform.position,automoton.transform.position)<stoppingDistance/3
                    && h.CanWalk)
                {
                    // mid = 2xy - xy
                    float x1 = ho.transform.position.x;
                    float y1 = ho.transform.position.z;
                    float x2 = automoton.transform.position.x;
                    float y2 = automoton.transform.position.z;
                    Vector3 position = new Vector3(2*(2 * x1) - x2, ho.transform.position.y, 2*(2 * y1) - y2);
                    TravelTo(ho, position, true);
                    ho.GetComponent<Animator>().SetBool("isShooting", false);
                }
                */
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
                return new Vector3(spawnRadius, 0, Random.Range(-spawnRadius, spawnRadius));
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
        h.Target = automoton;
        return h;

    }

    void TravelTo(GameObject a, Vector3 place, bool stop)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
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
            if (h != null && h.Obj.Equals(ho))
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
        if (h.Health <= 0)
        {
            int i = -1;
            for (int j = 0; j < deployed.Length; j++)
            {
                if (deployed[j] == (h))
                {
                    i = j;
                    break;
                }
            }

            if (i == -1)
                return;

            deployed[i] = null;
            PlayClip("death");
            Destroy(h.Obj);

            if (deployed.Equals(new Hunter[] { null, null, null }))
            {
                isDeployed = false;
                canSpawn = false;
            }
        }
        else
        {
            PlayClip("hit");
        }
    }

    IEnumerator FireCoolDown(Hunter hunt)
    {
        hunt.JustShot = true;
        yield return new WaitForSeconds(2f);
        hunt.JustShot = false;
    }

    void Fire(Hunter hunter)
    {
        if (hunter.Target != null)
        {
            Vector3 direction = hunter.Target.transform.position - hunter.Obj.transform.position;
            PlayClip("shoot");

            Vector3 shootFrom = hunter.Obj.transform.Find("FireFrom").position;
            StartCoroutine(TrailOff(0.07f, shootFrom, hunter.Target.transform.position + new Vector3(0, 50, 0)));

            //            int hitChance = Random.Range(0, 2);
            //          if (hitChance == 0)
            //        {
            //Debug.Log("hit");

            //      }
        }
    }

    GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);

        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);
        GameObject trail = (GameObject)Instantiate(Resources.Load("RedBulletTrail"), start - dif, angle * offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }

    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
    }

    public void PlayClip(string str)
    {
        AudioSource tempSource = automoton.GetComponent<AudioSource>();
        if (str.Equals("attack"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(attack1Clip);
            else
                tempSource.PlayOneShot(attack2Clip);
        }
        else if (str.Equals("shoot"))
            tempSource.PlayOneShot(shootClip);
        else if (str.Equals("hit"))
            tempSource.PlayOneShot(hitClip);
        else if (str.Equals("death"))
            tempSource.PlayOneShot(destroyClip);
    }
}
