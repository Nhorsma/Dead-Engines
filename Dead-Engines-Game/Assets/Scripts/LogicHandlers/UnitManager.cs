using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems selectItems;
    public ResourceHandling resourceHandling;
    public EnemyHandler enemyHandler;
    public EncampmentHandler encampmentHandler;
    public SpawningPoolController spawnPool;
    public RoomManager roomManager;

    public Vector3 robotPos;
    public GameObject robot;

    public int startingUnits;

    public GameObject unitPrefab;
    public List<GameObject> units = new List<GameObject>(); // unit prefabs, unit script attached. lists are better imo

    [System.NonSerialized]
    public static List<GameObject> selectedUnits;

    public float stoppingDistance;
    public float pickUpDistance;
    public float shootingDistance;

    public int unitDamage;
    public float unitFireCooldown = 1f;
    public float downTime;
    public float piercingMultiplier;

    AudioSource audioSource;
    public AudioClip goingClip1, goingClip2, confirmPing, deadClip, shootClip,
                        dropOffClop, pickAxeClip;

    Color enemyRed = new Color32(207, 67, 74, 100);
    Color resourceGreen = new Color32(69, 207, 69, 100);
    Color selectedYellow = new Color32(255, 255, 0, 100);
    NavMeshAgent nv;

    void Start()
    {
        selectedUnits = new List<GameObject>();
        robotPos = new Vector3(robot.transform.position.x, 0, robot.transform.position.z);

        audioSource = Camera.main.GetComponent<AudioSource>();
        SetUpUnits(startingUnits);

        Debug.Log(robotPos);
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
            GameObject u = (GameObject)Instantiate(Resources.Load(unitPrefab.name), FindSpotToSpawn(i), robot.transform.rotation);

            u.GetComponent<Unit>().Id = i;
            u.GetComponent<Unit>().UnitName = "U" + u.GetComponent<Unit>().Id.ToString();
            units.Add(u);
            ShowGun(u, false);
        }
    }

    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, ~(1 << 10)))
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
                Unit unit_data = u.GetComponent<Unit>();

                ResetJob(unit_data);
                unit_data.Job = ("Extraction" + clickedObj.tag);
                SetAnimation(u, "inCombat", false);
                unit_data.JobPos = (clickedObj); // fishy
                unit_data.JustDroppedOff = (true);

                TravelTo(units[unit_data.Id], unit_data.JobPos.transform.position, false, false);
            }
        }
        if (clickedObj.tag == "Enemy" || clickedObj.tag == "Encampment")
        {
            foreach (GameObject u in selectedUnits)
            {
                Unit unit_data = u.GetComponent<Unit>();
                ResetJob(unit_data);
                unit_data.Job = "Combat";
                SetAnimation(u, "inCombat", true);

                unit_data.JobPos = clickedObj; // fishy

                TravelTo(units[unit_data.Id], unit_data.JobPos.transform.position, true, true);
            }
        }
        PlayClip("ping");
    }

    void RunAllJobs()
    {
        foreach (GameObject u in units)
        {
            Animator anim = u.GetComponent<Animator>();
            Unit unit_data = u.GetComponent<Unit>();
            //Debug.Log(u + " - " + unit_data.Job);
            Vector3 navmesh_velocity = u.GetComponent<NavMeshAgent>().velocity;

            SetAnimation(u, "walkingSpeed", Mathf.Abs(navmesh_velocity.x + navmesh_velocity.z) / 2);


            if (unit_data.Job != "none")//&& !unit.JobPos.Equals(null)
            {
                if (unit_data.Job == "Combat")
                {
                    Combat(u); //
                }
                else if (unit_data.Job.Equals("ExtractionMetal") || unit_data.Job.Equals("ExtractionElectronics"))
                {
                    Extraction(u, GetResourceID(unit_data.JobPos), unit_data.Job); //
                }
                else if (unit_data.Job == "shrine" || unit_data.Job == "refinery"
                    || unit_data.Job == "storage" || unit_data.Job == "study")
                {
                    if (Vector3.Distance(u.transform.position, robotPos) < 10f)
                    {
                        u.SetActive(false);
                    }
                }
                else if (unit_data.Job == "dead" || unit_data.Job == "infirmary")
                {
                    if (unit_data.CanSpawn)
                    {
                        u.transform.position = new Vector3(robotPos.x+3, 0, robotPos.z+3);
                        SetSpawnedUnitInfo(u, true);

                        TravelTo(u, new Vector3(robotPos.x - 20, 0, robotPos.z - 20),false,true);
                        ReadyClip();
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
                ShowGun(u, false);
            }
        }
    }

    void MoveAllSelected()
    {
        if (selectedUnits.Count == 1)
        {
            ResetJob(selectedUnits[0].GetComponent<Unit>());
            TravelTo(selectedUnits[0], Hit().point, false, false);
            ShowGun(selectedUnits[0], false);
        }
        else if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                SetAnimation(selectedUnits[i], "inCombat", false);
                ShowGun(selectedUnits[i], false);
                Unit unit_data = selectedUnits[i].GetComponent<Unit>();
                if (unit_data.Job != "shrine" || unit_data.Job != "study"
                    || unit_data.Job != "refinery" || unit_data.Job != "storage") // && selectedUnits[i].activeSelf
                {
                    ResetJob(unit_data);

                    if (i == 0)
                    {
                        ResetJob(unit_data);
                        TravelTo(selectedUnits[i], Hit().point, false, false);

                    }
                    else if (i > 0)
                    {
                        ResetJob(unit_data);
                        Vector3 prevDes = selectedUnits[i - 1].GetComponent<NavMeshAgent>().destination;
                        Vector3 newDes = new Vector3();
                        if (i % 3 > 0)
                        {
                            newDes = prevDes + new Vector3(0, 0, 8);
                        }
                        else
                        {
                            newDes = selectedUnits[i - 3].GetComponent<NavMeshAgent>().destination + new Vector3(stoppingDistance + 5, 0, 0);
                        }
                        TravelTo(selectedUnits[i], newDes, false, false);
                    }
                }
            }
        }
    }

    /// <summary>
    /// edit this for storageCheck
    /// </summary>
    void Extraction(GameObject unit, int resourceId, string resource)
    {
        if (unit.GetComponent<Unit>().JobPos == null)
        {
            return;
        }
        ShowGun(unit, false);
        Unit unit_data = unit.GetComponent<Unit>();

        Vector3 depositPos = resourceHandling.resourceDeposits[resourceId].transform.position;
        Vector3 unitPos = unit.transform.position;

        if (unit_data.JustDroppedOff && Vector3.Distance(unit.transform.position, unit_data.JobPos.transform.position) < pickUpDistance)
        {
            Extract(resourceId);
            IsDepositProtected(unit_data.JobPos);
            unit_data.JustDroppedOff = false;
            TravelTo(unit, robotPos, false, false);
        }
        else if (!unit_data.JustDroppedOff && Vector3.Distance(unit.transform.position, robotPos) < pickUpDistance) //reaches robot
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
            unit_data.JustDroppedOff = (true);
            TravelTo(unit, unit_data.JobPos.transform.position, false, false);
        }
    }

    void IsDepositProtected(GameObject deposit)
    {
        foreach (Encampment encampment in encampmentHandler.encampments)
        {
            if (deposit == encampment.ClosestResource)
                encampmentHandler.CheckForTrigger(encampment);
        }
    }

    void Combat(GameObject unit)
    {
        NavMeshAgent nv = unit.GetComponent<NavMeshAgent>();
        Unit unit_data = unit.GetComponent<Unit>();
        ShowGun(unit, true);

        if (unit_data.JobPos != null &&
            Vector3.Distance(unit_data.JobPos.transform.position, unit.transform.position) > stoppingDistance)
            TravelTo(units[unit_data.Id], unit_data.JobPos.transform.position, true, false);

        if (unit_data.JobPos != null &&
            Vector3.Distance(unit.transform.position, unit_data.JobPos.transform.position) < shootingDistance
            && !unit_data.JustShot)
        {
            Fire(unit);
        }
    }

    public void TakeInUnit()
    {
        GameObject wanderingUnit = (GameObject)Instantiate(Resources.Load("unit"), FindSpotToSpawn(0), robot.transform.rotation);

        wanderingUnit.GetComponent<Unit>().Id = units.Count;
        wanderingUnit.GetComponent<Unit>().UnitName = "U" + wanderingUnit.GetComponent<Unit>().Id.ToString();

        units.Add(wanderingUnit);
    }

    /// <summary>
    /// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
    /// </summary>

    public void TravelTo(GameObject unit, Vector3 place, bool stop, bool randomize)
    {
        if (unit.activeSelf == true && unit.GetComponent<NavMeshAgent>() != null && unit.GetComponent<NavMeshAgent>().isActiveAndEnabled)
        {
            NavMeshAgent nav = unit.GetComponent<NavMeshAgent>();
            nav.isStopped = false;

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
                place += new Vector3(Random.Range(-stoppingDistance / 10, stoppingDistance / 10), 0, Random.Range(-stoppingDistance / 10, stoppingDistance / 10));
            }

            nav.SetDestination(place);
        }

    }
    public Vector3 FindSpotToSpawn(int number)
    {
        float range = Random.Range(-5, 0);
        return robotPos + new Vector3(-pickUpDistance + range, 0, -pickUpDistance - (5*number));
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
        ShowGun(unit, false);
        TravelTo(unit, robotPos, false, false);
    }
    public void LeaveRoomJob(GameObject unit)
    {
        ReadyClip();
        ResetJob(unit.GetComponent<Unit>());

        if (!AutomotonAction.endPhaseOne)
        {
            unit.transform.position = FindSpotToSpawn(0);
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

    public List<GameObject> ReturnDeadUnits()
    {
        List<GameObject> dead_units = new List<GameObject>();
        foreach (GameObject u in units)
        {
            if (u.GetComponent<Unit>().Job == "dead")
            {
                dead_units.Add(u);
            }
        }
        return dead_units;
    }

	// my version of the method above, pls use it instead
	public GameObject ReturnKnockedOutUnit()
	{
		foreach (GameObject u in units)
		{
			if (u.GetComponent<Unit>().Job == "dead")
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
        float hitChance = Random.Range(0, 2);
        if (hitChance > 0.5f)
        {
            StartCoroutine(TrailOff(0.05f, unit.transform.position, unit.GetComponent<Unit>().JobPos.transform.position));
            if (unit.GetComponent<Unit>().JobPos.tag == "Enemy")
            {
                //access's the enemy via the enemyHandler, and reduces the enemie's health by one
                if (unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>() == null || unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Health <= 0) //fishy
                {
                    ResetJob(unit.GetComponent<Unit>());
                }
                else
                {
                    if(unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Armored &&
                        unit.GetComponent<Unit>().Piercing)
                        unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Health -= (int)(unitDamage*piercingMultiplier);
                    else
                        unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>().Health -= unitDamage;
                }
            }
            else if (unit.GetComponent<Unit>().JobPos.tag == "Encampment")
            {
                Encampment encampmentTarget = unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>();
                encampmentTarget.Health -= unitDamage;
                encampmentHandler.CheckForTrigger(encampmentTarget);

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
            }
        }
        StartCoroutine(FireCoolDown(hitChance, unit.GetComponent<Unit>()));
    }
    IEnumerator FireCoolDown(float extratime, Unit unit_data)
    {
        unit_data.JustShot = true;
        yield return new WaitForSeconds(unitFireCooldown + extratime / 2);
        unit_data.JustShot = false;
    }

    public void UnitDown(GameObject unit)
    {
        if (unit.GetComponent<Unit>().Health <= 0)
        {
            SetSpawnedUnitInfo(unit, false);
            StartCoroutine(WaitToDespawn(unit));
        }
    }

    //lets the "dying" animation play out, then makes the unit disappear
    IEnumerator WaitToDespawn(GameObject unit)
    {
        yield return new WaitForSeconds(5f);

        if (selectedUnits.Contains(unit))
        {
            selectItems.RemoveSpecific(unit);
        }
        StartCoroutine(WaitToRespawn(unit));

    }

    //waits to allow the unit to respawn, see "RunAllJobs" for the "dead" job
    IEnumerator WaitToRespawn(GameObject unit)
    {
        Unit unit_data = unit.GetComponent<Unit>();
        unit.SetActive(false);
        unit.transform.position = robotPos;
        float temp_downtime = downTime;

        if (unit.GetComponent<Unit>().Job == "infirmary")
            temp_downtime = downTime / 2;

        yield return new WaitForSeconds(temp_downtime);
        unit.GetComponent<Unit>().CanSpawn = true;
    }

    void SetSpawnedUnitInfo(GameObject unit, bool isSpawned)
    {
        if (!isSpawned)
        {
            ShowGun(unit, false);
            SetAnimation(unit, "inCombat", false);
            SetAnimation(unit, "knockedOut", true);
            PlayClip("dead");
            unit.GetComponent<NavMeshAgent>().enabled = false;

            if (roomManager.CheckInfirmary())
                unit.GetComponent<Unit>().Job = "infirmary";
            else
                unit.GetComponent<Unit>().Job = "dead";
            unit.GetComponent<Unit>().JobPos = null;
        }
        else
        {
            unit.SetActive(true);
            unit.GetComponent<Unit>().Health = 10;
            unit.GetComponent<Unit>().CanSpawn = false;
            SetAnimation(unit, "knockedOut", false);
            unit.GetComponent<NavMeshAgent>().enabled = true;
            ResetJob(unit.GetComponent<Unit>());
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

    public GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);

        GameObject trail = spawnPool.poolDictionary["trails"].Dequeue();
        trail.transform.position = start - dif;
        trail.transform.rotation = angle * offset;
        trail.SetActive(true);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary["trails"].Enqueue(t);
        t.SetActive(false);
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
        else if (str.Equals("dead"))
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
        if (unit_data.Job == "Combat")
        {
            SetJobCircleColor(unit_data, enemyRed);
        }
        else if (unit_data.JobPos != null && (unit_data.JobPos.tag == "Metal" || unit_data.JobPos.tag == "Electronics"))
        {
            SetJobCircleColor(unit_data, resourceGreen);
        }
    }

    //edit this for unit movement in phase two
    public void PhaseTwoUnits()
    {
        foreach (GameObject unit in units)
        {
            unit.SetActive(false);
        }
    }

    public void SetAnimation(GameObject unit_object, string animationSetting, bool value)
    {
        if (unit_object.GetComponent<Animator>() != null)
            unit_object.GetComponent<Animator>().SetBool(animationSetting, value);
    }

    public void SetAnimation(GameObject unit_object, string animationSetting, float value)
    {
        if (unit_object.GetComponent<Animator>() != null)
            unit_object.GetComponent<Animator>().SetFloat(animationSetting, value);
    }

    public void ShowGun(GameObject unit_object,bool showGun)
    {
        if(unit_object.transform.Find("Rifle")!=null)
        {
            unit_object.transform.Find("Rifle").gameObject.SetActive(showGun);
        }
    }

}