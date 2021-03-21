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
    public AudioHandler audioHandler;

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

    public float unitFireCooldown = 1f;
    public float downTime;
    public float piercingMultiplier;

    Color enemyRed = new Color32(207, 67, 74, 100);
    Color resourceGreen = new Color32(69, 207, 69, 100);
    Color selectedYellow = new Color32(255, 255, 0, 100);
    NavMeshAgent nv;

    void Start()
    {
        selectedUnits = new List<GameObject>();
        robotPos = new Vector3(robot.transform.position.x, 0, robot.transform.position.z);
        SetUpUnits(startingUnits);
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
                selectItems.hudView.UpdateUnit(selectedUnits[0].GetComponent<Unit>());
            }
            else
            {
                MoveAllSelected();
            }
            PlayConfirmClip(selectedUnits[0]);
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
        audioHandler.PlayClip(clickedObj, "confirmPing");
        Debug.Log("CLicked Thing: " + clickedObj+", " +clickedObj.tag);
    }

    void RunAllJobs()
    {
        foreach (GameObject u in units)
        {
            Animator anim = u.GetComponent<Animator>();
            Unit unit_data = u.GetComponent<Unit>();
            Vector3 navmesh_velocity = u.GetComponent<NavMeshAgent>().velocity;

            SetAnimation(u, "walkingSpeed", Mathf.Abs(navmesh_velocity.x + navmesh_velocity.z) / 2);
            robotPos = new Vector3(robot.transform.position.x, 0, robot.transform.position.z);

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
                        PlayReadyClip(u);
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
        GameObject wanderingUnit = (GameObject)Instantiate(Resources.Load(unitPrefab.name), FindSpotToSpawn(0), robot.transform.rotation);

        wanderingUnit.GetComponent<Unit>().Id = units.Count;
        wanderingUnit.GetComponent<Unit>().UnitName = "U" + wanderingUnit.GetComponent<Unit>().Id.ToString();

        units.Add(wanderingUnit);
        ShowGun(wanderingUnit, false);
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
        audioHandler.PlayClip(robot, "pickaxeClang");
    }

    public void SetJobFromRoom(GameObject unit, string roomJob)
    {
        audioHandler.PlayClip(unit, "confirmPing");
        ShowGun(unit, false);
        TravelTo(unit, robotPos, false, false);
    }
    public void LeaveRoomJob(GameObject unit)
    {
        PlayReadyClip(unit);
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
        Unit unit_data = unit.GetComponent<Unit>();
        Vector3 direction = unit_data.JobPos.transform.position - unit.transform.position;
        audioHandler.PlayClip(unit, "smallGun");

            StartCoroutine(TrailOff(0.05f, unit.transform.position, unit.GetComponent<Unit>().JobPos.transform.position));
            if (unit.GetComponent<Unit>().JobPos.tag == "Enemy")
            {
                Enemy enemy_target = unit.GetComponent<Unit>().JobPos.GetComponent<Enemy>();
                if (enemy_target == null || enemy_target.Health <= 0)
                {
                    ResetJob(unit.GetComponent<Unit>());
                }
                else
                {
                    if(enemy_target.Armored && !unit_data.Piercing)
                        enemy_target.Health -= 1;
                    else
                        enemy_target.Health -= CalculateDamage(unit_data.Attack, enemy_target.Defense);
                }
            }
            else if (unit.GetComponent<Unit>().JobPos.tag == "Encampment")
            {
                Encampment encampmentTarget = unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>();
                encampmentTarget.Health -= unit_data.Attack;
                encampmentHandler.CheckForTrigger(unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>());

                if (unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>().Health <= unit_data.Attack) // fishy
                {
                    gameObject.GetComponent<EncampmentHandler>().BeDestroyed();
                    ResetJob(unit.GetComponent<Unit>());
                }
                else
                {
                    gameObject.GetComponent<EncampmentHandler>().BeDestroyed();
                    unit.GetComponent<Unit>().JobPos.GetComponent<Encampment>().Health -= unit_data.Attack; //fishy
                }
            }
        StartCoroutine(FireCoolDown(unit.GetComponent<Unit>()));
    }


    IEnumerator FireCoolDown(Unit unit_data)
    {
        float extratime = Random.Range(-0.3f, 0.3f);
        unit_data.JustShot = true;
        yield return new WaitForSeconds(unitFireCooldown + extratime);
        unit_data.JustShot = false;
    }

    int CalculateDamage(int attack, int defense)
    {
        int dmg = attack - defense;

        if (dmg <= 0)
            return 1;
        else
            return dmg;
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
            audioHandler.PlayClip(unit, "unitDead");
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
        audioHandler.PlayClip(robot, "dropOffClop");
    }
    void AddElectronics()
    {
        ResourceHandling.electronics++;
        audioHandler.PlayClip(robot, "dropOffClop");
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

    public void PlayReadyClip(GameObject unit)
    {
        int num = Random.Range(1, 2);
        audioHandler.PlayClip(unit, "unitReady" + num);
    }

    public void PlayConfirmClip(GameObject unit)
    {
        int num = Random.Range(1, 2);
        audioHandler.PlayClip(unit, "unitConfirm" + num);
    }

}