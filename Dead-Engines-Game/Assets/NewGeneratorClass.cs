using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGeneratorClass : NewRoomClass
{
	private AutomotonAction automatonAction;

	//public Text gatheringUsage;
	public Text artilleryUsage;
	public Text laserUsage;
	public Text punchUsage;
	public Text kickUsage;
	//public Text movementUsage;

	public void Start()
	{
		automatonAction = FindObjectOfType<AutomotonAction>();
	}

	public void Update()
	{
		capacityText.text = "fuel: " + ResourceHandling.oil;

		artilleryUsage.text = "artillery fuel rate: " + automatonAction.barrageCost.ToString() + " /action";
		laserUsage.text = "laser fuel rate: " + automatonAction.lazerCost.ToString() + " /action";
		punchUsage.text = "punch fuel rate: " + automatonAction.punchCost.ToString() + " /action";
		kickUsage.text = "punch fuel rate: " + automatonAction.stompCost.ToString() + " /action";
	}

	public NewGeneratorClass()
	{

	}

	public void ToggleText()
	{
		buildButton.gameObject.SetActive(false);

		capacityText.gameObject.SetActive(true);
		artilleryUsage.gameObject.SetActive(true);
		laserUsage.gameObject.SetActive(true);
		punchUsage.gameObject.SetActive(true);
		kickUsage.gameObject.SetActive(true);
	}

}
