using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EffectConnector : MonoBehaviour
{
	public UnitManager unitManager;
	//public ResourceHandling resourceHandling;
	public RoomManager roomManager;

	//shrine
	public static int unitSpeed = 10;
	public static int unitBaseSpeed = 10;
	public static float unitFiringRate = 1f;
	public static float unitRecoveryTime = 60f;
	//public static int autoWeaponCooldown;
	//public static int roomResilience;

	//study
	public static float roomCost = 1;
	public static int unitWeaponDamage = 1;
	//public static int autoWeaponDamage;
	//public static int autoHealth;
	//public static int autoWeaponUpgrades;

	//refinery
	public static int efficiency = 0;

	//storage
	public int storageSpace = 100;
	public bool hasSpace = true;

    void Start()
    {
		Recalculate();
		//RecalculateRefineryStorage();
		//RecalculateControllerGenerator();
    }
    
    void Update()
    {
		AutomatonUI.debugText[0].text = "Unit Speed: " + unitSpeed;
		AutomatonUI.debugText[1].text = "Room Cost: " + (11 - roomCost)*10 + "%";
		AutomatonUI.debugText[2].text = "Efficiency: " + efficiency;
    }

	public void Recalculate()
	{
		//shrine related
		foreach (GameObject o in unitManager.unitsGM)
		{
			o.GetComponent<NavMeshAgent>().speed = unitSpeed;
		}

		unitManager.unitFireCooldown = unitFiringRate;

		unitManager.downTime -= unitRecoveryTime;

		//study related
		roomManager.refineryCost_M = roomManager.refineryCost_Mo * ((11 - roomCost)/10);
		Debug.Log("Cost M/E : " + roomManager.refineryCost_M);
		roomManager.refineryCost_E = roomManager.refineryCost_Eo * ((11 - roomCost) / 10);
		roomManager.storageCost_M = roomManager.storageCost_Mo * ((11-roomCost)/10);
		roomManager.storageCost_E = roomManager.storageCost_Eo * ((11-roomCost)/10);
		roomManager.shrineCost_M = roomManager.shrineCost_Mo * ((11-roomCost)/10);
		roomManager.shrineCost_E = roomManager.shrineCost_Eo * ((11-roomCost)/10);
		//roomManager.studyCost_M *= roomCost; -----------------------------------------------------> I'm scared of the combo capabilities, ie every room is free to build
		//roomManager.studyCost_E *= roomCost;

		unitManager.unitDamage = unitWeaponDamage;

		//refinery related
		roomManager.efficiency = efficiency;
	}

	//can more items be placed in storage?
	public bool StockCheck()
	{
		foreach (Room r in roomManager.rooms)
		{
			if (r.Type == "storage")
			{
				if (r.Level == 1)
				{
					storageSpace += 200;
				}
				else if (r.Level == 2)
				{
					storageSpace += 250;
				}
				else if (r.Level == 3)
				{
					storageSpace += 500;
				}
			}
		}

		if (storageSpace - (ResourceHandling.metal + ResourceHandling.plate + ResourceHandling.bolt + ResourceHandling.part + ResourceHandling.electronics + ResourceHandling.chip + ResourceHandling.wire + ResourceHandling.board) > 0)
		{
			hasSpace = true;
			return true;
		}
		else
		{
			hasSpace = false;
			return false;
		}
	}
}
