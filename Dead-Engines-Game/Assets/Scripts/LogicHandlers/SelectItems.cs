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

    Color normalC, highLightedC, selectedC;

    GameObject highlightThisUnit;
    public GameObject g1, g2; 

    Vector2 mouseFirst, mouseSecond;
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    Vector3 TL, TR, BL, BR;
    bool hasCreatedSquare;

    Vector3 clickSpot;
    RaycastHit hit;

	public GameObject unitUIPanel;
	public Text unitName;
	public Text unitJob;
	public Text unitItem;
	public Text unitHealth;
	public Slider healthSlider;

    public AudioSource audioSource;
    public AudioClip readyClip1, readyClip2;

    private void Start()
    {
        hasCreatedSquare = false;
        um = this.gameObject.GetComponent<UnitManager>();
        selectionSquareTrans = selectionSquare.rectTransform;
        mouseSecond = mouseFirst = (Input.mousePosition);

        normalC = new Color(0, 100, 100);
        highLightedC = new Color(255, 0, 255);
        selectedC = new Color(255, 255, 0);

        audioSource = Camera.main.GetComponent<AudioSource>();

    }

    private void Update()
    {
        //Highlight();
        LeftClick();

        if(Input.GetKeyDown(KeyCode.Space))
        {
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
            DrawBox(false,1);
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
                        hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>().color = selectedC;

					    UpdateUnitUI(um.GetUnit(hit.collider.gameObject)); //////////////////////////////////////////
                        ReadyClip();
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
                //highlightThisUnit.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
                highlightThisUnit.GetComponentInChildren<SpriteRenderer>().color = highLightedC;
			}
            else
            {
                highlightThisUnit.GetComponentInChildren<SpriteRenderer>().color = selectedC;
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
                    highlightThisUnit.GetComponentInChildren<SpriteRenderer>().color = highLightedC;
                    //highlightThisUnit.GetComponentInChildren<MeshRenderer>().material = highlightMaterial;
                }
                else
                {
                    highlightThisUnit.GetComponentInChildren<SpriteRenderer>().color = selectedC;
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
                hasCreatedSquare = true;
        }
        else
        {
            hasCreatedSquare = false;
        }
    }


    void DrawBox(bool draw, float mod)
    {
        selectionSquare.enabled = hasCreatedSquare = draw;
        if (draw)
        {
            Vector3 middle = (squareStartPos+squareEndPos) / 2f;
            selectionSquareTrans.position = middle;

            float sizeX = Mathf.Abs(squareStartPos.x - squareEndPos.x);
            float sizeY = Mathf.Abs(squareStartPos.y - squareEndPos.y);

            //Set the size of the square
            selectionSquareTrans.sizeDelta = new Vector2(sizeX*mod, sizeY*mod);

        }
    }

    void DrawBox(bool draw, Vector3 s, Vector3 e)
    {
        selectionSquare.enabled = hasCreatedSquare = draw;
        if (draw)
        {
            Vector3 middle = (Camera.main.WorldToScreenPoint(s) + Camera.main.WorldToScreenPoint(e)) / 2f;

            selectionSquareTrans.position = middle;

            float sizeX = Mathf.Abs(Camera.main.WorldToScreenPoint(s).x - Camera.main.WorldToScreenPoint(e).x);
            float sizeY = Mathf.Abs(Camera.main.WorldToScreenPoint(s).y - Camera.main.WorldToScreenPoint(e).y);

            //Set the size of the square
            selectionSquareTrans.position = middle;
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
                currentUnit.GetComponentInChildren<SpriteRenderer>().color = selectedC;
                //currentUnit.GetComponentInChildren<MeshRenderer>().material = selectedMaterial;
                UnitManager.selectedUnits.Add(currentUnit);
                ReadyClip();
            }
            //Otherwise deselect the unit if it's not in the square
            else
            {
                currentUnit.GetComponentInChildren<SpriteRenderer>().color = normalC;
                //currentUnit.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
            }
        }
		if (UnitManager.selectedUnits.Count == 1)
		{
			UpdateUnitUI(um.GetUnit(UnitManager.selectedUnits[0]));
			unitUIPanel.SetActive(true);
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

                //g1.transform.position = mf;
                //g2.transform.position = ms;

                //DrawBox(true,0.9f);
            }
        }

        mf.y = 100f;
        ms.y = -100f;
        size = new Vector3(Mathf.Abs(mf.x - ms.x), Mathf.Abs(mf.y - ms.y), Mathf.Abs(mf.z - ms.z));
        center = new Vector3((mf.x + ms.x)/2, (mf.y + ms.y)/2, (mf.z + ms.z) / 2);

        Bounds b = new Bounds(center, size);

        return b.Contains(u);
    }



    void ClearAll()
    {
        for(int i=0;i< UnitManager.selectedUnits.Count;i++)
        {
            UnitManager.selectedUnits[i].GetComponentInChildren<SpriteRenderer>().color = normalC;
            //UnitManager.selectedUnits[i].GetComponentInChildren<MeshRenderer>().material = normalMaterial;
        }
        UnitManager.selectedUnits.Clear();
		unitUIPanel.SetActive(false);
	}


    public List<GameObject> GetSelected()
    {
        return UnitManager.selectedUnits;
    }

    public void RemoveSpecific(GameObject gm)
    {
        UnitManager.selectedUnits.Remove(gm);
        //gm.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
        gm.GetComponentInChildren<SpriteRenderer>().color = normalC;
    }

	public void UpdateUnitUI(Unit u)
	{
		unitUIPanel.SetActive(true);
		unitName.text = u.UnitName;
		unitJob.text = u.Job;
		//unitItem.text = u.Holding;
		unitHealth.text = u.Health.ToString();
		healthSlider.maxValue = 3;
		healthSlider.value = u.Health;
	}

    void ReadyClip()
    {
        //audioSource.Stop();
        if (!audioSource.isPlaying)
            if (Random.Range(0, 2) == 0)
            audioSource.PlayOneShot(readyClip1);
        else
            audioSource.PlayOneShot(readyClip2);
    }

}
