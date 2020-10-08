using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems si;
    public ResourceHandling rh;
    public Vector3 robotPos;
    public GameObject robot;
    public float stoppingDistance;

    public GameObject[] unitsGM;         //the gameobjects
    public Unit[] units;                 //the 'unit' info attached to the gameobjects
    [System.NonSerialized]
    public static List<GameObject> selectedUnits;
    NavMeshAgent nv;

    void Start()
    {
        selectedUnits = new List<GameObject>();
        robotPos = robot.transform.position;
        unitsGM = GameObject.FindGameObjectsWithTag("Friendly");
        units = new Unit[unitsGM.Length];
        SetUpUnits();
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
        for(int i=0;i<unitsGM.Length;i++)
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

            }
            else
            {
                MoveAllSelected();
            }

        }
    }

    void MoveAllSelected()
    {
        if (selectedUnits.Count != 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                //
                //When more Room jobs are implemented, Fix This to have more Jobs
                //
                if (GetUnit(selectedUnits[i]).Job != "shrine" && selectedUnits[i].activeSelf)
                {
                    units[i].Job = "none";
                    units[i].JobPos = null;

                    if (i == 0)
                    {
                        TravelTo(selectedUnits[i], Hit().point, false, false);
                    }
                    else if (i > 0)
                    {
                        Vector3 dif = selectedUnits[i - 1].transform.position - selectedUnits[i].transform.position;

                        //**this will move the selected to the same position relative to eachother
                        //    Vector3 newDes = selectedUnits[i - 1].GetComponent<NavMeshAgent>().destination - dif;

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

    void MoveObject(GameObject selected)
    {
        TravelTo(selected, Hit().point, false, false);
    }

    void RunAllJobs()
    {
        foreach (Unit unit in units)
        {
            if (!unit.Job.Equals("none") && !unit.JobPos.Equals(null))
            {
                if(unit.Job.Equals("Combat"))
                {
                    Combat(unit);
                }
                else if(unit.Job.Equals("ExtractionMetal") || unit.Job.Equals("ExtractionElectronics"))
                {
                    Extraction(unit, GetResourceID(unit.JobPos), unit.Job);///////////////////////////////////////////////////////////////////////
                }
                else if(unit.Job=="shrine")
                {
                    float jx = GetUnitObject(unit).transform.position.x;
                    float jz = GetUnitObject(unit).transform.position.z;
                    if (Mathf.Abs(jx-unit.JobPos.transform.position.x)<1f && Mathf.Abs(jz - unit.JobPos.transform.position.z) < 1f)
                    {
                        GetUnitObject(unit).SetActive(false);
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
        if (thing==null)
            return;

        if (thing.tag == "Metal" ||
            thing.tag == "Electronics")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);
                unit.Job=("Extraction" + thing.tag); //////////////////////////////////////////////////////////////////////////////////////////////
                unit.JobPos=(thing);
                unit.JustDroppedOff=(true);
                TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, false, false);
            }
        }
        else if(thing.tag == "Enemy" || thing.tag == "Encampment")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
            //    gm.GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
                Unit unit = GetUnit(gm);
                unit.Job="Combat";
                unit.JobPos=thing;
            //   TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true);
            }
        }
    }
    

    int GetResourceID(GameObject gm)
    {
        for(int i=0;i<rh.resDeposits.Length;i++)
        {
            if (rh.resDeposits[i]==gm)
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
            if (unitsGM[i]==gm)
                return units[i];
        }
        return null;
    }


    void Extraction(Unit unit, int ri, string resource)
    {
        if (unit.JobPos==null)
            return;

        GameObject gm = GetUnitObject(unit);
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = gm.transform.position;

        //reaches resource
        if (unit.JustDroppedOff && Vector3.Distance(gm.transform.position, unit.JobPos.transform.position)<stoppingDistance)
        {
            Extract(ri);
            unit.JustDroppedOff=false;
            TravelTo(gm, robotPos, false, false);
            //Debug.Log(unit.Job + " at " + unit.JobPos.transform.position);
        }
        else if (!unit.JustDroppedOff && Vector3.Distance(gm.transform.position, robotPos) < stoppingDistance) //reaches robot
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
            
            unit.JustDroppedOff=(true);
            TravelTo(gm, unit.JobPos.transform.position, false, false);
            //Debug.Log(unit.Job + " at "+unit.JobPos.transform.position);
        }
    }

    void Combat(Unit unit)
    {
        GameObject gm = GetUnitObject(unit);
        NavMeshAgent nv = GetUnitObject(unit).GetComponent<NavMeshAgent>();
        TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true, true);

        if (Vector3.Distance(gm.transform.position, unit.JobPos.transform.position) < stoppingDistance + 1f
            && !unit.JustShot)
        {
            Fire(unit);
            StartCoroutine(FireCoolDown(unit));
        }
    }

    IEnumerator FireCoolDown(Unit unit)
    {
        unit.JustShot = true;
        yield return new WaitForSeconds(1f);
        unit.JustShot = false;
    }

    void Fire(Unit unit)
    {
        Vector3 direction = unit.JobPos.transform.position - GetUnitObject(unit).transform.position;

        RaycastHit hit;
        if(Physics.Raycast(GetUnitObject(unit).transform.position, direction, out hit, 100f))
        {
            if (hit.collider.tag == "Enemy")
            {
                //access's the enemy via the enemyHandler, and reduces the enemie's health by one
                gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health--;

                //Debug.Log("enemy: " + gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health);
            }
            else if(hit.collider.tag=="Encampment")
            {
                gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health--;
                //Debug.Log("camp: "+gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health);
            }
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

}
