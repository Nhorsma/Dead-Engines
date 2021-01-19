using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HunterHandler : MonoBehaviour
{
    public GameObject automaton, h1, h2, h3;
    public AutomotonAction autoAction;

    public List<GameObject> deployedHunters;

    public float spawnRadius, stoppingDistance, movementSpeed;
    public bool canSpawn, isDeployed;
    public float chance, spawnTime;
    public AudioClip attack1Clip, attack2Clip, shootClip, hitClip, destroyClip, enemyDetectedClip;

    Vector3 last;

    void Start()
    {
        last = new Vector3();
    }

    void Update()
    {
        HappyHunting();
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
				hunterObj = (GameObject)Instantiate(Resources.Load("hunter 1"));
				hunterObj.GetComponent<Hunter>().Speed = 7f;
				hunterObj.GetComponent<Hunter>().Health = 2;
				hunterObj.GetComponent<Hunter>().Damage = 2;
				break;
			case 2:
				hunterObj = (GameObject)Instantiate(Resources.Load("hunter 2"));
				hunterObj.GetComponent<Hunter>().Speed = 10f;
				hunterObj.GetComponent<Hunter>().Health = 1;
				hunterObj.GetComponent<Hunter>().Damage = 1;
				break;
			case 3:
				hunterObj = (GameObject)Instantiate(Resources.Load("hunter 3"));
				hunterObj.GetComponent<Hunter>().Speed = 3f;
				hunterObj.GetComponent<Hunter>().Health = 3;
				hunterObj.GetComponent<Hunter>().Damage = 3;
				break;
		}

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
			Vector3 spawnPlace = automaton.transform.position + RandomSpawnPoint();
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

                Transform hunterTransform = h.GetComponentInChildren<Transform>();

                if (Vector3.Distance(h.transform.position, automaton.transform.position) > stoppingDistance * 1.5f)
                {
                    TravelTo(h, automaton.transform.position, true);
                    h.GetComponent<Animator>().SetBool("isShooting", false);
                }
                else if (Vector3.Distance(h.transform.position, automaton.transform.position) < stoppingDistance)
                {
                    h.GetComponent<Animator>().SetBool("isShooting", true);
                    if (!h.GetComponent<Hunter>().JustShot)
                    {
                        Fire(h, h.GetComponent<Hunter>());
                        StartCoroutine(FireCoolDown(h.GetComponent<Hunter>()));
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
				PlayClip("enemy");
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

	public void DealHunterDamage(GameObject hunter) //fishy
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
        hunter_data.Health--;
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
            PlayClip("death");
			Destroy(hunter);

            //if (deployedHunters.Equals(new Hunter[] { null, null, null }))
            //{
            //    isDeployed = false;
            //}

			if (deployedHunters.Count <= 0)
			{
				isDeployed = false;
			}
        }
        else
        {
            PlayClip("hit");
        }
    }

    void Fire(GameObject hunter, Hunter hunter_data)
    {
        if (hunter_data.Target != null)
        {
            Vector3 direction = hunter_data.Target.transform.position - hunter.transform.position;
            PlayClip("shoot");

            Vector3 shootFrom = hunter.transform.Find("FireFrom").position;
            StartCoroutine(TrailOff(0.07f, shootFrom, hunter_data.Target.transform.position + new Vector3(0, 50, 0)));

            autoAction.RecieveDamage(hunter_data.Damage);
        }
    }
	IEnumerator FireCoolDown(Hunter hunter_data)
	{
		hunter_data.JustShot = true;
		yield return new WaitForSeconds(2f);
		hunter_data.JustShot = false;
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
        AudioSource tempSource = automaton.GetComponent<AudioSource>();
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
        else if (str.Equals("enemy"))
            tempSource.PlayOneShot(enemyDetectedClip);
    }
}
