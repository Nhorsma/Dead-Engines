using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
	public SetupRoom setupRoom; //hopefully will become obsolete
	public AutomatonUI auto;
	public UnitManager unitManager;
	public LimbSystem limbSystem;
	public TabCreation tabCreation;

	public GameObject autoObj;

	public GameObject controllerTab;
	public GameObject generatorTab;

	public List<Sprite> roomIcons;	//stored in script
	public List<Button> displayedIcons; //the ones being shown to the player in the automaton

	public Sprite generatorRepairedSprite;
	public Sprite controllerRepairedSprite;

	public int roomSlotClicked = 0;

	// sound fx
    public AudioSource audioSource;
    public AudioClip wrenchClip;
    public AudioClip hammerClip;
    public AudioClip errorClip;

	// effects. this is where hella data is stored, mostly data that changes with room effects
	public EffectConnector effectConnector;
	public int efficiency = 0; // this needs to be here for some reason I can't remember at the moment

	// repair individual rooms
	public static bool generatorRepaired = false;
	public static bool controllerRepaired = false;

	public int housingMax = 3;
	public int storageMax = 25;

	void Start()
    {

	}

    void Update()
    {
  //      if (Input.GetKeyDown(KeyCode.K))
		//{
		//	Debug.Log(CheckInfirmary());
		//}
    }

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void Build(string type)
	{
		if (type == "refinery" && ResourceHandling.metal >= CostData.build_refinery[2] && ResourceHandling.electronics >= CostData.build_refinery[3])
		{
			ResourceHandling.metal -= (int)CostData.build_refinery[2];
			ResourceHandling.electronics -= (int)CostData.build_refinery[3];
			tabCreation.Replace(roomSlotClicked, "refinery");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "storage" && ResourceHandling.metal >= CostData.build_storage[2] && ResourceHandling.electronics >= CostData.build_storage[3])
		{
			ResourceHandling.metal -= (int)CostData.build_storage[2];
			ResourceHandling.electronics -= (int)CostData.build_storage[3];
			tabCreation.Replace(roomSlotClicked, "storage");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "shrine" && ResourceHandling.metal >= CostData.build_shrine[2] && ResourceHandling.electronics >= CostData.build_shrine[3])
		{
			ResourceHandling.metal -= (int)CostData.build_shrine[2];
			ResourceHandling.electronics -= (int)CostData.build_shrine[3];
			tabCreation.Replace(roomSlotClicked, "shrine");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "study" && ResourceHandling.metal >= CostData.build_study[2] && ResourceHandling.electronics >= CostData.build_study[3])
		{
			ResourceHandling.metal -= (int)CostData.build_study[2];
			ResourceHandling.electronics -= (int)CostData.build_study[3];
			tabCreation.Replace(roomSlotClicked, "study");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "infirmary" && ResourceHandling.metal >= 10 && ResourceHandling.electronics >= 10)
		{
			ResourceHandling.metal -= 10;
			ResourceHandling.electronics -= 10;
			tabCreation.Replace(roomSlotClicked, "infirmary");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "dormitory" && ResourceHandling.metal >= 10 && ResourceHandling.electronics >= 10)
		{
			ResourceHandling.metal -= 10;
			ResourceHandling.electronics -= 10;
			tabCreation.Replace(roomSlotClicked, "dormitory");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "barracks" && ResourceHandling.metal >= 10 && ResourceHandling.electronics >= 10)
		{
			ResourceHandling.metal -= 10;
			ResourceHandling.electronics -= 10;
			tabCreation.Replace(roomSlotClicked, "barracks");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else if (type == "clonery" && ResourceHandling.metal >= 10 && ResourceHandling.electronics >= 10)
		{
			ResourceHandling.metal -= 10;
			ResourceHandling.electronics -= 10;
			tabCreation.Replace(roomSlotClicked, "clonery");
			UpdateIcon(roomSlotClicked, type);
			//CalculateLimbStats();
			PlayClip("wrench");
		}
		else
		{
			Debug.Log("Not enough resources to build a " + type + ".");
			PlayClip("error");
		}
	}

	//add infirmary stuff
	public void Assign(string roomType, int slot)
	{
		if (roomType != "infirmary")
		{
			if (unitManager.ReturnJoblessUnit() == null)
			{
				Debug.Log("No worker available");
				return;
			}
			else
			{
				GameObject unit = unitManager.ReturnJoblessUnit();

				switch (roomType)
				{
					case "refinery":
						if (TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Count < TabCreation.FindSlot(slot).GetComponent<NewRefinery>().WorkerCapacity)
						{
							unit.GetComponent<Unit>().Job = roomType;
							unit.GetComponent<Unit>().JobPos = autoObj;
							unitManager.SetJobFromRoom(unit, roomType);
							TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Add(unit);
							//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewRefinery>().WorkerCapacity.ToString();
							Produce();
							Debug.Log("Assigned [" + unit.GetComponent<Unit>().UnitName + "] to[" + roomType + "][" + slot + "]");
						}
						else
						{
							Debug.Log("Room is full");
							return;
						}
						break;
					case "shrine":
						if (TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Count < TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().WorkerCapacity)
						{
							unit.GetComponent<Unit>().Job = roomType;
							unit.GetComponent<Unit>().JobPos = autoObj;
							unitManager.SetJobFromRoom(unit, roomType);
							TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Add(unit);
							//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().WorkerCapacity.ToString();
							Worship();
							Debug.Log("Assigned [" + unit.GetComponent<Unit>().UnitName + "] to[" + roomType + "][" + slot + "]");
						}
						else
						{
							Debug.Log("Room is full");
							return;
						}
						break;
					case "study":
						if (TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Count < TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().WorkerCapacity) {
							unit.GetComponent<Unit>().Job = roomType;
							unit.GetComponent<Unit>().JobPos = autoObj;
							unitManager.SetJobFromRoom(unit, roomType);
							TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Add(unit);
							//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().WorkerCapacity.ToString();
							Research();
							Debug.Log("Assigned [" + unit.GetComponent<Unit>().UnitName + "] to[" + roomType + "][" + slot + "]");
						}
						else
						{
							Debug.Log("Room is full");
							return;
						}
						break;
					default:
						break;
				}
			}
		}
		else if (roomType == "infirmary")
		{
			if (unitManager.ReturnKnockedOutUnit() == null)
			{
				Debug.Log("No knocked out units available");
				return;
			}
			else
			{
				GameObject unit = unitManager.ReturnKnockedOutUnit(); //ReturnKnockedOutUnit()

				if (TabCreation.FindSlot(slot).GetComponent<NewInfirmaryClass>().Workers.Count < TabCreation.FindSlot(slot).GetComponent<NewInfirmaryClass>().WorkerCapacity)
				{
					//assign to this room
					unit.GetComponent<Unit>().Job = roomType;
					unit.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(unit, roomType);
					TabCreation.FindSlot(slot).GetComponent<NewInfirmaryClass>().Workers.Add(unit);
					//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewInfirmaryClass>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewInfirmaryClass>().WorkerCapacity.ToString();

					Debug.Log("Assigned [" + unit.GetComponent<Unit>().UnitName + "] to[" + roomType + "][" + slot + "]");
				}
				else
				{
					Debug.Log("Room is full");
					return;
				}
			}
		}
	}

	//add infirmary stuff -> a variation for a timed unassign of sorts to mimic healing, maybe in unit manager?
	public void Unassign(string roomType, int slot)
	{
		switch (roomType)
		{
			case "refinery":
				if (TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Count == 0)
				{
					Debug.Log("No worker to unassign");
					return;
				}
				else
				{
					GameObject unit = TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers[0]; //first unit
					unit.GetComponent<Unit>().Job = "none";
					unitManager.LeaveRoomJob(unit);
					unitManager.TravelTo(unit, unit.transform.position, false, false);
					TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Remove(unit);
					//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewRefinery>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewRefinery>().WorkerCapacity.ToString();
					Produce();
					Debug.Log("Unassigned [" + unit.GetComponent<Unit>().UnitName + "] from [" + roomType + "][" + slot + "]");
				}
				break;
			case "shrine":
				if (TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Count == 0)
				{
					Debug.Log("No worker to unassign");
					return;
				}
				else
				{
					GameObject unit = TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers[0]; //first unit
					unit.GetComponent<Unit>().Job = "none";
					unitManager.LeaveRoomJob(unit);
					unitManager.TravelTo(unit, unit.transform.position, false, false);
					TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Remove(unit);
					//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().WorkerCapacity.ToString();
					Worship();
					Debug.Log("Unassigned [" + unit.GetComponent<Unit>().UnitName + "] from [" + roomType + "][" + slot + "]");
				}
				break;
			case "study":
				if (TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Count == 0)
				{
					Debug.Log("No worker to unassign");
					return;
				}
				else
				{
					GameObject unit = TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers[0]; //first unit
					unit.GetComponent<Unit>().Job = "none";
					unitManager.LeaveRoomJob(unit);
					unitManager.TravelTo(unit, unit.transform.position, false, false);
					TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Remove(unit);
					//setupRoom.roomComponents[slot].capacity.text = TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().Workers.Count.ToString() + " / " + TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().WorkerCapacity.ToString();
					Research();
					Debug.Log("Unassigned [" + unit.GetComponent<Unit>().UnitName + "] from [" + roomType + "][" + slot + "]");
				}
				break;
			default:
				break;
		}
	}

	public void Refine(string what, int howMany)
	{
		int efficiencyRand = 0;
		int efficiencyBonus = 0;

		if (EffectConnector.efficiency > 0)
		{
			if (CanAfford(what, howMany))
			{
				efficiencyRand = Random.Range(1, 11);

				if (efficiencyRand <= EffectConnector.efficiency)
				{
					efficiencyBonus = 1;
				}
				else
				{
					efficiencyBonus = 0;
				}

				if (what == "bolt")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.bolt++;
						ResourceHandling.metal -= (CostData.metal_bolt - efficiencyBonus);
					}
				}
				else if (what == "plate")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.plate++;
						ResourceHandling.metal -= (CostData.metal_plate - efficiencyBonus);
					}
				}
				else if (what == "part")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.part++;
						ResourceHandling.bolt -= (CostData.special_part[0] - efficiencyBonus);
						ResourceHandling.plate -= (CostData.special_part[1] - efficiencyBonus);
					}
				}
				else if (what == "wire")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.wire++;
						ResourceHandling.electronics -= (CostData.electronics_wire - efficiencyBonus);
					}
				}
				else if (what == "chip")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.chip++;
						ResourceHandling.electronics -= (CostData.electronics_chip - efficiencyBonus);
					}
				}
				else if (what == "board")
				{
					for (int i = 0; i < howMany; i++)
					{
						ResourceHandling.board++;
						ResourceHandling.wire -= (CostData.special_board[0] - efficiencyBonus);
						ResourceHandling.chip -= (CostData.special_board[1] - efficiencyBonus);
					}
				}
				PlayClip("hammer");
			}
			else
			{
				Debug.Log("Not enough resources");
                PlayClip("error");
            }
		}
		else
		{
			Debug.Log("No workers");
			PlayClip("error");
		}
	} 

	public void Discard(string what, int howMany)
	{
		if (what == "metal" && ResourceHandling.metal >= howMany)
		{
			ResourceHandling.metal -= howMany;
		}
		else if (what == "bolt" && ResourceHandling.bolt >= howMany)
		{
			ResourceHandling.bolt -= howMany;
		}
		else if (what == "plate" && ResourceHandling.plate >= howMany)
		{
			ResourceHandling.plate -= howMany;
		}
		else if (what == "part" && ResourceHandling.part >= howMany)
		{
			ResourceHandling.part -= howMany;
		}
		else if (what == "electronics" && ResourceHandling.electronics >= howMany)
		{
			ResourceHandling.electronics -= howMany;
		}
		else if (what == "wire" && ResourceHandling.wire >= howMany)
		{
			ResourceHandling.wire -= howMany;
		}
		else if (what == "chip" && ResourceHandling.chip >= howMany)
		{
			ResourceHandling.chip -= howMany;
		}
		else if (what == "board" && ResourceHandling.board >= howMany)
		{
			ResourceHandling.board -= howMany;
		}
		else
		{
			Debug.Log("You do not have " + howMany + " " + what + ".");
		}
	}

	//rewrite
	public void Bedrest(int slot)
	{
		Assign("infirmary", slot);
		
	}

	// need to clean
	// worship and research methods only need to be called when their multiplier changes, or when a unit is assigned successfully. this will definitely save on horsepower
	public void Worship()
	{
		int combinedMultiplier = 0;
		List<string> effects = new List<string>();
		effects.Clear();

		foreach (NewRoomClass r in TabCreation.rooms)
		{
			if (r.Type == "shrine")
			{
				Debug.Log("Shrine[" + r.Slot + "]: " + r.GetComponent<NewShrineClass>().Workers.Count + "x boost");
				combinedMultiplier += r.GetComponent<NewShrineClass>().Workers.Count; //case for multiples of the same room
				effects.Add(r.GetComponent<NewShrineClass>().ActiveEffect);
			}
		}
		Debug.Log("Total Shrine Multiplier: " + combinedMultiplier);

		//calculate effects
		foreach (string e in effects)
		{
			if (e == "unitSpeed")  //-----------------------> r.ActiveEffect will be set by a button that is activated once room is upgraded
			{
				EffectConnector.unitSpeed = EffectConnector.unitBaseSpeed + combinedMultiplier;
			}
			else if (e == "none")
			{
				EffectConnector.unitSpeed = EffectConnector.unitBaseSpeed + 0; // --------------------------------------------------------------------> forseeable issues with multiple shrines
			}
		}
		effectConnector.Recalculate();
	}

	// need to clean
	public void Research()
	{
		int combinedMultiplier = 0;
		List<string> effects = new List<string>();
		effects.Clear();

		foreach (NewRoomClass r in TabCreation.rooms)
		{
			if (r.Type == "study")
			{
				Debug.Log("Study[" + r.Slot + "]: " + r.GetComponent<NewStudyClass>().Workers.Count + "x boost");
				combinedMultiplier += r.GetComponent<NewStudyClass>().Workers.Count; //case for multiples of the same room
				effects.Add(r.GetComponent<NewStudyClass>().ActiveEffect);
			}

		}
		Debug.Log("Total Study Multiplier: " + combinedMultiplier);

		//calculate effects
		foreach (string e in effects)
		{
			if (e == "roomCost")    //-----------------------> r.ActiveEffect will be set by a button that is activated once room is upgraded
			{
				EffectConnector.roomCost = combinedMultiplier + 1;
			}
			else if (e == "none")
			{
				EffectConnector.roomCost = 1 + 0; // ------------------------------------------------------------------------------------------------> forseeable issues with multiple studies
				return;
			}

		}
		effectConnector.Recalculate();
	}

	public void Produce()
	{
		EffectConnector.efficiency = 0;
		foreach (NewRoomClass r in TabCreation.rooms)
		{
			if (r.Type == "refinery")
			{
				if (r.GetComponent<NewRefinery>().Workers.Count > 0)
				{
					Debug.Log("Refinery[" + r.Slot + "]: Running");
					EffectConnector.efficiency += r.GetComponent<NewRefinery>().Workers.Count;
				}
			}
		}
		effectConnector.Recalculate();
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	//rewrite
	public void OpenRoom(int clickedSlot)
	{
		TabCreation.FindSlot(clickedSlot).roomTab.SetActive(true);
		//Debug.Log("Current tab " + clickedSlot);
	}

	public void OpenController()
	{
		controllerTab.gameObject.SetActive(true);
	}
	public void OpenGenerator()
	{
		generatorTab.gameObject.SetActive(true);
	}

	public void TakeToBuild(int clickedSlot)
	{
		roomSlotClicked = clickedSlot;
		auto.OpenBuildTab();
	}

	// this will need to get moved to SetupRoom once I make it a functional room
	public void RepairGenerator()
	{
		if (ResourceHandling.metal >= CostData.repair_generator)
		{
			ResourceHandling.metal -= CostData.repair_generator;
			generatorRepaired = true;
			generatorTab.GetComponent<RoomComponents>().build.gameObject.SetActive(false);
			generatorTab.GetComponent<RoomComponents>().pic.sprite = generatorRepairedSprite;
		}
	}
	public void RepairController()
	{
		if (ResourceHandling.electronics >= CostData.repair_controller)
		{
			ResourceHandling.electronics -= CostData.repair_controller;
			controllerRepaired = true;
			controllerTab.GetComponent<RoomComponents>().build.gameObject.SetActive(false);
			controllerTab.GetComponent<RoomComponents>().pic.sprite = controllerRepairedSprite;
		}
	}

	public void UpdateIcon(int slot, string type)
	{
		switch (type)
		{
			case "empty":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[0];
				break;
			case "refinery":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[1];
				break;
			case "storage":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[2];
				break;
			case "shrine":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[3];
				break;
			case "study":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[4];
				break;
			case "infirmary":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[5];
				break;
			case "dormitory":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[6];
				break;
			case "barracks":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[7];
				break;
			case "clonery":
				displayedIcons[slot].GetComponent<Image>().sprite = roomIcons[8];
				break;
			default:
				break;
		}

		auto.OpenRoomsTab();
	}

	public bool CanAfford(string what, int howMany)
	{
		switch (what)
		{
			case "bolt":
				return (ResourceHandling.metal >= 1 * howMany);
			case "plate":
				return (ResourceHandling.metal >= 3 * howMany);
			case "part":
				return (ResourceHandling.bolt >= 3 * howMany && ResourceHandling.plate >= 2 * howMany);
			case "wire":
				return (ResourceHandling.electronics >= 1 * howMany);
			case "chip":
				return (ResourceHandling.electronics >= 3 * howMany);
			case "board":
				return (ResourceHandling.wire >= 2 * howMany && ResourceHandling.chip >= 3 * howMany);
			default:
				return false;
		}
	}

	//rewrite
	public void SetActiveEffect(string buff, int slot)
	{
		//TabCreation.FindSlot(slot).ActiveEffect = buff;
		switch (TabCreation.FindSlot(slot).Type)
		{
			case "shrine":
				TabCreation.FindSlot(slot).GetComponent<NewShrineClass>().ActiveEffect = buff;
				Worship();
				break;
			case "study":
				TabCreation.FindSlot(slot).GetComponent<NewStudyClass>().ActiveEffect = buff;
				Research();
				break;
		}
		//Debug.Log("Active effect of [ " + rooms[slot].Type + " " + slot + " ] is now : " + rooms[slot].ActiveEffect + ".");
	}

    void PlayClip(string str)
    {
		switch (str)
		{
			case "wrench":
				audioSource.PlayOneShot(wrenchClip);
				break;
			case "hammer":
				audioSource.PlayOneShot(hammerClip);
				break;
			case "error":
				audioSource.PlayOneShot(errorClip);
				break;
		}
    }

	//rewrite
	public bool CheckInfirmary()
	{
		foreach (NewShrineClass r in TabCreation.rooms) //broken on purpose >:)
		{
			if (r.Type == "infirmary")
			{
				if (r.Workers.Count < r.WorkerCapacity)
				{
					return true;
				}
			}
		}
		return false;
	}

	//for barracks and clonery
	public Unit ChooseUnit(int unit_id)
	{
		return unitManager.units[unit_id].GetComponent<Unit>();
	}

	//find unit to change, then change
	public void UseBarracks(Unit unit_data, string newType)
	{
		Debug.Log(unit_data.Attack + " " + unit_data.Defense);
		unit_data.ChangeType(newType);
		Debug.Log(unit_data.Attack + " " + unit_data.Defense);
		//pay cost
	}

	public void UseClonery(Unit unit_data)
	{
		unitManager.CloneUnit(unit_data);
		//pay cost
	}

	public bool CheckDormitories()
	{
		housingMax = 3;
		foreach (NewRoomClass d in TabCreation.rooms)
		{
			if (d.Type == "dormitory")
			{
				Debug.Log("dorm count");
				housingMax += 3 * d.Level;
			}
		}
		Debug.Log(housingMax);
		if (unitManager.units.Count < housingMax)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public bool CheckStorage()
	{
		storageMax = 25;
		foreach (NewRoomClass s in TabCreation.rooms)
		{
			if (s.Type == "storage")
			{
				storageMax += 50 * s.Level;
			}
		}

		if (ResourceHandling.storageUsed < storageMax)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	public void CalculateLimbStats(int attack, int defense, int health)
	{
		//determine limb from slot
		string limbToUpdate = "limb";
		limbSystem.ChangeAttack(limbToUpdate, attack);
		limbSystem.ChangeDefense(limbToUpdate, defense);
		limbSystem.ChangeHealth(limbToUpdate, health);
		limbSystem.performedUpdate = true;
	}

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
	}
}
