using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems si;
    public ResourceHandling rh;
    public EnemyHandler eh;
    public Vector3 robotPos;
    public GameObject robot;
    public float stoppingDistance;
    public float shootingDistance;
    public float downTime;
	public int unitDamage;

    public GameObject[] unitsGM;         //the gameobjects
    public Unit[] units;                 //the 'unit' info attached to the gameobjects
    [System.NonSerialized]
    public static List<GameObject> selectedUnits;
    NavMeshAgent nv;

	public AutomatonUI auto;

	public float unitFireCooldown = 1f;

	public EffectConnector effConnector;

    void Start()
    {
        eh = GetComponent<EnemyHandler>();
        selectedUnits = new List<GameObject>();
        robotPos = robot.transform.position;
        unitsGM = GameObject.FindGameObjectsWithTag("Friendly");
        units = new Unit[unitsGM.Length];
        SetUpUnits();
		auto.UpdateInfoTab();
	}

    void Update()
    {
        RightClick();
        RunAllJobs();
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------


    void SetUpUnits()
    {
        for (int i = 0; i < unitsGM.Length; i++)
        {
            units[i] = new Unit(i);
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
				si.UpdateUnitUI(GetUnit(selectedUnits[0])); /////////////////////////////////////////////////////////////////
			}
            else
            {
                MoveAllSelected();
            }

        }
    }

    void MoveAllSelected()
    {
        if (selectedUnits.Count == 1)
        {
            units[0].Job = "none";
            units[0].JobPos = null;
            TravelTo(selectedUnits[0], Hit().point, false, false);
        }
        else if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                //
                //When more Room jobs are implemented, Fix This to have more Jobs
                //
                if (GetUnit(selectedUnits[i]).Job != "shrine") // && selectedUnits[i].activeSelf
                {
                    GetUnit(selectedUnits[i]).Job = "none";
                    GetUnit(selectedUnits[i]).JobPos = null;

                    if (i == 0)
                    {
                        TravelTo(selectedUnits[i], Hit().point, false, false);
                    }
                    else if (i > 0)
                    {
                        Vector3 prevDes = selectedUnits[i - 1].GetComponent<NavMeshAgent>().destination;
                        Vector3 newDes = new Vector3();
                        if (i % 3 > 0)
                        {
                            newDes = prevDes + new Vector3(0, 0, 2);
                        }
                        else
                        {
                            newDes = selectedUnits[i - 3].GetComponent<NavMeshAgent>().destination + new Vector3(2, 0, 0);
                        }
                        TravelTo(selectedUnits[i], newDes, false, false);
                    }
                }

            }
        }
    }

    void RunAllJobs()
    {
        foreach (Unit unit in units)
        {
            if (unit.Job != "none")//&& !unit.JobPos.Equals(null)
            {
                if (unit.Job == "Combat")
                {
                    Combat(unit);
                }
                else if (unit.Job.Equals("ExtractionMetal") || unit.Job.Equals("ExtractionElectronics"))
                {
                    Extraction(unit, GetResourceID(unit.JobPos), unit.Job);///////////////////////////////////////////////////////////////////////
                }
                else if (unit.Job == "shrine")
                {
                    float jx = GetUnitObject(unit).transform.position.x;
                    float jz = GetUnitObject(unit).transform.position.z;
                    if (Mathf.Abs(jx - unit.JobPos.transform.position.x) < 1f && Mathf.Abs(jz - unit.JobPos.transform.position.z) < 1f)
                    {
                        GetUnitObject(unit).SetActive(false);
                    }
                }
                else if(unit.Job=="dead")
                {
                    if(unit.CanSpawn)
                    {
                        unit.Health = 10;
                        unit.Job = "none";
                        GetUnitObject(unit).SetActive(true);
                        //TravelTo(GetUnitObject(unit), robotPos + new Vector3(-stoppingDistance*1.5f, 0, -stoppingDistance*1.5f), false, true);
                        GetUnitObject(unit).transform.position = robotPos + new Vector3(-stoppingDistance+Random.Range(-3,3),0,-stoppingDistance + Random.Range(-3, 3));
                        unit.CanSpawn = false;
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
        }
    }

    void SetJobOfSelected(GameObject thing)
    {
        if (thing == null)
            return;

        if (thing.tag == "Metal" ||
            thing.tag == "Electronics")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);
                unit.Job = ("Extraction" + thing.tag); //////////////////////////////////////////////////////////////////////////////////////////////
                unit.JobPos = (thing);
                unit.JustDroppedOff = (true);
                TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, false, false);
            }
        }
        if (thing.tag == "Enemy" || thing.tag == "Encampment")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);
                unit.Job = "Combat";
                unit.JobPos = thing;
                TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true, true);
            }
        }
    }


    int GetResourceID(GameObject gm)
    {
        for (int i = 0; i < rh.resDeposits.Length; i++)
        {
            if (rh.resDeposits[i] == gm)
                return i;
        }
        return -1;
    }

    public GameObject GetUnitObject(Unit unit)
    {
        return unitsGM[unit.Id];
    }

    public Unit GetUnit(GameObject gm)
    {
        for (int i = 0; i < unitsGM.Length; i++)
        {
            if (unitsGM[i] == gm)
                return units[i];
        }
        return null;
    }


    void Extraction(Unit unit, int ri, string resource)
    {
        if (unit.JobPos == null)
            return;

        GameObject gm = GetUnitObject(unit);
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = gm.transform.position;

		//reaches resource
		if (unit.JustDroppedOff && Vector3.Distance(gm.transform.position, unit.JobPos.transform.position) < stoppingDistance)
		{
			Extract(ri);
			unit.JustDroppedOff = false;
			TravelTo(gm, robotPos, false, false);
			//Debug.Log(unit.Job + " at " + unit.JobPos.transform.position);
		}
		else if (!unit.JustDroppedOff && Vector3.Distance(gm.transform.position, robotPos) < stoppingDistance) //reaches robot
		{
			if (effConnector.StockCheck() == true)
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

				unit.JustDroppedOff = (true);
				TravelTo(gm, unit.JobPos.transform.position, false, false);
				//Debug.Log(unit.Job + " at "+unit.JobPos.transform.position);
			}
			else
			{
				Debug.Log("No storage space");
				//make unit drop in unit info
			}
		}
    }

    void Combat(Unit unit)
    {
        GameObject gm = GetUnitObject(unit);
        NavMeshAgent nv = GetUnitObject(unit).GetComponent<NavMeshAgent>();

        if (unit.JobPos != null &&
            Vector3.Distance(unit.JobPos.transform.position, GetUnitObject(unit).transform.position) > stoppingDistance)
            TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true, true);

        if (unit.JobPos != null && 
            Vector3.Distance(gm.transform.position, unit.JobPos.transform.position) < shootingDistance
            && !unit.JustShot)
        {
            Fire(unit);
            StartCoroutine(FireCoolDown(unit));
        }
    }

    IEnumerator FireCoolDown(Unit unit)
    {
        unit.JustShot = true;
		yield return new WaitForSeconds(unitFireCooldown);
        unit.JustShot = false;
    }

    void Fire(Unit unit)
    {
        Vector3 direction = unit.JobPos.transform.position - GetUnitObject(unit).transform.position;

        RaycastHit hit;
        if (Physics.Raycast(GetUnitObject(unit).transform.position, direction, out hit, 100f))
        {
            StartCoroutine(TrailOff(0.05f, GetUnitObject(unit).transform.position, unit.JobPos.transform.position));
            if (hit.collider.tag == "Enemy")
            {
                //access's the enemy via the enemyHandler, and reduces the enemie's health by one
                if (eh.GetEnemy(hit.collider.gameObject).Health == unitDamage)
                {
                    unit.Job = "none";
                    unit.JobPos = null;
                }
				eh.GetEnemy(hit.collider.gameObject).Health -= unitDamage;
                eh.EnemyDead(eh.GetEnemy(hit.collider.gameObject));

                //Debug.Log("enemy: " + gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health);
            }
            else if (hit.collider.tag == "Encampment")
            {
                gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health -= unitDamage;
                //Debug.Log("camp: "+gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health);
            }
        }
    }


    public void UnitDown(Unit unit)
    {
        if(unit.Health<=0)
        {
            Debug.Log(unit+" is dead");
            unit.Job = "dead";
            unit.JobPos = null;
            GetUnitObject(unit).SetActive(false);
            GetUnitObject(unit).transform.position = robotPos;

            if(selectedUnits.Contains(GetUnitObject(unit)))
            {
                si.RemoveSpecific(GetUnitObject(unit));
            }
            StartCoroutine(WaitToRespawn(unit));
        }
    }

    IEnumerator WaitToRespawn(Unit unit)
    {
        if (!unit.CanSpawn)
        {
            yield return new WaitForSeconds(downTime);
            unit.CanSpawn = true;

        }
    }

    void TravelTo(GameObject b, Vector3 place, bool stop, bool randomize)
    {
        if (b.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshAgent a = b.GetComponent<NavMeshAgent>();
            if (stop)
                a.stoppingDistance = stoppingDistance;
            if (randomize)
                place += new Vector3(Random.Range(-stoppingDistance/2, stoppingDistance/2), 0, Random.Range(-stoppingDistance/2, stoppingDistance/2));

                a.SetDestination(place);
        }
        
    }

    void Extract(int id)
    {
        rh.resQuantities[id] -= 1;
    }
    
    void AddMetal()
    {
		ResourceHandling.metal++;
    }

    void AddElectronics()
    {
		ResourceHandling.electronics++;
	}

    public Unit ReturnJoblessUnit()
    {
        foreach(Unit u in units)
        {
            if(u.Job=="none")
            {
                return u;
            }
        }
        return null;
    }

    public void SetJobFromRoom(Unit unit, string roomJob)
    {
        TravelTo(GetUnitObject(unit), unit.JobPos.transform.position, false, false);
    }

    GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);

        Vector3 dif = (start-end)/2;
        Quaternion angle = Quaternion.LookRotation(start - end);
        GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"),start-dif, angle*offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
    }

	public void FindUnit(Unit unit)
	{
		Debug.Log("No one is here");
	}
}
