using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverInfo : MonoBehaviour
{

	public GameObject hoverBox;

	public Text amountText;
	private int depositQuantity;

	private Ray ray;
	private RaycastHit hit;

	public ResourceHandling resourceHandling;
	private GameObject resourceObj;

	public EncampmentHandler encampmentHandling;
	private GameObject encampmentObj;
	private int encampmentHealth;

    void Start()
    {
		amountText.text = " ";
    }

	/// <summary>
	/// https://answers.unity.com/questions/547513/how-do-i-detect-when-mouse-passes-over-an-object.html
	/// </summary>
	void Update()
    {
		ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if (Physics.Raycast(ray, out hit))
		{
			if (hit.collider.tag == "Metal" || hit.collider.tag == "Electronics")
			{
				//display
				resourceObj = hit.collider.gameObject;
				//further proof we should just rewrite Resources like actual objects imo
				//depositQuantity = resourceHandling.resourceQuantities[resourceHandling.GetNumber(resourceObj)]; //here
				
				SetupHoverBox(resourceObj, depositQuantity);
				DisplayHoverBox();
			}
			else if (hit.collider.tag == "Encampment")
			{
				encampmentObj = hit.collider.gameObject;
				encampmentHealth = encampmentObj.GetComponent<Encampment>().Health;
				SetupHoverBox(encampmentObj, encampmentHealth);
				DisplayHoverBox();
			}
			else if (hit.collider.tag != "Metal" || hit.collider.tag != "Electronics" || hit.collider.tag != "Encampment")
			{
				hoverBox.SetActive(false);
			}
		}
	}

	void SetupHoverBox(GameObject obj, int amount)
	{
		amountText.text = amount.ToString();
		hoverBox.transform.position = Camera.main.WorldToScreenPoint(new Vector3(obj.transform.position.x, obj.transform.position.y+5, obj.transform.position.z));
	}

	void DisplayHoverBox()
	{
		hoverBox.SetActive(true);
	}
}
