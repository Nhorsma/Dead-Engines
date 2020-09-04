﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
    public Image selectionSquare;
    RectTransform selectionSquareTrans;
    public float dragLimit;

    //The materials
    public Material normalMaterial;
    public Material highlightMaterial;
    public Material selectedMaterial;

    [System.NonSerialized]
    public List<GameObject> selectedUnits = new List<GameObject>();
    public GameObject[] allUnits;

    GameObject highlightThisUnit;

    Vector2 mouseFirst, mouseSecond;
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    Vector3 TL, TR, BL, BR;
    bool hasCreatedSquare;

 //   public Transform sphere1;
 //   public Transform sphere2;


  //  public Image first, second;
    NavMeshAgent nv;
    Vector3 clickSpot;
    GameObject selected;

    private void Start()
    {
        hasCreatedSquare = false;
        allUnits = GameObject.FindGameObjectsWithTag("Friendly");
        selectionSquareTrans = selectionSquare.rectTransform;
        mouseSecond = mouseFirst = (Input.mousePosition);
    }

    private void Update()
    {
        Highlight();
        LeftClick();
        RightClick();

    }

    //Help From http://totologic.blogspot.se/2014/01/accurate-point-in-triangle-test.html
    //----------------------------------------------------------------------------------------------------------------
    //----------------------------------------------------------------------------------------------------------------


    void LeftClick()
    {
        if (Input.GetMouseButton(0))
        {
            DetermineIfBox();
            Select();
        }
        else
        {
            DrawBox(false);
            mouseSecond = mouseFirst = (Input.mousePosition);
        }
    }

    void RightClick()
    {
        if (Input.GetMouseButtonDown(1) && selectedUnits.Count>0)
        {
            MoveAllSelected();   
        }
    }


    void Select()
    {
        RaycastHit hit;
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if (!hasCreatedSquare)
                {
                    ClearAll();
                    if (hit.collider.CompareTag("Friendly") && !selectedUnits.Contains(hit.collider.gameObject))
                    {
                        selectedUnits.Add(hit.collider.gameObject);
                        hit.collider.gameObject.GetComponent<MeshRenderer>().material = selectedMaterial;
                    }
                }
                else //group select
                {
                ClearAll();
                GroupSelect();
                }
            }
    }


    void MoveAllSelected()
    {
        if(selectedUnits.Count!=0)
        for (int i = 0; i < selectedUnits.Count; i++)
        {
            MoveObject(selectedUnits[i]);
        }
    }


    void MoveObject(GameObject selected)
    {
        nv = selected.GetComponent<NavMeshAgent>();
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            nv.destination = hit.point;
    }


    void Highlight()
    {
        //Change material on the latest unit we highlighted
        if (highlightThisUnit != null)
        {
            //But make sure the unit we want to change material on is not selected
            bool isSelected = false;
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                if (selectedUnits[i] == highlightThisUnit)
                {
                    isSelected = true;
                    break;
                }
            }

            if (!isSelected)
            {
                highlightThisUnit.GetComponent<MeshRenderer>().material = normalMaterial;
            }

            highlightThisUnit = null;
        }

        //Fire a ray from the mouse position to get the unit we want to highlight
        RaycastHit hit;

        //Fire ray from camera
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit))
        {
            //Did we hit a friendly unit?
            if (hit.collider.CompareTag("Friendly"))
            {
                //Get the object we hit
                GameObject currentObj = hit.collider.gameObject;

                //Highlight this unit if it's not selected
                bool isSelected = false;
                for (int i = 0; i < selectedUnits.Count; i++)
                {
                    if (selectedUnits[i] == currentObj)
                    {
                        isSelected = true;
                        break;
                    }
                }

                if (!isSelected)
                {
                    highlightThisUnit = currentObj;

                    highlightThisUnit.GetComponent<MeshRenderer>().material = highlightMaterial;
                }
            }
        }
    }


    //returns true when the player is clicking and dragging their mouse;
    void DetermineIfBox()
    {
        mouseSecond = Input.mousePosition;
        if (mouseFirst != mouseSecond)
        {
                squareStartPos = mouseFirst;
                squareEndPos = mouseSecond;
                DrawBox(true);
                hasCreatedSquare = true;
        }
        else
        {
            DrawBox(false);
            hasCreatedSquare = false;
        }
    }


    void DrawBox(bool draw)
    {
        selectionSquare.enabled = hasCreatedSquare = draw;
        if (draw)
        {
            Vector3 middle = (squareStartPos + squareEndPos) / 2f;

            selectionSquareTrans.position = middle;

            float sizeX = Mathf.Abs(squareStartPos.x - squareEndPos.x);
            float sizeY = Mathf.Abs(squareStartPos.y - squareEndPos.y);

            //Set the size of the square
            selectionSquareTrans.sizeDelta = new Vector2(sizeX, sizeY);

        }
    }


    void GroupSelect()
    {
        for (int i = 0; i < allUnits.Length; i++)
        {
            GameObject currentUnit = allUnits[i];

            //Is this unit within the square
            if (InRect(currentUnit.transform.position))
            {
                currentUnit.GetComponent<MeshRenderer>().material = selectedMaterial;
                selectedUnits.Add(currentUnit);
            }
            //Otherwise deselect the unit if it's not in the square
            else
            {
                currentUnit.GetComponent<MeshRenderer>().material = normalMaterial;
            }
        }
    }


    //Is a unit within a polygon determined by 4 corners
    bool InRect(Vector3 u)
    {
        Vector3 mf, ms, size, center;
        RaycastHit hit1, hit2;
        mf = ms = new Vector3();

        if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseFirst), out hit1, 1000f))
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(mouseSecond), out hit2, 1000f))
            {
                mf = hit1.point;
                ms = hit2.point;
            }
        }

        mf.y = 100f;
        ms.y = -100f;
        size = new Vector3(Mathf.Abs(mf.x - ms.x), Mathf.Abs(mf.y - ms.y), Mathf.Abs(mf.z - ms.z));
        center = new Vector3((mf.x + ms.x)/2, (mf.y + ms.y)/2, (mf.z + ms.z) / 2);

    //    sphere1.position = mf;
    //    sphere4.position = ms;

        Bounds b = new Bounds(center, size);

        return b.Contains(u);
    }



    void ClearAll()
    {
        for(int i=0;i<selectedUnits.Count;i++)
        {
            selectedUnits[i].GetComponent<MeshRenderer>().material = normalMaterial;
        }
        selectedUnits.Clear();
    }

}
