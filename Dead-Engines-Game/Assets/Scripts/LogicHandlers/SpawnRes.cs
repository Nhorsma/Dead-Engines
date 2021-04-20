using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRes : MonoBehaviour
{

	public GameObject metal1, metal2, electronics, crater, enemy;          // resource + enemy game objects (prefabs)
	public GameObject thin_metal1, thin_metal2, thin_electronics, thin_oil, thin_encampment;  // thinOut attached
    private GameObject m1, m2, e, c;                      // the clones/instantiations of the resources (to be returned)
	public GameObject autoObj;
	public Transform startPos;							// where the robot body is located
	public int s_maxDistance, s_minDistance;			// max/min distance from the start position
	public int r_maxDistance, r_minDistance;			// max/min distance from an unspecified resource
	public float e_maxDistance, e_minDistance;			// max/min distance from an enemy camp
	private Transform t_metal1, t_metal2, t_electronics;			// transforms used to calculate the above distances
	public int howManyEncampments;                      // how many enemies to spawn
	public int low_range;
	public int high_range;

	public int outer_low_range;
	public int outer_high_range;
	public GameObject[] outerResources;
	public int outerSpawnDensity;

	public int inner_low_range;
	public int inner_high_range;
	public GameObject[] innerResources;
	public int innerSpawnDensity;

	public int core_low_range;
	public int core_high_range;
	public GameObject[] coreResources;
	public int coreSpawnDensity;

    public ResourceHandling recHandle;

	//public GameObject placeholderObj;

	public int spawnWithEncampmentProbability;


	void Start()
    {
		startPos = autoObj.transform;
		Debug.Log(autoObj.transform.position);

		// must call in order else null reference
		// these will be left in (in case the player spawns slightly far out from any resources)
		SpawnResource(1);
		SpawnResource(2);
		SpawnResource(3);

		outerResources = new GameObject[outerSpawnDensity];
		innerResources = new GameObject[innerSpawnDensity];
		coreResources = new GameObject[coreSpawnDensity];

		//debugging stuff
		//placeholderObj.GetComponentInChildren<SpriteRenderer>().color = Color.magenta;
		SpawnOuterResources();

		//placeholderObj.GetComponentInChildren<SpriteRenderer>().color = Color.cyan;
		SpawnInnerResources();

		//placeholderObj.GetComponentInChildren<SpriteRenderer>().color = Color.yellow;
		SpawnCoreResources();

		//SpawnEnemies(1); // first version method

		//placeholderObj.GetComponentInChildren<SpriteRenderer>().color = Color.white;
		SpawnEncampments(spawnWithEncampmentProbability, outerResources);
	}

	void Update()
	{
		// debug reload scene
		if (Input.GetKeyDown(KeyCode.P))
		{
			//SceneManager.LoadScene("v1");
		}
    }

	/// <summary>
	/// 1. randomize x & z coords while too close or too far from spawn + other resources
	/// 2. instantiate the resource
	/// </summary>
	void SpawnResource(int resNum)
	{
		int x, z = 0;
		if (resNum == 1)
		{
			do
			{
				x = Random.Range(-(low_range-1), (high_range));
				z = Random.Range(-(low_range - 1), (high_range));
			} while (Vector3.Distance(new Vector3(startPos.position.x+x, 0, startPos.position.z+z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance);
		//	Debug.Log("Spawned res1");
			m1 = Instantiate(metal1, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			t_metal1 = m1.transform;
		}
		if (resNum == 2)
		{
			do
			{
				x = Random.Range(-(low_range - 1), (high_range));
				z = Random.Range(-(low_range - 1), (high_range));
			} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance 
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) >= r_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) <= r_minDistance);
		//	Debug.Log("Spawned res2");
			m2 = Instantiate(metal2, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			t_metal2 = m2.transform;
		}
		if (resNum == 3)
		{
			do
			{
				x = Random.Range(-(low_range - 1), (high_range));
				z = Random.Range(-(low_range - 1), (high_range));
			} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance 
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) >= r_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) <= r_minDistance 
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal2.position) >= r_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal2.position) <= r_minDistance);
		//	Debug.Log("Spawned res3");
			e = Instantiate(electronics, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			t_electronics = e.transform;
		}
		else return;
	}

	/// <summary>
	/// 1. randomize x & z coords while too close or too far from spawn + matching resource location
	///	-----[ie first enemy camp spawns near resource 1]
	/// 2. instantiate the resource
	/// 3. DO NOT USE, OUTDATED!
	/// </summary>
	void SpawnEnemies(int howMany)
	{
		int x, z = 0;
		for (int i = 0; i < howMany; i++)
		{
			if (i == 0)
			{
				do
				{
					x = Random.Range(-(low_range - 1), (high_range));
					z = Random.Range(-(low_range - 1), (high_range));
				} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) >= e_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal1.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			}
			if (i == 1)
			{
				do
				{
					x = Random.Range(-(low_range - 1), (high_range));
					z = Random.Range(-(low_range - 1), (high_range));
				} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal2.position) >= e_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_metal2.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			}
			if (i == 2)
			{
				do
				{
					x = Random.Range(-(low_range - 1), (high_range));
					z = Random.Range(-(low_range - 1), (high_range));
				} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_electronics.position) >= e_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), t_electronics.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(startPos.position.x + x, -6.5f, startPos.position.z + z), Quaternion.identity);
			}
		}
	}

	// new version, testing
	void SpawnEncampments(int probability, GameObject[] resourceLayer)
	{
		int x, z = 0;
		for (int i = 0; i < resourceLayer.Length; i++)
		{
			int r = Random.Range(1, probability + 1);
			if (r == probability)
			{
				do
				{
					x = Random.Range(-(outer_low_range - 1), (outer_high_range));
					z = Random.Range(-(outer_low_range - 1), (outer_high_range));
				} while (Vector3.Distance(new Vector3(x, 0, z), resourceLayer[i].transform.position) > e_maxDistance || Vector3.Distance(new Vector3(x, 0, z), resourceLayer[i].transform.position) < e_minDistance);

				var e = Instantiate(thin_encampment, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
		}
	}

	public void SpawnOuterResources()
	{
		int x, z = 0;
		GameObject outer_res = new GameObject();

		for (int i = 0; i < outerSpawnDensity; i++)
		{
			x = Random.Range(-(outer_low_range - 1), (outer_high_range));
			z = Random.Range(-(outer_low_range - 1), (outer_high_range));

			if (x % 2 == 0 && z % 2 == 0)
			{
				outer_res = Instantiate(thin_metal1, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			else if (x % 2 != 0 && z % 2 != 0)
			{
				outer_res = Instantiate(thin_metal2, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			else if ((x % 2 != 0 && z % 2 == 0) || (x % 2 == 0 && z % 2 != 0))
			{
				outer_res = Instantiate(thin_electronics, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			outerResources[i] = outer_res;
		}
    }

	public void SpawnInnerResources()
	{
		int x, z = 0;
		GameObject inner_res = new GameObject();

		for (int i = 0; i < innerSpawnDensity; i++)
		{
			x = Random.Range(-(inner_low_range - 1), (inner_high_range));
			z = Random.Range(-(inner_low_range - 1), (inner_high_range));

			if (x % 2 == 0 && z % 2 == 0)
			{
				inner_res = Instantiate(thin_oil, new Vector3(x, -4.12f, z), Quaternion.identity);				// right now, oil is 2x as likely to spawn than crater
			}
			else if (x % 2 != 0 && z % 2 != 0)
			{
				inner_res = Instantiate(crater, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			else if ((x % 2 != 0 && z % 2 == 0) || (x % 2 == 0 && z % 2 != 0))
			{
				inner_res = Instantiate(thin_oil, new Vector3(x, -4.12f, z), Quaternion.identity);
			}
			innerResources[i] = inner_res;
		}
	}

	public void SpawnCoreResources()
	{
		int x, z = 0;
		GameObject core_res = new GameObject();

		for (int i = 0; i < coreSpawnDensity; i++)
		{
			do
			{
				x = Random.Range(-(core_low_range - 1), (core_high_range));
				z = Random.Range(-(core_low_range - 1), (core_high_range));
			} while (Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(startPos.position.x + x, 0, startPos.position.z + z), startPos.position) <= s_minDistance);

			if (x % 2 == 0 && z % 2 == 0)
			{
				core_res = Instantiate(thin_metal1, new Vector3(x, -6.5f, z), Quaternion.identity);				// change thin_res with new resource later
			}
			else if (x % 2 != 0 && z % 2 != 0)
			{
				core_res = Instantiate(thin_metal2, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			else if ((x % 2 != 0 && z % 2 == 0) || (x % 2 == 0 && z % 2 != 0))
			{
				core_res = Instantiate(thin_electronics, new Vector3(x, -6.5f, z), Quaternion.identity);
			}
			coreResources[i] = core_res;
		}
	}

	//needs to be reworked ... uh still not sure about this one???
/*	public GameObject[] GetResources()
	{
		return new GameObject[] { metal1, metal2, electronics};
	}
    */

	//needs to be reworked ... I think it's done?
	public List<GameObject> GetAllResources()
	{
		List<GameObject> allResources = new List<GameObject>();

		allResources.Add(m1);
		allResources.Add(m2);
		allResources.Add(e);

		foreach (GameObject o in outerResources)
		{
			allResources.Add(o);
		}
		foreach (GameObject o in innerResources)
		{
			allResources.Add(o);
		}
		foreach (GameObject o in coreResources)
		{
			allResources.Add(o);
		}

		return allResources;
	}

	public void TurnOffThinners()
	{
		foreach (GameObject o in GetAllResources())
		{
			if (o != null)
			{
				o.gameObject.GetComponent<SphereCollider>().enabled = false;
			}
		}
		autoObj.GetComponent<SphereCollider>().enabled = false;
	}

}
