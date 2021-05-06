using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudView : MonoBehaviour
{

	// to do:
	//	- hook up canvas objects to this script
	//	- hook this script up to SelectItems
	//	- call the correct method depending on what was left clicked
	//	- fix the box selection unit information to group logic in UnitManager
	//	- test, test, and test again

	public GameObject hudPanel;
	public Text objectName;
	public Text objectInfo_1; // health ------------------
	public Text objectInfo_2; // attack ------------------
	public Text objectInfo_3; // defense -----------------
	public Text objectInfo_4; // armor piercing ----------
	public Text objectInfo_5; // speed -------------------
	public Text objectInfo_6; // current job -------------

	public List<Image> unitSpecificIcons = new List<Image>();

	// Start is called before the first frame update
	void Start()
    {
        
    }

	public void UpdateUnit(Unit unit)
	{
		hudPanel.SetActive(true);
		foreach (Image i in unitSpecificIcons)
		{
			i.gameObject.SetActive(true);
		}
		objectName.text = unit.UnitName;
		objectInfo_1.text = unit.Health.ToString();
		objectInfo_2.text = unit.Attack.ToString();
		objectInfo_3.text = unit.Defense.ToString();
		objectInfo_4.text = "x";
		objectInfo_5.text = EffectConnector.unitSpeed.ToString();
		objectInfo_6.text = unit.Job.ToString();
	}

	public void UpdateGroup(Unit unit)
	{
		hudPanel.SetActive(true);
		foreach (Image i in unitSpecificIcons)
		{
			i.gameObject.SetActive(false);
		}
		objectName.text = "Unit Group";
		objectInfo_1.text = "";
		objectInfo_2.text = "";
		objectInfo_3.text = "";
		objectInfo_4.text = "";
		objectInfo_5.text = "";
		objectInfo_6.text = "";
	}

	public void UpdateEnemy(Enemy enemy)
	{
		hudPanel.SetActive(true);

		unitSpecificIcons[0].gameObject.SetActive(true);
		unitSpecificIcons[1].gameObject.SetActive(true);
		unitSpecificIcons[2].gameObject.SetActive(true);
		unitSpecificIcons[3].gameObject.SetActive(false);
		unitSpecificIcons[4].gameObject.SetActive(false);

		objectName.text = enemy.name;
		objectInfo_1.text = enemy.Health.ToString();
		objectInfo_2.text = enemy.Attack.ToString();
		objectInfo_3.text = enemy.Defense.ToString();
		objectInfo_4.text = "";
		objectInfo_5.text = "";
		objectInfo_6.text = "";
	}

	public void UpdateEncampment(Encampment camp)
	{
		hudPanel.SetActive(true);
		foreach (Image i in unitSpecificIcons)
		{
			i.gameObject.SetActive(false);
		}
		unitSpecificIcons[0].gameObject.SetActive(true);

		objectName.text = "Encampment";
		objectInfo_1.text = camp.Health.ToString();
		objectInfo_2.text = "";
		objectInfo_3.text = "";
		objectInfo_4.text = "";
		objectInfo_5.text = "";
		objectInfo_6.text = "";
	}

	public void UpdateResource(Resource resource)
	{
		hudPanel.SetActive(true);
		foreach (Image i in unitSpecificIcons)
		{
			i.gameObject.SetActive(false);
		}
		unitSpecificIcons[0].gameObject.SetActive(true);

		objectName.text = "Resource Deposit";
		objectInfo_1.text = resource.Quantity.ToString();
		objectInfo_2.text = "";
		objectInfo_3.text = "";
		objectInfo_4.text = "";
		objectInfo_5.text = "";
		objectInfo_6.text = "Type: " + resource.Type;
	}

	public void CloseBox()
	{
		hudPanel.SetActive(false);
	}
}
