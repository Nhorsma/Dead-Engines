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

    public static GameObject[] unitsGM;         //the gameobjects
    public static Unit[] units;                 //the 'unit' info attached to the gameobjects
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
            unitsGM[i].GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
            units[i] = new Unit();
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
                SetJobOfSelected();
            else
                MoveAllSelected();

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
            if (!unit.GetJob().Equals("none"))
            {
                if(!unit.GetJob().Equals("Combat"))
                {
                    Extraction(GetUnitID(unit), GetResourceID(unit.GetJobPos()),unit.GetJob());
                }
                Combat();
            }
        }
    }

    void SetJobOfSelected()
    {
        GameObject thing = Hit().collider.gameObject;
        if (thing.tag == "Metal" ||
            thing.tag == "Electronics")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject unit in selectedUnits)
            {
                int ui = GetUnitID(unit);
                units[ui].SetJob("Extraction");
                units[ui].SetJobPos(thing);
                TravelTo(unitsGM[ui].GetComponent<NavMeshAgent>(), rh.resDeposits[ri].transform.position);
            }
        }
        else if(thing.tag == "Enemy")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject unit in selectedUnits)
            {
                int ui = GetUnitID(unit);
                units[ui].SetJob("Combat");
                units[ui].SetJobPos(thing);
            }
        }
    }

    int GetResourceID(GameObject gm)
    {
        int i = 0;
        while(i<rh.resDeposits.Length && !rh.resDeposits[i].Equals(gm))
        {
            i++;
        }
        return i;
    }

    int GetUnitID(GameObject gm)
    {
        int i = 0;
        while (i < unitsGM.Length && !unitsGM[i].Equals(gm))
        {
            i++;
        }
        return i;
    }

    int GetUnitID(Unit gm)
    {
        int i = 0;
        while (i < units.Length && !units[i].Equals(gm))
        {
            i++;
        }
        return i;
    }


    void Extraction(int ui, int ri, string resource)
    {
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = unitsGM[ui].transform.position;

        //TravelTo(unitsGM[ui].GetComponent<NavMeshAgent>(), rh.resDeposits[ri].transform.position);
        if (unitsGM[ui].GetComponent<NavMeshAgent>().speed<=0.1f 
            && Mathf.Abs(uPos.x - depPos.x) < Mathf.Abs(uPos.x - robotPos.x)
            && Mathf.Abs(uPos.y - depPos.y) < Mathf.Abs(uPos.y - robotPos.y))
        {
            Extract(ri);
            TravelTo(unitsGM[ui].GetComponent<NavMeshAgent>(), robotPos);
        }
        else if(unitsGM[ui].GetComponent<NavMeshAgent>().speed <= 0.1f)
        {
            if (resource == "Metal")
                AddMetal();
            else if (resource == "Electronics")
                AddElectronics();
            TravelTo(unitsGM[ui].GetComponent<NavMeshAgent>(), rh.resDeposits[ri].transform.position);
        }
    }

    void Combat()
    {
        //To be Implemented
    }

    void TravelTo(NavMeshAgent a, Vector3 place)
    {
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
