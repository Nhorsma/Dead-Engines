using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EncampmentHandler : MonoBehaviour
{
	public int startingHealth;			

    public List<Encampment> encampments;

    public EnemyHandler enemyHandler;
    public UnitManager unitManager;
    public SpawnRes spawnRes;
    public ResourceHandling resourceHandling;
    public GameObject automaton;
    public AudioHandler audioHandler;

    public float startSpawnTime,spawnTime, spawnDistance; //
    public bool startSpawning;
    public float volume;
    public float distanceToProtect;

    public GameObject enemy_model1, enemy_model2, enemy_model3;
    Enemy enemy_type_1 = new Enemy(5, 1, 1, 1.5f, false);
    Enemy enemy_type_2 = new Enemy(10, 2, 2, 0.5f, false);
    Enemy enemy_type_3 = new Enemy(50, 3, 3, 3f, true);

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
            encampmentObjects[i].GetComponent<Encampment>().Id = i;
            encampmentObjects[i].GetComponent<Encampment>().Obj = encampmentObjects[i];
            encampmentObjects[i].GetComponent<Encampment>().ClosestResources = new List<GameObject>();
            GetClosestResource(encampmentObjects[i].GetComponent<Encampment>());
            encampmentObjects[i].GetComponent<Encampment>().Deployment = new string[]{enemy_model1.name};
            encampmentObjects[i].GetComponent<Encampment>().Health = startingHealth;

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
                audioHandler.PlayClip(encampments[i].Obj, "explosion");
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


    public void SetEnemyJobs(Encampment encampment)
    {
        if(encampment!=null)
        if(encampment.Health > startingHealth/2) //startinghealth/2 normally
        {
            encampment.EnemyJobs = "guard";
        }
        else
        {
            encampment.EnemyJobs = "destroy";
        }
    }



    /// <summary>
    /// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
    /// </summary>

    public void CheckForTrigger(Encampment encampment)
    { 
        if (encampment.OnField < 2 && (encampment.Health < 95f))
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
        SetEnemyJobs(encampment);
    }

    public void CheckForTrigger(Resource resource)
    {
        Encampment encampment = resource.Level;
        if (encampment.OnField < 2 || resource.Quantity<resourceHandling.startQuantity/2)
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
        SetEnemyJobs(encampment);
    }


    void SpawnEnemy(Encampment encampment)
    {
        audioHandler.PlayClip(encampment.Obj, "enemyAttack2");
        CheckDeployment(encampment);
        GameObject enemyObj = new GameObject();
        Vector3 autoPos = automaton.transform.position;

        for (int i=0;i<encampment.Deployment.Length;i++)
        {
            Vector3 spawnPlace = encampments[encampment.Id].Obj.transform.position+EnemySpawnPlacement(i,20);
            enemyObj = (GameObject)Instantiate(Resources.Load(encampment.Deployment[i]), spawnPlace, transform.rotation);

            Enemy new_enemy = SetEnemyDataOfSpawned(encampment.Deployment[i]);
            enemyObj.GetComponent<Enemy>().Health = new_enemy.Health;
            enemyObj.GetComponent<Enemy>().Attack = new_enemy.Attack;
            enemyObj.GetComponent<Enemy>().Defense = new_enemy.Defense;
            enemyObj.GetComponent<Enemy>().FiringSpeed = new_enemy.FiringSpeed;
            enemyObj.GetComponent<Enemy>().Armored = new_enemy.Armored;

            enemyObj.GetComponent<Enemy>().Resource = GetRandomProtectedResource(encampment);
            enemyObj.GetComponent<Enemy>().CampObj = encampment.Obj;
            enemyObj.GetComponent<Enemy>().CampData = encampment;
            enemyObj.GetComponent<Enemy>().Id = enemyHandler.enemies.Count;
            enemyHandler.enemies.Add(enemyObj);
            enemyHandler.FindSpot(enemyObj);
            encampment.OnField++;
        }
    }

    Vector3 EnemySpawnPlacement(int number, int multi)
    {
        switch (number)
        {
            case 0:
                return new Vector3(multi, 0, multi);
            case 1:
                return new Vector3(multi, 0, -multi);
            case 2:
                return new Vector3(-multi, 0, multi);
            case 3:
                return new Vector3(-multi, 0, -multi);
        }
        return new Vector3(1, 0, 1);
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
        int quantity = 0;

        foreach(GameObject resource in encampment_data.ClosestResources)
        {
            quantity += resource.GetComponent<Resource>().Quantity;
        }
        quantity = quantity / encampment_data.ClosestResources.Count;

        if (quantity <= 0)
        {
            encampment_data.Deployment = new string[50];
            return;
        }

        //default is 4 gunners
        string[] deployment = new string[] { enemy_model1.name, enemy_model1.name, enemy_model1.name, enemy_model1.name };

        if (resourceHandling.resourcesLeft <= 2 || encampment_data.Health < startingHealth/2 || quantity < (resourceHandling.startQuantity / 2))
        {
            deployment[3] = enemy_model2.name;
        }
        if(resourceHandling.resourcesLeft <= 2)
        {
            if (encampment_data.Health < startingHealth / 3 || quantity < (resourceHandling.startQuantity / 2))
                deployment[2] = enemy_model3.name;
            else
                deployment[2] = enemy_model2.name;
        }
        if(resourceHandling.resourcesLeft <= 1)
        {
            if (encampment_data.Health < startingHealth / 3 || quantity < (resourceHandling.startQuantity / 2))
                deployment[1] = enemy_model3.name;
            else
                deployment[1] = enemy_model2.name;
        }
        
        encampment_data.Deployment = deployment;
        
    }

    Enemy SetEnemyDataOfSpawned(string name)
    {
            if (name==enemy_model1.name)
                return enemy_type_1;
            else if (name == enemy_model2.name)
                return enemy_type_2;
            else if (name == enemy_model3.name)
                return enemy_type_3;

        return null;

    }


    public void GetClosestResource(Encampment encampment)
    {
        foreach (GameObject r in spawnRes.GetAllResources())
        {
            if (r!=null && Vector3.Distance(r.transform.position, encampment.Obj.transform.position) < distanceToProtect)
            {
                encampment.ClosestResources.Add(r);
                r.GetComponent<Resource>().Level = encampment;
            }
        }
    }

    public GameObject GetRandomProtectedResource(Encampment encampment)
    {
        int i = Random.Range(0, encampment.ClosestResources.Count);
        return encampment.ClosestResources[i];
    }
}
