using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectItems : MonoBehaviour
{
    public Image selectionSquare;
    RectTransform selectionSquareTrans;
    public float dragLimit;
    public UnitManager unitManager;
    public AutomotonAction autoAction;

    //The materials
    public Material normalMaterial;
    public Material highlightMaterial;
    public Material selectedMaterial;

	public HudView hudView;

    Color normalC = new Color32(250, 250, 250, 200);
    Color highLightedC = new Color32(255, 0, 255, 200);
    Color selectedC = new Color32(255, 255, 0, 200);
    Color enemyRed = new Color32(207, 67, 74, 200);
    Color resourceGreen = new Color32(69, 207, 69, 200);
    Color selectedYellow = new Color32(255, 255, 0, 200);

    GameObject highlightThisUnit;
    public GameObject g1, g2; 

    Vector2 mouseFirst, mouseSecond;
    Vector3 squareStartPos;
    Vector3 squareEndPos;
    Vector3 TL, TR, BL, BR;
    bool hasCreatedSquare;

    Vector3 clickSpot;
    RaycastHit hit;

    public AudioSource audioSource;
    public AudioClip readyClip1, readyClip2;

    private void Start()
    {
        hasCreatedSquare = false;
        unitManager = this.gameObject.GetComponent<UnitManager>(); // why
        selectionSquareTrans = selectionSquare.rectTransform;
        mouseSecond = mouseFirst = (Input.mousePosition);

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
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
        {
            if (!hasCreatedSquare)
            {
				ClearAll(false);
                if (hit.collider.CompareTag("Friendly") && !UnitManager.selectedUnits.Contains(hit.collider.gameObject) && hit.collider.gameObject.activeInHierarchy)
                {
                    UnitManager.selectedUnits.Add(hit.collider.gameObject);
                    SetColor(hit.collider.gameObject, false);
					//hit.collider.gameObject.GetComponentInChildren<SpriteRenderer>().color = selectedC;

					//UpdateUnitUI(hit.collider.gameObject.GetComponent<Unit>()); ////////////////////////////////////////// ------------------------------------->
					ReadyClip();
                }
                if(hit.collider.CompareTag("Robot") && autoAction.enabled==true)
                {
                    autoAction.SetSeleted(true, selectedC);
                }
            }
            else //group select
            {
                ClearAll(false);
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
		for (int i = 0; i < unitManager.units.Count; i++)
		{
			GameObject currentUnit = unitManager.units[i];

			//Is this unit within the square
			if (InRect(currentUnit.transform.position) && currentUnit.activeInHierarchy)
			{
				//currentUnit.GetComponentInChildren<SpriteRenderer>().color = selectedC;
				SetColor(currentUnit, false);

				UnitManager.selectedUnits.Add(currentUnit);
				ReadyClip();
			}
			//Otherwise deselect the unit if it's not in the square
			else
			{
				//currentUnit.GetComponentInChildren<SpriteRenderer>().color = normalC;

				SetColor(currentUnit, true);
			}
            if (InRect(autoAction.gameObject.transform.position) && autoAction.enabled==true)
            { 
                autoAction.SetSeleted(true, selectedC);
            }
        }
		if (UnitManager.selectedUnits.Count == 1)
		{
			//UpdateUnitUI(UnitManager.selectedUnits[0].GetComponent<Unit>()); ------------------------------------->
			hudView.hudPanel.SetActive(true);
		}

		if (UnitManager.selectedUnits.Count > 1)
		{
			hudView.UpdateGroup(UnitManager.selectedUnits[0].GetComponent<Unit>());
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



	void ClearAll(bool includeHudPanel)
	{
		for (int i = 0; i < UnitManager.selectedUnits.Count; i++)
		{
			//UnitManager.selectedUnits[i].GetComponentInChildren<SpriteRenderer>().color = normalC;
			SetColor(UnitManager.selectedUnits[i], true);
		}
		UnitManager.selectedUnits.Clear();
		if (includeHudPanel)
		{
			hudView.hudPanel.SetActive(false);
		}
        autoAction.SetSeleted(false, normalC);
    }


    public List<GameObject> GetSelected()
    {
        return UnitManager.selectedUnits;
    }

    public void RemoveSpecific(GameObject gm)
    {
        UnitManager.selectedUnits.Remove(gm);
        //gm.GetComponentInChildren<MeshRenderer>().material = normalMaterial;
        //gm.GetComponentInChildren<SpriteRenderer>().color = normalC;
        SetColor(gm, true);
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

    void SetColor(GameObject gameObj, bool reset)
    {
        if (gameObj.GetComponentInChildren<SpriteRenderer>()==null)
            return;
        
        if(reset)
        {
            gameObj.GetComponentInChildren<SpriteRenderer>().color = normalC;
            unitManager.ResetColor(gameObj.GetComponent<Unit>());
        }
        else
        {
            gameObj.GetComponentInChildren<SpriteRenderer>().color = selectedC;
            unitManager.SetJobCircleColor(gameObj.GetComponent<Unit>(), selectedYellow);
        }
    }

}
