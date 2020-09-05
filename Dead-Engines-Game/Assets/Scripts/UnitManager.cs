using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems si;
    public ResourceHandling rh;
    public Vector3 robotPos;

    public static GameObject[] unitsGM;         //the gameobjects
    public static Unit[] units;                 //the 'unit' info attached to the gameobjects
    [System.NonSerialized]
    public static List<GameObject> selectedUnits = new List<GameObject>();
    NavMeshAgent nv;

    void Start()
    {
        unitsGM = GameObject.FindGameObjectsWithTag("Friendly");
        units = new Unit[unitsGM.Length];
    }

    void Update()
    {
        RightClick();
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------


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


    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            return hit;
        return hit;
    }


    void SetJobOfSelected()
    {
        GameObject thing = Hit().collider.gameObject;
        if (thing.tag=="Metal")
        {
            //extract resource
            int i = rh.GetNumber(thing);
            
            //this is not finished, I am having a brain fart and will
            //come back to it to finish assigning jobs
        }
        
    }

    int GetID(GameObject gm)
    {
        int i = 0;
        while(i<unitsGM.Length && !unitsGM[i].Equals(gm))
        {
            i++;
        }
        return i;
    }


/*
    public GameObject SelectedDeposit()
    {
        return Hit().collider.gameObject;
    }

    public void Extract(GameObject dep)
    {
        rh.Extract(dep);
    }

    GameObject[] GetSelected()
    {
        return selectedUnits.ToArray();
    }

    void StartResourceCollection(GameObject[] selected, GameObject dep)
    {
        Vector3 depPos = dep.transform.position;
        
        //set back-and-forth movement of selected units
    }
    */

}
