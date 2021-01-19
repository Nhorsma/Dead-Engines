using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems selectItems;							// pull
    public ResourceHandling resourceHandling;				// pull
    public EnemyHandler enemyHandler;						// pull
    public EncampmentHandler encampmentHandler;				// pull

    public Vector3 robotPos;								//
    public GameObject robot;                                //

	public int startingUnits;                               //

	public GameObject unitPrefab;
	public List<GameObject> units = new List<GameObject>(); // unit prefabs, unit script attached. lists are better imo

	[System.NonSerialized]
	public static List<GameObject> selectedUnits;

	public float stoppingDistance;							//
	public float pickUpDistance;							//
    public float shootingDistance;                          //

	public int unitDamage;                                  //
	public float unitFireCooldown = 1f;

	public float downTime;									//

    AudioSource audioSource;
    public AudioClip goingClip1, goingClip2, confirmPing, deadClip, shootClip,
                        dropOffClop, pickAxeClip;

    Color enemyRed = new Color32(207,67, 74, 100);
    Color resourceGreen = new Color32(69, 207, 69, 100);
    Color selectedYellow = new Color32(255, 255, 0, 100);
    NavMeshAgent nv;

    void Start()
    {
        //enemyHandler = GetComponent<EnemyHandler>();
        //encampmentHandler = GetComponent<EncampmentHandler>();
        selectedUnits = new List<GameObject>();
		robotPos = new Vector3(robot.transform.position.x,0, robot.transform.position.z);

		audioSource = Camera.main.GetComponent<AudioSource>();

        SetUpUnits(startingUnits);
        //auto.UpdateInfoTab();
    }

    void Update()
    {
        RightClick();
        RunAllJobs();
    }

	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void SetUpUnits(int startingUnits)
    {
        for (int i = 0; i < startingUnits; i++)
        {
            //Vector3 initialPos = FindSpotToSpawn();
            GameObject u = (GameObject)Instantiate(Resources.Load("unit"), FindSpotToSpawn(), robot.transform.rotation);

			u.GetComponent<Unit>().Id = i;
			u.GetComponent<Unit>().UnitName = "U" + u.GetComponent<Unit>().Id.ToString();

			units.Add(u);
        }
    }

	RaycastHit Hit()
	{
		RaycastHit hit;
		if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
			return hit;
		return hit;
	}

	void RightClick()
	{
		if (Input.GetMouseButtonDown(1) && Hit().point != null && selectedUnits.Count > 0)
		{
			//right click on anything that has units do a job
			if (Hit().collider != null &&
				Hit().collider.gameObject.tag == "Metal" ||
				Hit().collider.gameObject.tag == "Electronics" ||
				Hit().collider.gameObject.tag == "Enemy" ||
				Hit().collider.gameObject.tag == "Encampment")
			{
				SetJobOfSelected(Hit().collider.gameObject);
				selectItems.UpdateUnitUI(selectedUnits[0].GetComponent<Unit>());
			}
			else
			{
				MoveAllSelected();
			}
			ReadyClip();
		}
	}

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	/// <summary>
	/// might add a parameter Unit unit_data for performance 
	/// </summary>
	void SetJobOfSelected(GameObject clickedObj)
	{
		if (clickedObj == null)
			return;

		if (clickedObj.tag == "Metal" ||
			clickedObj.tag == "Electronics")
		{
			int ri = GetResourceID(clickedObj);
			foreach (GameObject u in selectedUnits)
			{
				Unit unit = u.GetComponent<Unit>();

				ResetJob(unit);
				unit.Job = ("Extraction" + clickedObj.tag);
				unit.JobPos = (clickedObj); // fishy
				unit.JustDroppedOff = (true);

				TravelTo(units[unit.GetComponent<Unit>().Id], unit.GetComponent<Unit>().JobPos.transform.position, false, false);
			}
		}
		if (clickedObj.tag == "Enemy" || clickedObj.tag == "Encampment")
		{
			foreach (GameObject unit in selectedUnits)
			{
				ResetJob(unit.GetComponent<Unit>());
				unit.GetComponent<Unit>().Job = "Combat";
				unit.GetComponent<Unit>().JobPos = clickedObj; // fishy

				TravelTo(units[unit.GetComponent<Unit>().Id], unit.GetComponent<Unit>().JobPos.transform.position, true, true);
			}
		}
		PlayClip("ping");
	}

	/// <summary>
	/// might add a parameter Unit unit_data for performance 
	/// </summary>
	void RunAllJobs()
	{
		foreach (GameObject unit in units)
		{
			Animator anim = unit.GetComponent<Animator>();
			if (unit.GetComponent<Unit>().Job != "none")//&& !unit.JobPos.Equals(null)
			{
				if (unit.GetComponent<Unit>().Job == "Combat")
				{
					Combat(unit); //
				}
				else if (unit.GetComponent<Unit>().Job.Equals("ExtractionMetal") || unit.GetComponent<Unit>().Job.Equals("ExtractionElectronics"))
				{
					Extraction(unit, GetResourceID(unit.GetComponent<Unit>().JobPos), unit.GetComponent<Unit>().Job); //
				}
				else if (unit.GetComponent<Unit>().Job == "shrine" || unit.GetComponent<Unit>().Job == "refinery"
					|| unit.GetComponent<Unit>().Job == "storage" || unit.GetComponent<Unit>().Job == "study")
				{
					float jx = unit.transform.position.x; //
					float jz = unit.transform.position.z; //
					if (Mathf.Abs(jx - unit.GetComponent<Unit>().JobPos.transform.position.x) < 1f && Mathf.Abs(jz - unit.GetComponent<Unit>().JobPos.transform.position.z) < 1f)
					{
						unit.SetActive(false);
					}
				}
				else if (unit.GetComponent<Unit>().Job == "dead")
				{
					if (unit.GetComponent<Unit>().CanSpawn)
					{
						unit.GetComponent<Unit>().Health = 3; //
						unit.GetComponent<Unit>().Job = "none";
						unit.SetActive(true);
						//TravelTo(GetUnitObject(unit), robotPos + new Vector3(-stoppingDistance*1.5f, 0, -stoppingDistance*1.5f), false, true);
						unit.transform.position = robotPos + new Vector3(-stoppingDistance + Random.Range(-3, 3), 0, -stoppingDistance + Random.Range(-3, 3));
						unit.GetComponent<Unit>().CanSpawn = false;
					}
				}
				else
				{
					//
					//When units stop having jobs in Robot, set them back to Active
					//
					// GetUnitObject(unit).SetActive(true);
				}
			}
			else
			{
				if (unit.GetComponent<NavMeshAgent>().velocity == new Vector3(0, 0, 0))
				{
					unit.GetComponent<Animator>().SetBool("walking", false);
				}

			}
		}
	}

	/// <summary>
	/// might add a parameter Unit unit_data for performance 
	/// </summary>
	void MoveAllSelected()
    {
        if (selectedUnits.Count == 1)
        {
            ResetJob(selectedUnits[0].GetComponent<Unit>());
            TravelTo(selectedUnits[0], Hit().point, false, false);
        }
        else if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                //
                //When more Room jobs are implemented, Fix This to have more Jobs
                //
                if (selectedUnits[i].GetComponent<Unit>().Job != "shrine" || selectedUnits[i].GetComponent<Unit>().Job != "study"
                    || selectedUnits[i].GetComponent<Unit>().Job != "refinery" || selectedUnits[i].GetComponent<Unit>().Job != "storage") // && selectedUnits[i].activeSelf
                {
                    ResetJob(selectedUnits[i].GetComponent<Unit>());

                    if (i == 0)
                    {
                        ResetJob(selectedUnits[i].GetComponent<Unit>());
                        TravelTo(selectedUnits[i], Hit().point, false, false);

                    }
                    else if (i > 0)
                    {
                        ResetJob(selectedUnits[i].GetComponent<Unit>());
                        Vector3 prevDes = selectedUnits[i - 1].GetComponent<NavMeshAgent>().destination;
                        Vector3 newDes = new Vector3();
                        if (i % 3 > 0)
                        {
                            newDes = prevDes + new Vector3(0, 0, 3);
                        }
                        else
                        {
                            newDes = selectedUnits[i - 3].GetComponent<NavMeshAgent>().destination + new Vector3(3, 0, 0);
                        }
                        TravelTo(selectedUnits[i], newDes, false, false);
                    }
                }
            }
        }
    }

	/// <summary>
	/// might add a parameter Unit unit_data for performance 
	/// </summary>
    void Extraction(GameObject unit, int resourceId, string resource)
    {
        if (unit.GetComponent<Unit>().JobPos == null)
		{
			return;
		}

        Vector3 depositPos = resourceHandling.resourceDeposits[resourceId].transform.position;
        Vector3 unitPos = unit.transform.position;

        if (unit.GetComponent<Unit>().JustDroppedOff && Vector3.Distance(unit.transform.position, unit.GetComponent<Unit>().JobPos.transform.position) < pickUpDistance)
        {
            Extract(resourceId);
			unit.GetComponent<Unit>().JustDroppedOff = false;
            TravelTo(unit, robotPos, false, false);
        }
        else if (!unit.GetComponent<Unit>().JustDroppedOff && Vector3.Distance(unit.transform.position, robotPos) < pickUpDistance) //reaches robot
        {
            if (resource.Equals("ExtractionMetal"))
            {
                AddMetal();
                Debug.Log("Got metal");
            }
            else if (resource.Equals("ExtractionElectronics"))
            {
                AddElectronics();
                Debug.Log("Got electronics");
            }
			unit.GetComponent<Unit>().JustDroppedOff = (true);
            TravelTo(unit, unit.GetComponent<Unit>().JobPos.transform.position, false, false);
        }
    }

	/// <summary>
	/// might add a parameter Unit unit_data for performance 
	/// </summary>
	void Combat(GameObject unit)
    {
        NavMeshAgent nv = unit.GetComponent<NavMeshAgent>();

        if (unit.GetComponent<Unit>().JobPos != null &&
            Vector3.Distance(unit.GetComponent<Unit>().JobPos.transform.position, unit.transform.position) > stoppingDistance)
            TravelTo(units[unit.GetComponent<Unit>().Id], unit.GetComponent<Unit>().JobPos.transform.position, true, true);

        if (unit.GetComponent<Unit>().JobPos != null &&
            Vector3.Distance(unit.transform.position, unit.GetComponent<Unit>().JobPos.transform.position) < shootingDistance
            && !unit.GetComponent<Unit>().JustShot)
        {
            Fire(unit);
            StartCoroutine(FireCoolDown(unit.GetComponent<Unit>()));
        }
    }

	public void TakeInUnit()
	{
		GameObject wanderingUnit = (GameObject)Instantiate(Resources.Load("unit"), FindSpotToSpawn(), robot.transform.rotation);

		wanderingUnit.GetComponent<Unit>().Id = units.Count;
		wanderingUnit.GetComponent<Unit>().UnitName = "U" + wanderingUnit.GetComponent<Unit>().Id.ToString();

		units.Add(wanderingUnit);

		//GameObject wanderer = (GameObject)Instantiate(Resources.Load("unit"), FindSpotToSpawn(), robot.transform.rotation);
		//Unit u = new Unit(unitsGM.Length);
		//GameObject[] tempUnitsGM = new GameObject[unitsGM.Length + 1];
		//Unit[] tempUnits = new Unit[units.Length + 1];
		//unitsGM.CopyTo(tempUnitsGM, 0);
		//units.CopyTo(tempUnits, 0);

		//tempUnitsGM[unitsGM.Length] = wanderer;
		//tempUnits[units.Length] = u;

		//unitsGM = tempUnitsGM;
		//units = tempUnits;

		//unitsGM[unitsGM.Length] = wanderer;
		//units[units.Length] = new Unit(units.Length);
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void TravelTo(GameObject unit, Vector3 place, bool stop, bool randomize)
	{
		if (unit.activeSelf == true && unit.GetComponent<NavMeshAgent>() != null)
		{
			NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
			unit.GetComponent<Animator>().SetBool("walking", true);

			if (stop)
			{
				nav.stoppingDistance = this.stoppingDistance;
			}
			else
			{
				nav.stoppingDistance = 0f;
			}

			if (randomize)
			{
				place += new Vector3(Random.Range(-stoppingDistance / 2, stoppingDistance / 2), 0, Random.Range(-stoppingDistance / 2, stoppingDistance / 2));
			}

			nav.SetDestination(place);
		}

	}
	public Vector3 FindSpotToSpawn()
	{
		return robotPos + new Vector3(-pickUpDistance + Random.Range(-3, 3), 0, -pickUpDistance + Random.Range(-3, 3));
	}

	void Extract(int id)
	{
		//rh.resQuantities[id] -= 1;
		resourceHandling.Extract(id, 1);
		PlayClip("pickaxe");
	}

	public void SetJobFromRoom(GameObject unit, string roomJob)
	{
		PlayClip("ping");
		TravelTo(unit, robotPos, false, false);
	}
	public void LeaveRoomJob(GameObject unit)
	{
		ReadyClip();
		ResetJob(unit.GetComponent<Unit>());

		if (!AutomotonAction.endPhaseOne)
		{
			unit.transform.position = FindSpotToSpawn();
			unit.SetActive(true);
		}
	}
	public GameObject ReturnJoblessUnit()
	{
		foreach (GameObject u in units)
		{
			if (u.GetComponent<Unit>().Job == "none")
			{
				return u;
			}
		}
		return null;
	}
	void ResetJob(Unit unit_data)
	{
		ResetColor(unit_data);
		unit_data.Job = "none";
		unit_data.JobPos = null;
	}

	void Fire(GameObject unit)
	{
		Vector3 direction = unit.GetComponent<Unit>().JobPos.transform.position - unit.transform.position;
		PlayClip("shoot");
		//RaycastHit hit;
		//      if (Physics.Raycast(GetUnitObject(unit).transform.position, direction, out hit, 100f))
		//      {
		int hitChance = Random.Range(0, 2);
		if (hitChance == 0)
		{
			StartCoroutine(TrailOff(0.05f, unit.transform.position, unit.GetComponent<Unit>().JobPos.transform.position));
			if (unit.GetComponent<Unit>().JobPos.tag == "Enemy")
			{
				//access's the enemy via the enemyHandler, and reduces the enemie's health by one
				//Debug.Log("enemy : " + eh.GetEnemy(unit.JobPos));
				if (unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>() == null || unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Health <= 0) //fishy
				{
					ResetJob(unit.GetComponent<Unit>());
				}
				else
				{
					unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Health -= unitDamage;
				}

				//Debug.Log("enemy: " + gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health);
			}
			else if (unit.GetComponent<Unit>().JobPos.tag == "Encampment")
			{
				//gameObject.GetComponent<EncampmentHandler>().GetEncampment(unit.GetComponent<Unit>().JobPos).Health -= unitDamage;
				unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>().Health -= unitDamage; // fishy
				if (unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>().Health <= unitDamage) // fishy
				{
					gameObject.GetComponent<EncampmentHandler>().BeDestroyed();
					ResetJob(unit.GetComponent<Unit>());
				}
				else
				{
					gameObject.GetComponent<EncampmentHandler>().BeDestroyed();
					unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>().Health -= unitDamage; //fishy
				}
				//Debug.Log("camp: "+gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health);
			}
		}
	}
	IEnumerator FireCoolDown(Unit unit_data)
	{
		unit_data.JustShot = true;
		yield return new WaitForSeconds(unitFireCooldown);
		unit_data.JustShot = false;
	}

	public void UnitDown(GameObject unit)
	{
		if (unit.GetComponent<Unit>().Health <= 0)
		{
			PlayClip("dead");
			unit.GetComponent<Animator>().SetBool("knockedOut", true);
			Debug.Log(unit.GetComponent<Unit>().UnitName + " is dead"); //
			unit.GetComponent<Unit>().Job = "dead";
			unit.GetComponent<Unit>().JobPos = null;
			if (selectedUnits.Contains(unit))
			{
				selectItems.RemoveSpecific(unit);
			}
			StartCoroutine(WaitToRespawn(unit));
		}
	}
	IEnumerator WaitToRespawn(GameObject unit)
    {
        if (!unit.GetComponent<Unit>().CanSpawn)
        {
//            yield return new WaitForSeconds(3f);
            unit.SetActive(false);
            unit.transform.position = robotPos;
            yield return new WaitForSeconds(downTime);
            unit.GetComponent<Animator>().SetBool("knockedOut", false);
			unit.GetComponent<Unit>().CanSpawn = true;
            ReadyClip();
        }
    }

	int GetResourceID(GameObject clickedObj)
	{
		for (int i = 0; i < resourceHandling.resourceDeposits.Length; i++)
		{
			if (resourceHandling.resourceDeposits[i] == clickedObj)
				return i;
		}
		return -1;
	}
	void AddMetal()
    {
        ResourceHandling.metal++;
        PlayClip("drop");
    }
    void AddElectronics()
    {
        ResourceHandling.electronics++;
        PlayClip("drop");
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
        GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle * offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
    }

    void ReadyClip()
    {
            if (!audioSource.isPlaying)
                if (Random.Range(0, 2) == 0)
                {
                     audioSource.PlayOneShot(goingClip1);
                }
                else
                {
                    audioSource.PlayOneShot(goingClip2);
                }
    }
    void PlayClip(string str)
    {
        if (str.Equals("ping"))
        {
            audioSource.PlayOneShot(confirmPing);
            if (Random.Range(0, 2) == 0)
            {
                audioSource.PlayOneShot(goingClip1);
            }
            else
            {
                audioSource.PlayOneShot(goingClip2);
            }
        }
        else if (str.Equals("drop"))
        {
            audioSource.PlayOneShot(dropOffClop);
        }
        else if (str.Equals("pickaxe"))
        {
            audioSource.PlayOneShot(pickAxeClip);
        }
        else if (str.Equals("shoot"))
        {
            audioSource.PlayOneShot(shootClip);
        }
        else if(str.Equals("dead"))
        {
            audioSource.PlayOneShot(deadClip);
        }
    }

    public void SetJobCircleColor(Unit unit_data, Color colorChange)
    {
        if (unit_data.JobPos != null && unit_data.JobPos.GetComponentInChildren<SpriteRenderer>() != null)
        {
            unit_data.JobPos.GetComponentInChildren<SpriteRenderer>().color = colorChange;
        }
    }
    public void ResetColor(Unit unit_data)
    {
        if(unit_data.Job == "Combat")
		{
			SetJobCircleColor(unit_data, enemyRed);
		}
        else if (unit_data.JobPos != null && (unit_data.JobPos.tag == "Metal" || unit_data.JobPos.tag == "Electronics"))
		{
			SetJobCircleColor(unit_data, resourceGreen);
		}
    }

    public void PhaseTwoUnits()
    {
        foreach(GameObject unit in units)
        {
            unit.SetActive(false);
        }
    }
}