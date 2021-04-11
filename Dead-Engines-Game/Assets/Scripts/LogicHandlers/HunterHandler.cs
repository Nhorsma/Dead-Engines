using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterHandler : MonoBehaviour
{
    public GameObject automaton, h1, h2, h3, anteater;
    public AutomotonAction autoAction;
    public AudioHandler audioHandler;
    public SpawningPoolController spawnPool;

    public List<GameObject> deployedHunters;

    public float spawnRadius, stoppingDistance, movementSpeed, closeRange, tooCloseRange;
    public bool canSpawn, isDeployed;
    public float chance, spawnTime;

    Vector3 last;

    void Start()
    {
        last = new Vector3();
    }

    void Update()
    {
        HappyHunting();

        if(Input.GetKeyDown(KeyCode.P))
        {
            SpawnAnteater();
            isDeployed = true;
            //SpawnHunter();
        }
    }

	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	GameObject SetHunter() //fishy
	{
		int r = Random.Range(1, 4);
		GameObject hunterObj = h1.gameObject;

		switch (r)
		{
			case 1:
				hunterObj = (GameObject)Instantiate(h1);
				hunterObj.GetComponent<Hunter>().Speed = 30f;
				hunterObj.GetComponent<Hunter>().Health = 20;
				hunterObj.GetComponent<Hunter>().Attack = 2;
                hunterObj.GetComponent<Hunter>().FiringSpeed = 2f;
                hunterObj.GetComponent<Hunter>().CanRetreat = true;
                break;
			case 2:
				hunterObj = (GameObject)Instantiate(h2);
				hunterObj.GetComponent<Hunter>().Speed = 15f;
				hunterObj.GetComponent<Hunter>().Health = 15;
				hunterObj.GetComponent<Hunter>().Attack = 1;
                hunterObj.GetComponent<Hunter>().FiringSpeed = 3.5f;
                hunterObj.GetComponent<Hunter>().CanRetreat = true;
				break;
			case 3:
				hunterObj = (GameObject)Instantiate(h2);
				hunterObj.GetComponent<Hunter>().Speed = 10f;
				hunterObj.GetComponent<Hunter>().Health = 50;
				hunterObj.GetComponent<Hunter>().Attack = 3;
                hunterObj.GetComponent<Hunter>().FiringSpeed = 5f;
                break;
        }

        /*
         * Anteater
            hunterObj = (GameObject)Instantiate(Resources.Load("hunter 3"));
            hunterObj.GetComponent<Hunter>().Speed = 4f;
            hunterObj.GetComponent<Hunter>().Health = 10;
            hunterObj.GetComponent<Hunter>().Attack = 10;
            hunterObj.GetComponent<Hunter>().FiringSpeed = 6f;
        */

        hunterObj.GetComponent<Hunter>().Target = automaton;
		return hunterObj;
	}

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void SpawnHunter()
	{
		for (int i = 0; i < 3; i++)
		{
            Vector3 place = RandomSpawnPoint();

            Vector3 spawnPlace = automaton.transform.position + place;
			//Hunter h = SetHunter().GetComponent<Hunter>();
			GameObject hunterObj = SetHunter(); // fishy
			hunterObj.transform.position = spawnPlace;
			hunterObj.transform.rotation = transform.rotation;

			deployedHunters.Add(hunterObj);
			hunterObj.GetComponent<Hunter>().Id = i;
			hunterObj.GetComponent<NavMeshAgent>().speed = hunterObj.GetComponent<Hunter>().Speed;
		}
	}

	void HappyHunting()
    {
        if (isDeployed)
            foreach (GameObject h in deployedHunters)
            {
                if (h == null)
                    break;

                if (!CheckIfAtDestination(h))
                    h.GetComponent<Hunter>().NextMove = false;
                else
                    h.GetComponent<Hunter>().NextMove = true;

                Transform hunterTransform = h.GetComponentInChildren<Transform>();
                float distance = Vector3.Distance(h.transform.position, automaton.transform.position);

                if (h.GetComponent<Hunter>().NextMove)
                {
                    if (distance > closeRange)
                    {
                        GetClose(automaton.transform.position, h);
                        h.GetComponent<Animator>().SetBool("isShooting", false);
                        h.GetComponent<Animator>().SetBool("isWalking", true);

                    }
                    else
                    {
                        h.GetComponent<Animator>().SetBool("isShooting", true);
                        h.GetComponent<Animator>().SetBool("isWalking", false);
                        if (!h.GetComponent<Hunter>().JustShot)
                        {
                            Fire(h, h.GetComponent<Hunter>());
                            StartCoroutine(FireCoolDown(h.GetComponent<Hunter>()));
                        }
                    }
                }
                if (h.GetComponent<Hunter>().CanRetreat && distance < tooCloseRange)//if very close, walk backwards
                {
                    BackUp(automaton.transform.position,h);
                }

                hunterTransform.forward = automaton.transform.position - h.transform.position;
            }
    }

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void TravelTo(GameObject hunter, Vector3 place, bool stop)
	{
		if (hunter != null && hunter.GetComponent<NavMeshAgent>() != null)
		{
			NavMeshAgent nav = hunter.GetComponent<NavMeshAgent>();
			if (stop)
			{
				nav.stoppingDistance = stoppingDistance;
			}
			nav.SetDestination(place);
		}
		else
		{
			Debug.Log("not working");
		}
	}

	public void CheckSpawnHunter()
	{
		if (canSpawn && !isDeployed)
		{
			int hit = Random.Range(0, 2);
			if (hit < chance)
			{
                audioHandler.PlayClip(Camera.main.gameObject, "enemyDetected");
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
	Vector3 RandomSpawnPoint()
	{
		int r = Random.Range(1, 4);
        
		if (r < 3)
		{
			if (r == 1)
			{
				return new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, spawnRadius);
			}
			else
			{
				return new Vector3(Random.Range(-spawnRadius, spawnRadius), 0, -spawnRadius);
			}
		}
		else
		{
			if (r == 3)
			{
				return new Vector3(spawnRadius, 0, Random.Range(-spawnRadius, spawnRadius));
			}
			else
			{
				return new Vector3(-spawnRadius, 0, Random.Range(-spawnRadius, spawnRadius));
			}
		}
        
    }
	IEnumerator ChangeSpawnChance()
	{
		canSpawn = false;
		yield return new WaitForSeconds(spawnTime);
		canSpawn = true;
	}

	public void DealHunterDamage(GameObject hunter, int amount) //fishy
    {
        Hunter hunter_data = deployedHunters[0].GetComponent<Hunter>();
        foreach (GameObject h in deployedHunters)
        {
            if (h.GetComponent<Hunter>() != null && h.Equals(hunter_data))
            {
                hunter_data = h.GetComponent<Hunter>();
                break;
            }
        }
        hunter_data.Health-=amount;
        Debug.Log("shots recieved");
        HunterDeath(hunter, hunter_data);

    }
    public void HunterDeath(GameObject hunter, Hunter hunter_data)
    {
        if (hunter_data.Health <= 0)
        {
            int i = -1;
            for (int j = 0; j < deployedHunters.Count; j++)
            {
                if (deployedHunters[j] == (hunter_data))
                {
                    i = j;
                    break;
                }
            }

            if (i == -1)
			{
				return;
			}

			deployedHunters.Remove(hunter);
            audioHandler.PlayClip(hunter, "explosion");
            Destroy(hunter);

			if (deployedHunters.Count <= 0)
			{
				isDeployed = false;
			}
        }
        else
        {
            audioHandler.PlayClip(hunter, "metalHit");
        }
    }

    void Fire(GameObject hunter, Hunter hunter_data)
    {
        if (hunter_data.Target != null)
        {
            Vector3 direction = hunter_data.Target.transform.position - hunter.transform.position;
            audioHandler.PlayClip(hunter, "bigLaz");

            Vector3 shootFrom = hunter.transform.Find("FireFrom").position;
            StartCoroutine(TrailOff(0.07f, shootFrom, hunter_data.Target.transform.position + new Vector3(0, 50, 0)));

            autoAction.RecieveDamage(hunter_data.Attack);
        }
    }
	IEnumerator FireCoolDown(Hunter hunter_data)
	{
		hunter_data.JustShot = true;
		yield return new WaitForSeconds(hunter_data.FiringSpeed);
		hunter_data.JustShot = false;
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

        GameObject trail = spawnPool.poolDictionary["hunterLaz"].Dequeue();
        trail.transform.position = start - dif;
        trail.transform.rotation = angle * offset;
        trail.SetActive(true);

        trail.transform.localScale = new Vector3(0.5f, 0.5f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary["hunterLaz"].Enqueue(t);
        t.SetActive(false);
    }

    bool CheckIfAtDestination(GameObject hunter)
    {
        return hunter.GetComponent<NavMeshAgent>().remainingDistance < 50f;
    }

    void GetClose(Vector3 robot, GameObject hunter)
    {
        Vector3 diff = (hunter.transform.position - robot) * 1 / 2;
        robot += diff;
        TravelTo(hunter,robot,false);
        Debug.Log("Getting Closer");
    }

    void BackUp(Vector3 robot, GameObject hunter)
    {
        /*
        Vector3 difference = transform.position - backFrom;
        Vector3 a = difference*2;
        Vector3 b = a + backFrom;
        //move to b
        */

        Vector3 backUp = ((hunter.transform.position - robot) * 3) + robot;
        TravelTo(hunter, backUp, false);
        Debug.Log("Backign Up to "+backUp);
    }

    void FindFlank(Vector3 robot, GameObject hunter)
    {
        /*
        Vector3 difference = transform.position - backFrom;
        Vector3 a = (difference/z,y,difference.x)
        Vector3 b = a + backFrom;
        //move to b
        */

        Vector3 diff = hunter.transform.position - robot;
        Vector3 flank = new Vector3(diff.z, hunter.transform.position.y, diff.x);
        robot += flank;
        TravelTo(hunter, robot, false);
        Debug.Log("Flanking");
    }

    void SpawnAnteater()
    {
        Vector3 spawnPlace = automaton.transform.position + RandomSpawnPoint();
        GameObject hunterObj = Instantiate(anteater);
        hunterObj.GetComponent<Hunter>().Speed = 4f;
        hunterObj.GetComponent<Hunter>().Health = 10;
        hunterObj.GetComponent<Hunter>().Attack = 10;
        hunterObj.GetComponent<Hunter>().FiringSpeed = 6f;

        hunterObj.transform.position = spawnPlace;
        hunterObj.transform.rotation = transform.rotation;

        deployedHunters.Add(hunterObj);
        hunterObj.GetComponent<Hunter>().Id = 100;
        hunterObj.GetComponent<NavMeshAgent>().speed = hunterObj.GetComponent<Hunter>().Speed;

        hunterObj.GetComponent<SnoutBehavior>().pointTo = automaton;
    }
}
