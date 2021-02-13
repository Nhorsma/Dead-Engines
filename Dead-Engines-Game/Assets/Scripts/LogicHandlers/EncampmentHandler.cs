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
    public GameObject automaton;

    public float startSpawnTime,spawnTime, spawnDistance; //
    public bool startSpawning;

    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, deathClip, destroyClip;
    public float volume;

    // numbers represent xy = [% health][encampments on map]. meant to escalate

    public GameObject enemy_model1, enemy_model2, enemy_model3;

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
            encampmentObjects[i].GetComponent<Encampment>().Deployment = new string[]{enemy_model1.name};

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


    public void SetEnemyJobs(GameObject encampment)
    {
        Encampment encampment_data = encampment.GetComponent<Encampment>();
        if(encampment_data.Health > 80) //startinghealth/2 normally
        {
            encampment.GetComponent<Encampment>().EnemyJobs = "guard";
        }
        else
        {
            encampment.GetComponent<Encampment>().EnemyJobs = "destroy";
        }
    }



    /// <summary>
    /// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
    /// </summary>

    public void CheckForTrigger(Encampment encampment)
    { 
        if (encampment.OnField < 2 && (encampment.Health < 90 || resourceHandling.resourceQuantities[resourceHandling.GetNumber(encampment.ClosestResource)]<40))
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
            return;
        }
        SetEnemyJobs(encampment.Obj);
    }


    void SpawnEnemy(Encampment encampment)
    {
        PlayClip(encampment.Obj, "attack");
        CheckDeployment(encampment);
        GameObject enemyObj = new GameObject();
        Vector3 autoPos = automaton.transform.position;

        for (int i=0;i<encampment.Deployment.Length;i++)
        {
            Vector3 spawnPlace = encampments[encampment.Id].Obj.transform.position;
            enemyObj = (GameObject)Instantiate(Resources.Load(encampment.Deployment[i]), spawnPlace, transform.rotation);

            enemyObj.GetComponent<Enemy>().Resource = encampment.ClosestResource;
            enemyObj.GetComponent<Enemy>().CampObj = encampment.Obj;
            enemyObj.GetComponent<Enemy>().CampData = encampment;
            enemyObj.GetComponent<Enemy>().Id = enemyHandler.enemies.Count;
            enemyHandler.enemies.Add(enemyObj);
            enemyHandler.FindSpot(enemyObj);
            encampment.OnField++;
        }
    }

    IEnumerator WaitUntilCanSpawn(Encampment encampment_data)
    {
        encampment_data.CanSpawn = false;
        yield return new WaitForSeconds(spawnTime);

        if(encampment_data.OnField < 2)
            encampment_data.CanSpawn = true;
    }

    void CheckDeployment(Encampment encampment_data)
    {
        int quantity = resourceHandling.resourceQuantities[resourceHandling.GetNumber(encampment_data.ClosestResource)];

        if (quantity <= 0)
        {
            encampment_data.Deployment = new string[50];
            return;
        }

        //default is 4 gunners
        string[] deployment = new string[] { enemy_model1.name, enemy_model1.name, enemy_model1.name, enemy_model1.name };

        if (resourceHandling.resourcesLeft <= 2 || encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2))
        {
            deployment[3] = enemy_model2.name;
        }
        if(resourceHandling.resourcesLeft <= 2)
        {
            if (encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2))
                deployment[2] = enemy_model3.name;
            else
                deployment[2] = enemy_model2.name;
        }
        if(resourceHandling.resourcesLeft <= 1)
        {
            if (encampment_data.Health < 50 || quantity < (resourceHandling.startQuantity / 2))
                deployment[1] = enemy_model3.name;
            else
                deployment[1] = enemy_model2.name;
        }
        
        encampment_data.Deployment = deployment;
        
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
