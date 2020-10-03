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
    public static List<GameObject> selectedUnits = new List<GameObject>();
    NavMeshAgent nv;

    void Start()
    {
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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            return hit;
        return hit;
    }

    void RightClick()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count > 0)
        {
            //right click on anything that has units do a job
            if (Hit().collider.gameObject.tag == "Metal" ||
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
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                units[i].Job="none";
                units[i].JobPos=(null);

                if (i == 1)
                    TravelTo(selectedUnits[i], Hit().point, false, false);
                else if (i > 1)
                    TravelTo(selectedUnits[i], Hit().point, true, true);
            }
    }

/*
    void MoveObject(GameObject selected)
    {
        TravelTo(selected, Hit().point, false, false);
    }
*/

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
                Unit unit = GetUnit(gm);
                unit.Job="Combat";
                unit.JobPos=thing;
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
            Debug.Log(unit.Job + " at " + unit.JobPos.transform.position);
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
            TravelTo(gm, unit.JobPos.transform.position, false,false);
            Debug.Log(unit.Job + " at "+unit.JobPos.transform.position);
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

                Debug.Log("enemy: " + gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health);
            }
            else if(hit.collider.tag=="Encampment")
            {
                gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health--;
                Debug.Log("camp: "+gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health);
            }
        }
    }

    void TravelTo(GameObject b, Vector3 place, bool stop, bool randomize)
    {
        NavMeshAgent a = b.GetComponent<NavMeshAgent>();
        if (b.GetComponent<NavMeshAgent>() != null)
        {
            if (stop)
                a.stoppingDistance = stoppingDistance;

            if(randomize)
            {
                place += new Vector3(Random.Range(-stoppingDistance, stoppingDistance), 0, Random.Range(-stoppingDistance, stoppingDistance));
            }

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

}
