using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
	public SetupRoom setupRoom;
	public AutomatonUI auto;
	public UnitManager unitManager;

	public GameObject autoObj;

	// room data & tabs
	public List<Room> rooms = new List<Room>(); // room_data
	public List<GameObject> roomTabs = new List<GameObject>(); // the individual room panel
	public GameObject controllerTab;
	public GameObject generatorTab;

	public List<Text> roomTypesAtAGlance; //change to gameObject later -> do I still need to do this???
	public List<GameObject> roomSpritesInOrder; // --------------------------------------------------> is this being used?

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

	// need to clean
	void Start()
    {
		// might have a problem with list passing?
		InitializeRooms();

		// this is actually important
		UpdateRoomDisplay();
	}

    void Update()
    {
        
    }

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void InitializeRooms()
	{
		for (int i = 0; i < 7; i++)
		{
			rooms.Add(new Room("empty", i, 0));
		}
	}

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void Build(string room)
	{
		if (room == "refinery" && ResourceHandling.metal >= CostData.build_refinery[2] && ResourceHandling.electronics >= CostData.build_refinery[3])
		{
			ResourceHandling.metal -= (int)CostData.build_refinery[2];
			ResourceHandling.electronics -= (int)CostData.build_refinery[3];
			rooms[roomSlotClicked] = new Refinery(roomSlotClicked, 1);
			setupRoom.Setup(rooms[roomSlotClicked]); // ->
			PlayClip("wrench");
		}
		else if (room == "storage" && ResourceHandling.metal >= CostData.build_storage[2] && ResourceHandling.electronics >= CostData.build_storage[3])
		{
			ResourceHandling.metal -= (int)CostData.build_storage[2];
			ResourceHandling.electronics -= (int)CostData.build_storage[3];
			rooms[roomSlotClicked] = new Storage(roomSlotClicked, 1);
			setupRoom.Setup(rooms[roomSlotClicked]);
			PlayClip("wrench");
		}
		else if (room == "shrine" && ResourceHandling.metal >= CostData.build_shrine[2] && ResourceHandling.electronics >= CostData.build_shrine[3])
		{
			ResourceHandling.metal -= (int)CostData.build_shrine[2];
			ResourceHandling.electronics -= (int)CostData.build_shrine[3];
			rooms[roomSlotClicked] = new Shrine(roomSlotClicked, 1);
			setupRoom.Setup(rooms[roomSlotClicked]);
			PlayClip("wrench");
		}
		else if (room == "study" && ResourceHandling.metal >= CostData.build_study[2] && ResourceHandling.electronics >= CostData.build_study[3])
		{
			ResourceHandling.metal -= (int)CostData.build_study[2];
			ResourceHandling.electronics -= (int)CostData.build_study[3];
			rooms[roomSlotClicked] = new Study(roomSlotClicked, 1);
			setupRoom.Setup(rooms[roomSlotClicked]);
			PlayClip("wrench");
		}
		else if (room == "infirmary" && ResourceHandling.metal >= 10 && ResourceHandling.electronics >= 10)
		{
			ResourceHandling.metal -= 10;
			ResourceHandling.electronics -= 10;
			rooms[roomSlotClicked] = new Infirmary(roomSlotClicked, 1);
			setupRoom.Setup(rooms[roomSlotClicked]);
			PlayClip("wrench");
		}
		else
		{
			Debug.Log("Not enough resources to build a " + room + ".");
			PlayClip("error");
		}
		UpdateRoomDisplay();
	}

	//doing twice for shrine & study?
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

				if (rooms[slot].Workers.Count < rooms[slot].WorkerCapacity)
				{
					unit.GetComponent<Unit>().Job = roomType;
					unit.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(unit, roomType);
					rooms[slot].Workers.Add(unit);
					setupRoom.roomComponents[slot].capacity.text = rooms[slot].Workers.Count.ToString() + " / " + rooms[slot].WorkerCapacity.ToString();

					switch (roomType)
					{
						case "refinery":
							Produce();
							break;
						case "shrine":
							Worship();
							break;
						case "study":
							Research();
							break;
					}

					Debug.Log("Assigned [" + unit.GetComponent<Unit>().UnitName + "] to[" + roomType + "][" + slot + "]");
				}
				else
				{
					Debug.Log("Room is full");
					return;
				}
			}
		}
		else if (roomType == "infirmary")
		{
			if (unitManager.ReturnJoblessUnit() == null) //ReturnKnockedOutUnit()
			{
				Debug.Log("No knocked out units available");
				return;
			}
			else
			{
				GameObject unit = unitManager.ReturnJoblessUnit(); //ReturnKnockedOutUnit()

				if (rooms[slot].Workers.Count < rooms[slot].WorkerCapacity)
				{
					//assign to this room
					unit.GetComponent<Unit>().Job = roomType;
					unit.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(unit, roomType);
					rooms[slot].Workers.Add(unit);
					setupRoom.roomComponents[slot].capacity.text = rooms[slot].Workers.Count.ToString() + " / " + rooms[slot].WorkerCapacity.ToString();

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

	//add infirmary stuff
	public void Unassign(string roomType, int slot)
	{
		if (rooms[slot].Workers.Count == 0)
		{
			Debug.Log("No worker to unassign");
			return;
		}
		else
		{
			GameObject unit = rooms[slot].Workers[0]; //first unit

			unit.GetComponent<Unit>().Job = "none";
			unitManager.LeaveRoomJob(unit);
			unitManager.TravelTo(unit, unit.transform.position, false, false);
			rooms[slot].Workers.Remove(unit);

			setupRoom.roomComponents[slot].capacity.text = rooms[slot].Workers.Count.ToString() + " / " + rooms[slot].WorkerCapacity.ToString();

			switch (roomType)
			{
				case "refinery":
					Produce();
					break;
				case "shrine":
					Worship();
					break;
				case "study":
					Research();
					break;
			}
			Debug.Log("Unassigned [" + unit.GetComponent<Unit>().UnitName + "] from [" + roomType + "][" + slot + "]");
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

		foreach (Room r in rooms)
		{
			if (r.Type == "shrine")
			{
				Debug.Log("Shrine[" + r.Slot + "]: " + r.Workers.Count + "x boost");
				combinedMultiplier += r.Workers.Count; //case for multiples of the same room
				effects.Add(r.ActiveEffect);
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

		foreach (Room r in rooms)
		{
			if (r.Type == "study")
			{
				Debug.Log("Study[" + r.Slot + "]: " + r.Workers.Count + "x boost");
				combinedMultiplier += r.Workers.Count; //case for multiples of the same room
				effects.Add(r.ActiveEffect);
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
		foreach (Room r in rooms)
		{
			if (r.Type == "refinery")
			{
				if (r.Workers.Count > 0)
				{
					Debug.Log("Refinery[" + r.Slot + "]: Running");
					EffectConnector.efficiency += r.Workers.Count;
				}
			}
		}
		effectConnector.Recalculate();
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void OpenRoom(int clickedSlot)
	{
		roomTabs[clickedSlot].gameObject.SetActive(true); //...which is different. this is the panel itself
		Debug.Log("Current tab " + clickedSlot);
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
		if (ResourceHandling.metal >= 100)
		{
			ResourceHandling.metal -= 100;
			generatorRepaired = true;
			generatorTab.GetComponent<RoomComponents>().build.gameObject.SetActive(false);
			generatorTab.GetComponent<RoomComponents>().pic.sprite = generatorRepairedSprite;
		}
	}
	public void RepairController()
	{
		if (ResourceHandling.electronics >= 100)
		{
			ResourceHandling.electronics -= 100;
			controllerRepaired = true;
			controllerTab.GetComponent<RoomComponents>().build.gameObject.SetActive(false);
			controllerTab.GetComponent<RoomComponents>().pic.sprite = controllerRepairedSprite;
		}
	}

	public void UpdateRoomDisplay()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			roomTypesAtAGlance[i].text = rooms[i].Type;
			roomSpritesInOrder[i].GetComponent<Image>().sprite = setupRoom.roomComponents[i].pic.sprite;
		}

		Debug.Log("Updated Rooms");
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

	public void SetActiveEffect(string buff, int slot)
	{
		rooms[slot].ActiveEffect = buff;
		switch (rooms[slot].Type)
		{
			case "shrine":
				Worship();
				break;
			case "study":
				Research();
				break;
		}
		Debug.Log("Active effect of [ " + rooms[slot].Type + " " + slot + " ] is now : " + rooms[slot].ActiveEffect + ".");
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

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
	}
}
