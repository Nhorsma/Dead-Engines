using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnRes : MonoBehaviour
{

	public GameObject res1, res2, res3, enemy;			// resource + enemy game objects (prefabs)
    private GameObject r1, r2, r3;                      // the clones/instantiations of the resources (to be returned)
	public GameObject autoObj;
	private Transform startPos;							// where the robot body is located
	public int s_maxDistance, s_minDistance;			// max/min distance from the start position
	public int r_maxDistance, r_minDistance;			// max/min distance from an unspecified resource
	public float e_maxDistance, e_minDistance;			// max/min distance from an enemy camp
	private Transform t_res1, t_res2, t_res3;			// transforms used to calculate the above distances
	public int howMany;                                 // how many enemies to spawn
	public int low_range;
	public int high_range;

	/// <summary>
	/// still spawning at origin instead of robot pos
	/// start() makes inf loop
	/// waiting to set robot pos makes inf loop
	/// too many outside dependencies
	/// </summary>
    void Awake()
    {
		startPos = autoObj.transform;
		Debug.Log(autoObj.transform.position);

		// must call in order else null reference
		SpawnResource(1);
		SpawnResource(2);
		SpawnResource(3);

		SpawnEnemies(howMany);
	}

	void Update()
	{
		// debug reload scene
		if (Input.GetKey(KeyCode.R))
		{
			//SceneManager.LoadScene("v1");
			Debug.Log("respawn");
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
			} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance);
		//	Debug.Log("Spawned res1");
			r1 = Instantiate(res1, new Vector3(x, -.025f, z), Quaternion.identity);
			t_res1 = r1.transform;
		}
		if (resNum == 2)
		{
			do
			{
				x = Random.Range(-(low_range - 1), (high_range));
				z = Random.Range(-(low_range - 1), (high_range));
			} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance 
				|| Vector3.Distance(new Vector3(x, 0, z), t_res1.position) >= r_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res1.position) <= r_minDistance);
		//	Debug.Log("Spawned res2");
			r2 = Instantiate(res2, new Vector3(x, -.025f, z), Quaternion.identity);
			t_res2 = r2.transform;
		}
		if (resNum == 3)
		{
			do
			{
				x = Random.Range(-(low_range - 1), (high_range));
				z = Random.Range(-(low_range - 1), (high_range));
			} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance 
				|| Vector3.Distance(new Vector3(x, 0, z), t_res1.position) >= r_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res1.position) <= r_minDistance 
				|| Vector3.Distance(new Vector3(x, 0, z), t_res2.position) >= r_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res2.position) <= r_minDistance);
		//	Debug.Log("Spawned res3");
			r3 = Instantiate(res3, new Vector3(x, -.025f, z), Quaternion.identity);
			t_res3 = r3.transform;
		}
		else return;
	}

	/// <summary>
	/// 1. randomize x & z coords while too close or too far from spawn + matching resource location
	///	-----[ie first enemy camp spawns near resource 1]
	/// 2. instantiate the resource
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
				} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(x, 0, z), t_res1.position) >= e_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res1.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(x, -.025f, z), Quaternion.identity);
			}
			if (i == 1)
			{
				do
				{
					x = Random.Range(-(low_range - 1), (high_range));
					z = Random.Range(-(low_range - 1), (high_range));
				} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(x, 0, z), t_res2.position) >= e_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res2.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(x, -.025f, z), Quaternion.identity);
			}
			if (i == 2)
			{
				do
				{
					x = Random.Range(-(low_range - 1), (high_range));
					z = Random.Range(-(low_range - 1), (high_range));
				} while (Vector3.Distance(new Vector3(x, 0, z), startPos.position) >= s_maxDistance || Vector3.Distance(new Vector3(x, 0, z), startPos.position) <= s_minDistance
				|| Vector3.Distance(new Vector3(x, 0, z), t_res3.position) >= e_maxDistance || Vector3.Distance(new Vector3(x, 0, z), t_res3.position) <= e_minDistance
				);
			//	Debug.Log("Spawned enemy");
				var e = Instantiate(enemy, new Vector3(x, -.025f, z), Quaternion.identity);
			}
		}
	}


    public GameObject[] GetResources()
    {
        return new GameObject[] { r1, r2, r3 };
    }
}
