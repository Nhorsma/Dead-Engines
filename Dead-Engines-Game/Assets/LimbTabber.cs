using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbTabber : MonoBehaviour
{
	public List<GameObject> limbTabs = new List<GameObject>();

	public void OpenLimbTab(string limb)
	{
		foreach (GameObject o in limbTabs)
		{
			o.SetActive(false);
		}
		switch (limb)
		{
			case "torso":
				limbTabs[0].SetActive(true);
				break;
			case "left_arm":
				limbTabs[1].SetActive(true);
				break;
			case "left_leg":
				limbTabs[2].SetActive(true);
				break;
			case "right_arm":
				limbTabs[3].SetActive(true);
				break;
			case "right_leg":
				limbTabs[4].SetActive(true);
				break;
			default:
				break;
		}
	}

}
