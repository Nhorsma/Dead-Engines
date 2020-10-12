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
	public static float unitFiringRate = 1f;
	public static float unitRecoveryTime = 60f;
	//public static int autoWeaponCooldown;
	//public static int roomResilience;

	//study
	public static int roomCost = 1;
	public static int unitWeaponDamage = 1;
	//public static int autoWeaponDamage;
	//public static int autoHealth;
	//public static int autoWeaponUpgrades;

	//refinery
	public static int efficiency = 1;

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
		roomManager.refineryCost_M *= roomCost;
		roomManager.refineryCost_E *= roomCost;
		roomManager.storageCost_M *= roomCost;
		roomManager.storageCost_E *= roomCost;
		roomManager.shrineCost_M *= roomCost;
		roomManager.shrineCost_E *= roomCost;
		roomManager.studyCost_M *= roomCost;
		roomManager.studyCost_E *= roomCost;

		unitManager.unitDamage = unitWeaponDamage;

		//refinery related
		roomManager.efficiency = efficiency;
	}

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
