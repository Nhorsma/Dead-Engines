﻿using System.Collections;
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
                units[i].Job=("none");
                units[i].JobPos=(null);
                MoveObject(selectedUnits[i]);
            }
    }

    void MoveObject(GameObject selected)
    {
        selected.GetComponent<NavMeshAgent>().stoppingDistance = 0;
        nv = selected.GetComponent<NavMeshAgent>();
        nv.destination = Hit().point;
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
                else if(unit.Job.Equals("Extraction"))
                {
                    Extraction(unit, GetResourceID(unit.JobPos), unit.Job);
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
                gm.GetComponent<NavMeshAgent>().stoppingDistance = 0;
                Unit unit = GetUnit(gm);
                unit.Job=("Extraction");
                unit.JobPos=(thing);
                unit.JustDroppedOff=(true);
                TravelTo(unitsGM[unit.Id].GetComponent<NavMeshAgent>(), unit.JobPos.transform.position);
            }
        }
        else if(thing.tag == "Enemy")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                gm.GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
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

    GameObject GetUnitObject(Unit unit)
    { 
        return unitsGM[unit.Id];
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
        if (unit.JobPos==null)
            return;

        GameObject gm = GetUnitObject(unit);
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = gm.transform.position;

        if (Vector3.Distance(gm.transform.position, unit.JobPos.transform.position)<3f
            && unit.JustDroppedOff)
        {
            Extract(ri);
            unit.JustDroppedOff=false;
            TravelTo(gm.GetComponent<NavMeshAgent>(), robotPos);
        }
        else if(Vector3.Distance(gm.transform.position, robotPos) < 3f && !unit.JustDroppedOff)
        {
            if (resource == "Metal")
                AddMetal();
            else if (resource == "Electronics")
                AddElectronics();

            unit.JustDroppedOff=(true);
            TravelTo(gm.GetComponent<NavMeshAgent>(), unit.JobPos.transform.position);
        }
    }

    void Combat(Unit unit)
    {
        GameObject gm = GetUnitObject(unit);
        NavMeshAgent nv = GetUnitObject(unit).GetComponent<NavMeshAgent>();
        TravelTo(nv, unit.JobPos.transform.position);

        Vector3 direction = unit.JobPos.transform.position - GetUnitObject(unit).transform.position;
        Debug.DrawRay(GetUnitObject(unit).transform.position, direction, Color.black);

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
        Vector3 direction = GetUnitObject(unit).transform.position - unit.JobPos.transform.position;
        RaycastHit hit;
        if(Physics.Raycast(GetUnitObject(unit).transform.position, direction, out hit, 100f))
        {
            Debug.DrawRay(GetUnitObject(unit).transform.position, direction, Color.black);
            if (hit.collider.tag == "Enemy")
                Debug.Log("Gotem!");
        }
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
