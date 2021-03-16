using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickManager : MonoBehaviour
{

	Vector3 clickSpot;
	RaycastHit hit;

	public HudView hudView;

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

		if (Input.GetMouseButtonDown(0))
		{
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
			{
				if (hit.collider.gameObject.tag == "Friendly" && UnitManager.selectedUnits.Count == 1)
				{
					hudView.UpdateUnit(hit.collider.gameObject.GetComponent<Unit>());
				}
				else if (hit.collider.gameObject.tag == "Encampment")
				{
					hudView.UpdateEncampment(hit.collider.gameObject.GetComponent<Encampment>());
				}
				else if (hit.collider.gameObject.tag == "Enemy")
				{
					hudView.UpdateEnemy(hit.collider.gameObject.GetComponent<Enemy>());
				}
				else if (hit.collider.gameObject.tag == "Metal" || hit.collider.gameObject.tag == "Electronics")
				{
					//change display to be for resources
				}
			}
		}


	}
}
