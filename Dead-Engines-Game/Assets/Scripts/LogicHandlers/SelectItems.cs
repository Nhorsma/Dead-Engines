using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
    public Image selectionSquare;
    RectTransform selectionSquareTrans;
    public float dragLimit;
    public UnitManager um;

    //The materials
    public Material normalMaterial;
    public Material highlightMaterial;
    public Material selectedMaterial;

    GameObject highlightThisUnit;

    Vector2 mouseFirst, mouseSecond;
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    Vector3 TL, TR, BL, BR;
    bool hasCreatedSquare;

    Vector3 clickSpot;
    RaycastHit hit;

    //HealthUI hui;

    private void Start()
    {
        hasCreatedSquare = false;
        um = this.gameObject.GetComponent<UnitManager>();
        selectionSquareTrans = selectionSquare.rectTransform;
        mouseSecond = mouseFirst = (Input.mousePosition);
        //hui = GetComponent<HealthUI>();
		//hui.toDisplay = new List<GameObject>();
        //hui.hoverHPs = new List<Image>();
    }

    private void Update()
    {
        Highlight();
        LeftClick();


        if(Input.GetKeyDown(KeyCode.Space))
        {
            //hui.DeleteHovers();
            Debug.Log("space");
        }
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


    void Select()
    {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100))
            {
                if (!hasCreatedSquare)
                {
                    ClearAll();
                    if (hit.collider.CompareTag("Friendly") && !UnitManager.selectedUnits.Contains(hit.collider.gameObject))
                    {
                        UnitManager.selectedUnits.Add(hit.collider.gameObject);
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


    void Highlight()
    {
        //Change material on the latest unit we highlighted
        if (highlightThisUnit != null)
        {
            //But make sure the unit we want to change material on is not selected
            bool isSelected = false;
            for (int i = 0; i < UnitManager.selectedUnits.Count; i++)
            {
                if (UnitManager.selectedUnits[i] == highlightThisUnit)
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
                for (int i = 0; i < UnitManager.selectedUnits.Count; i++)
                {
                    if (UnitManager.selectedUnits[i] == currentObj)
                    {
                        isSelected = true;
                        break;
                    }
                }

                if (!isSelected)
                {
                    highlightThisUnit = currentObj;
                    highlightThisUnit.GetComponent<MeshRenderer>().material = highlightMaterial;
                    //hui.AddToList(currentObj);
                    //hui.HoverHealth();
                }
            }
            else
            {
              //  hui.DeleteHovers();
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
        for (int i = 0; i < um.unitsGM.Length; i++)
        {
            GameObject currentUnit = um.unitsGM[i];

            //Is this unit within the square
            if (InRect(currentUnit.transform.position))
            {
                currentUnit.GetComponent<MeshRenderer>().material = selectedMaterial;
                UnitManager.selectedUnits.Add(currentUnit);
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
        for(int i=0;i< UnitManager.selectedUnits.Count;i++)
        {
            UnitManager.selectedUnits[i].GetComponent<MeshRenderer>().material = normalMaterial;
        }
        UnitManager.selectedUnits.Clear();
    }


    public List<GameObject> GetSelected()
    {
        return UnitManager.selectedUnits;
    }

}
