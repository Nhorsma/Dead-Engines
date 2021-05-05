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
	public static int unitWeaponDamage = 2;
	//public static int autoWeaponDamage;
	//public static int autoHealth;
	//public static int autoWeaponUpgrades;

	//refinery
	public static int efficiency = 0;

    void Start()
    {
		Recalculate();
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
		foreach (GameObject u in unitManager.units)
		{
			u.GetComponent<NavMeshAgent>().speed = unitSpeed;
		}
		unitManager.unitFireCooldown = unitFiringRate;
		unitManager.downTime -= unitRecoveryTime;

		//study related
		//CostData.ChangeRoomCost();
		//unitManager.unitDamage = unitWeaponDamage;

		//refinery related
		roomManager.efficiency = efficiency;
	}
}
