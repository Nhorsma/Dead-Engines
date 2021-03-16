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
	public Text objectInfo_1;
	public Text objectInfo_2;
	public Text healthNumber;
	public Slider healthSlider;

	// Start is called before the first frame update
	void Start()
    {
        
    }

	public void UpdateUnit(Unit unit)
	{
		hudPanel.SetActive(true);
		objectName.text = unit.UnitName;
		objectInfo_1.text = unit.Job;
		healthNumber.text = unit.Health.ToString();
		healthSlider.maxValue = unit.Health; // fix
		healthSlider.value = unit.Health;
	}

	public void UpdateEnemy(Enemy enemy)
	{
		hudPanel.SetActive(true);
		objectName.text = "Enemy";
		objectInfo_1.text = " ";
		objectInfo_2.text = enemy.Health.ToString();
		healthSlider.maxValue = enemy.Health; // fix
		healthSlider.value = enemy.Health;
	}

	public void UpdateEncampment(Encampment camp)
	{
		hudPanel.SetActive(true);
		objectName.text = "Encampment";
		objectInfo_1.text = " ";
		objectInfo_2.text = camp.Health.ToString();
		healthSlider.maxValue = camp.Health; // fix
		healthSlider.value = camp.Health;
	}

}
