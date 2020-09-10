using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems si;
    public ResourceHandling rh;
    public Vector3 robotPos;
    public float stoppingDistance;

    public GameObject[] unitsGM;         //the gameobjects
    public Unit[] units;                 //the 'unit' info attached to the gameobjects
    [System.NonSerialized]
    public static List<GameObject> selectedUnits = new List<GameObject>();
    NavMeshAgent nv;

    void Start()
    {
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
            /* Ok, so, there needs to be methods that choose the job of selected units,
             * then there must be methods that determine which deposit those units are working on,
             * then units must move back and forth from those deposits, when they reach it they must
             * deplete the deposit's quantity by one per trip, deposits are in the ResrouceHandler class
             * something like "find ID of deposit clicked, unit starts couroutine until depleted, etc)
             */


            //right click on anything that has units do a job
            if (Hit().collider.gameObject.tag == "Metal" ||
                Hit().collider.gameObject.tag == "Electronics" ||
                Hit().collider.gameObject.tag == "Enemy")
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
                MoveObject(selectedUnits[i]);
            }
    }

    void MoveObject(GameObject selected)
    {
        nv = selected.GetComponent<NavMeshAgent>();
        nv.destination = Hit().point;
    }

    void RunAllJobs()
    {
        foreach (Unit unit in units)
        {
            if (!unit.GetJob().Equals("none") && !unit.GetJobPos().Equals(null))
            {
                if(unit.GetJob().Equals("Combat"))
                {
                    Combat();
                }
                else if(unit.GetJob().Equals("Extraction"))
                {
                    Extraction(unit, GetResourceID(unit.GetJobPos()), unit.GetJob());
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
                unit.SetJob("Extraction");
                unit.SetJobPos(thing);
                unit.SetDroppedOff(true);
                TravelTo(unitsGM[unit.ID()].GetComponent<NavMeshAgent>(), unit.GetJobPos().transform.position);
            }
        }
        else if(thing.tag == "Enemy")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);
                unit.SetJob("Combat");
                unit.SetJobPos(thing);
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

    GameObject GetUnitObject(Unit unit)
    { 
        return unitsGM[unit.ID()];
    }

    Unit GetUnit(GameObject gm)
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
        if (unit.GetJobPos()==null)
            return;

        GameObject gm = GetUnitObject(unit);
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = gm.transform.position;

        if (Vector3.Distance(gm.transform.position, unit.GetJobPos().transform.position)<3f
            && unit.GetDroppedOff())
        {
            Extract(ri);
            unit.SetDroppedOff(false);
            TravelTo(gm.GetComponent<NavMeshAgent>(), robotPos);
        }
        else if(Vector3.Distance(gm.transform.position, robotPos) < 3f && !unit.GetDroppedOff())
        {
            if (resource == "Metal")
                AddMetal();
            else if (resource == "Electronics")
                AddElectronics();

            unit.SetDroppedOff(true);
            TravelTo(gm.GetComponent<NavMeshAgent>(), unit.GetJobPos().transform.position);
        }
    }

    void Combat()
    {
        //To be Implemented
    }

    void TravelTo(NavMeshAgent a, Vector3 place)
    {
        if(a!=null)
            a.SetDestination(place);
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
