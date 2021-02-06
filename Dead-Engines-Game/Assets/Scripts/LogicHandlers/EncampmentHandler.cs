using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EncampmentHandler : MonoBehaviour
{
    public static int metal;			//why did i put these here???
    public static int electronics;		//
	public int startingHealth;			//

    public List<Encampment> encampments;

    public EnemyHandler enemyHandler;
    public UnitManager unitManager;
    public SpawnRes spawnRes;
    public ResourceHandling resourceHandling;

    public float startSpawnTime,spawnTime, spawnDistance; //
    public bool startSpawning;

    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, deathClip, destroyClip;
    public float volume;

	// numbers represent xy = [% health][encampments on map]. meant to escalate
    string[] health100_3left = { "gunner", "gunner", "gunner" };
    string[] health75_3left = { "gunner", "APC_2", "gunner" };
    string[] health50_3left = { "gunner", "gunner", "APC_2", "APC_2" };
    string[] health25_3left = { "gunner", "APC_2", "gunner", "APC_2" };

    string[] health100_2left = { "gunner", "gunner", "APC_2" };
    string[] health75_2left = { "gunner", "APC_2", "gunner", "APC_2" };
    string[] health50_2left = { "APC_2", "APC_2", "Mech 2" };
    string[] health25_2left = { "Mech 2", "APC_2", "Mech 2" };

    string[] health100_1left = { "gunner", "gunner", "gunner", "APC_2" };
    string[] health75_1left = { "gunner", "gunner", "APC_2", "APC_2", "Mech 2" };
    string[] health50_1left = { "gunner", "gunner", "gunner", "APC_2", "APC_2", "Mech 2", "Mech 2" };
    string[] health25_1left = { "Mech 2", "gunner", "gunner", "APC_2", "APC_2", "Mech 2", "APC_2" };

    void Start()
    {
        resourceHandling = GetComponent<ResourceHandling>(); //fishy
        enemyHandler = GetComponent<EnemyHandler>(); //fishy
		encampments = new List<Encampment>();

        SetUpCamps(); //fishy
        startSpawning = true;
        startSpawnTime = spawnTime;
    }

	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>
	//this will establish that spawned enemies travel to this resource as well as will attack player units that get too close to it.
	void SetUpCamps()
    {
        GameObject[] encampmentObjects = GameObject.FindGameObjectsWithTag("Encampment");
        for (int i = 0; i < encampmentObjects.Length; i++)
        {
            encampmentObjects[i].GetComponent<Encampment>().Obj = encampmentObjects[i];
            encampmentObjects[i].GetComponent<Encampment>().ClosestResource = GetClosestResource(encampmentObjects[i].GetComponent<Encampment>());
            Debug.Log("*** encampment's closest resource : " + encampmentObjects[i].GetComponent<Encampment>().ClosestResource);
            encampmentObjects[i].GetComponent<Encampment>().Deployment = health100_3left;

            encampments.Add(encampmentObjects[i].GetComponent<Encampment>());
        }
    }

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void BeDestroyed()
	{
		for (int i = 0; i < encampments.Count; i++)
		{
			if (encampments[i].Health <= 0)
			{
				PlayClip(encampments[i].Obj, "death");
				startSpawning = false;
				encampments[i].Obj.SetActive(false);
				//encamps[i] = null;
			}
			else
			{
				if (encampments[i].Health >= startSpawnTime * 0.75f)
				{
					spawnTime--;
				}
				if (encampments[i].Health >= startSpawnTime * 0.5f)
				{
					spawnTime--;
				}
				else if (encampments[i].Health >= startSpawnTime * 0.25f)
				{
					spawnTime--;
				}
			}
		}
	}

    /// <summary>
    /// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
    /// </summary>

    public void CheckForTrigger(Encampment encampment)
    {
        Debug.Log("ecampments resource : " + encampment.ClosestResource);
        if (encampment.CanSpawn && (encampment.Health < 90 || resourceHandling.resourceQuantities[resourceHandling.GetNumber(encampment.ClosestResource)]<40))
        {
            int hit = Random.Range(1, 10);

            if (hit < encampment.Chance)
            {
                SpawnEnemy(encampment);
                encampment.Chance = 0;
            }
            else
            {
                encampment.Chance++;
            }
        }
    }


    void SpawnEnemy(Encampment encampment)
    {
        Debug.Log("Object: "+encampment.Obj);
        PlayClip(encampment.Obj, "attack");
        CheckDeployment(encampment);
        GameObject enemyObj = new GameObject();

        for (int i=0;i<encampment.Deployment.Length;i++)
        {
            Vector3 spawnPlace = encampments[encampment.Id].Obj.transform.position + new Vector3(Random.Range(1, 5), 0, Random.Range(1, 5));
            enemyObj = (GameObject)Instantiate(Resources.Load(encampment.Deployment[i]), spawnPlace, transform.rotation);
        }

        enemyObj.GetComponent<Enemy>().Resource = encampment.ClosestResource;
        enemyObj.GetComponent<Enemy>().Camp = encampment.Obj;
        enemyObj.GetComponent<Enemy>().Id = enemyHandler.enemies.Count;

        StartCoroutine(WaitUntilCanSpawn(encampment));

        enemyHandler.enemies.Add(enemyObj);
        enemyHandler.FindSpot(enemyObj);
        encampment.OnField++;
    }

    IEnumerator WaitUntilCanSpawn(Encampment encampment_data)
    {
        encampment_data.CanSpawn = false;
        yield return new WaitForSeconds(spawnTime);
        encampment_data.CanSpawn = true;
    }

    //adds "gunner", "APC", or "Mech"
    void CheckDeployment(Encampment encampment_data)
    {
        int quantity = resourceHandling.resourceQuantities[resourceHandling.GetNumber(encampment_data.ClosestResource)];

        if (quantity <= 0)
        {
            encampment_data.Deployment = new string[50];
            return;
        }

        if (resourceHandling.resourcesLeft == 3)
        {
            if (encampment_data.Health < 25 || quantity < resourceHandling.startQuantity / 4)
            {
				encampment_data.Deployment = health25_3left;
            }
            if (encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2)) // fishy if if elsesif else
            {
				encampment_data.Deployment = health50_3left;
            }
            else if (encampment_data.Health < 75 || quantity < resourceHandling.startQuantity * 0.25f)
            {
				encampment_data.Deployment = health75_3left;
            }
            else
            {
				encampment_data.Deployment = health100_3left;
            }
        }
        else if (resourceHandling.resourcesLeft == 2)
        {
            if (encampment_data.Health < 25 || quantity < resourceHandling.startQuantity / 4)
            {
				encampment_data.Deployment = health25_2left;
            }
            if (encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2)) // fishy if if elsesif else
			{
                encampment_data.Deployment = health50_2left;
            }
            else if (encampment_data.Health < 75 || quantity < resourceHandling.startQuantity * 0.25f)
            {
				encampment_data.Deployment = health75_2left;
            }
            else
            {
				encampment_data.Deployment = health100_2left;
            }
        }
        else if (resourceHandling.resourcesLeft == 1)
        {
            if (encampment_data.Health < 25 || quantity < resourceHandling.startQuantity / 4)
            {
                encampment_data.Deployment = health25_1left;
            }
            if (encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2))
            {
                encampment_data.Deployment = health50_1left;
            }
            else if (encampment_data.Health < 75 || quantity < resourceHandling.startQuantity * 0.25f)
            {
                encampment_data.Deployment = health75_1left;
            }
            else
            {
                encampment_data.Deployment = health100_1left;
            }
        }
        else
        {
            return;
        }

    }


	GameObject GetClosestResource(Encampment encampment)
    {
        float distance = Mathf.Infinity;
        GameObject chosen = new GameObject();
        foreach (GameObject r in spawnRes.GetResources())
        {
            if (Vector3.Distance(r.transform.position, encampment.Obj.transform.position) < distance)
            {
                distance = Vector3.Distance(r.transform.position, encampment.Obj.transform.position);
                chosen = r;
            }
        }
        return chosen;
    }

    public void PlayClip(GameObject encampment_Object, string str)
    {
        AudioSource tempSource = encampment_Object.GetComponent<AudioSource>();
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
