using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
	// pulling scripts
	public SpawnRes spawnRes;
    public ResourceHandling resourceHandling;
    public SelectItems selectItems;

	// room data & display
	public List<Room> rooms = new List<Room>(); // room_data
	public List<Text> display; //change to gameObject later -> do I still need to do this???
	public List<GameObject> displaySprites; // --------------------------------------------------> is this being used?
	public List<Sprite> rightRoomSprites; // 4, 5, 6
	public List<Sprite> leftRoomSprites; // 0, 1, 2, 3
	public Sprite generatorRepairedSprite;
	public Sprite controllerRepairedSprite;

	// pulling scripts
	public int roomSlotClicked = 0;
	public GameObject autoObj;
	public AutomatonUI auto;
    public UnitManager unitManager;
    public HunterHandler hunterHandler;

	// I believe miniTabs are the physically room parts, maybe they should be renamed?
	public List<GameObject> roomComponents = new List<GameObject>(); //-----------------------> miniTabs is getting renamed to "roomComponents"
	public GameObject controllerComponents;
	public GameObject generatorComponents;

	// sound fx
    public AudioSource audioSource;
    public AudioClip wrenchClip;
    public AudioClip hammerClip;
    public AudioClip errorClip;

	/// <summary>
	/// this could be so much better
	/// </summary>
	public float refineryCost_M;
	public float refineryCost_E;

	public float storageCost_M;
	public float storageCost_E;

	public float shrineCost_M;
	public float shrineCost_E;

	public float studyCost_M;
	public float studyCost_E;

	//origs
	/// <summary>
	/// I hate that I have to do this
	/// </summary>
	public float refineryCost_Mo;
	public float refineryCost_Eo;

	public float storageCost_Mo;
	public float storageCost_Eo;

	public float shrineCost_Mo;
	public float shrineCost_Eo;

	public float studyCost_Mo;
	public float studyCost_Eo;


	// effects. this is where hella data is stored, mostly data that changes with room effects
	public EffectConnector effectConnector;
	public int efficiency = 0;

	//room-specific data used for construction; maybe move these to a different script?
	public List<Text> refineryEntries;
	public List<Text> refineryCosts;
	public List<Button> refineryButtons;

	public List<Text> storageEntries;
	public List<Text> storageDesc;
	public List<Button> storageButtons;

	public List<Button> shrineEffectButtons;
	public List<string> shrineEffectKeys;
	public List<string> shrineEffectDescs;

	public List<Button> studyEffectButtons;
	public List<string> studyEffectKeys;
	public List<string> studyEffectDescs;

	// room worker capacity? add generator 1 & control 1 later
	public int refineryCapacity;
	public int shrineCapacity;
	public int studyCapacity;

	// trigger phase 2; maybe this can be moved to a static data-ish file
	public bool isAutomatonRepaired = false;
	public static bool generatorRepaired = false;
	public static bool controllerRepaired = false;

	void Start()
    {
		// might have a problem with list passing?
		InitializeRooms();

		// I hate that I had to do this -_-
		refineryCost_Mo = refineryCost_M;
		refineryCost_Eo = refineryCost_E;

		storageCost_Mo = storageCost_M;
		storageCost_Eo = storageCost_E;

		shrineCost_Mo = shrineCost_M;
		shrineCost_Eo = shrineCost_E;

		studyCost_Mo = studyCost_M;
		studyCost_Eo = studyCost_E;

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
		if (room == "refinery" && ResourceHandling.metal >= refineryCost_M && ResourceHandling.electronics >= refineryCost_E)
		{
			ResourceHandling.metal -= (int)refineryCost_M;
			ResourceHandling.electronics -= (int)refineryCost_E;
			//rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1); // ->
			rooms[roomSlotClicked] = new Refinery(roomSlotClicked, 1, 15, 0);
			SetupRefinery(roomSlotClicked); // ->
			PlayClip("wrench");
		}
		else if (room == "storage" && ResourceHandling.metal >= storageCost_M && ResourceHandling.electronics >= refineryCost_E)
		{
			ResourceHandling.metal -= (int)storageCost_M;
			ResourceHandling.electronics -= (int)storageCost_E;
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
			SetupStorage(roomSlotClicked);
			PlayClip("wrench");
		}
		else if (room == "shrine" && ResourceHandling.metal >= shrineCost_M && ResourceHandling.electronics >= shrineCost_E)
		{
			ResourceHandling.metal -= (int)shrineCost_M;
			ResourceHandling.electronics -= (int)shrineCost_E;
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
			SetupShrine(roomSlotClicked);
			PlayClip("wrench");
		}
		else if (room == "study" && ResourceHandling.metal >= studyCost_M && ResourceHandling.electronics >= studyCost_E)
		{
			ResourceHandling.metal -= (int)studyCost_M;
			ResourceHandling.electronics -= (int)studyCost_E;
			rooms[roomSlotClicked] = new Room("study", roomSlotClicked, 1);
			SetupStudy(roomSlotClicked);
			PlayClip("wrench");
		}
		else
		{
			Debug.Log("Not enough resources to build a " + room + ".");
			PlayClip("error");
		}
		UpdateRoomDisplay();
	}

	public void Assign(string where, Room r)
	{
		if (unitManager.ReturnJoblessUnit() == null)
		{
			Debug.Log("No worker available");
			return;
		}
		else
		{
			GameObject who = unitManager.ReturnJoblessUnit();

			if (r.Type == "shrine")
			{
				if (r.Workers.Count >= shrineCapacity)
				{
					Debug.Log("Room is full");
					return;
				}
				else
				{
					who.GetComponent<Unit>().Job = where;
					who.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + shrineCapacity.ToString();

					Worship();
					Debug.Log("Assigned [" + who.GetComponent<Unit>().UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
				}
			}
			else if (r.Type == "study")
			{
				if (r.Workers.Count >= studyCapacity)
				{
					Debug.Log("Room is full");
					return;
				}
				else
				{
					who.GetComponent<Unit>().Job = where;
					who.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + studyCapacity.ToString();

					Research();
					Debug.Log("Assigned [" + who.GetComponent<Unit>().UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
				}
			}
			else if (r.Type == "refinery")
			{
				if (r.Workers.Count >= refineryCapacity)
				{
					Debug.Log("Room is full");
					return;
				}
				else
				{
					who.GetComponent<Unit>().Job = where;
					who.GetComponent<Unit>().JobPos = autoObj;
					unitManager.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + refineryCapacity.ToString();

					Produce();
					Debug.Log("Assigned [" + who.GetComponent<Unit>().UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
				}
			}

			//
			//method does not exist yet
			//info.UpdateUnitViewer();
		}
	}

	public void Unassign(string where, Room r)
	{
		if (r.Workers.Count == 0)
		{
			Debug.Log("No worker available");
			return;
		}
		else
		{
			GameObject who = r.Workers[0]; //first unit
			Debug.Log("Unassigned [" + who.GetComponent<Unit>().UnitName + "] from [" + r.Type + "][" + r.Slot + "]");
			who.GetComponent<Unit>().Job = "none";

			//who.JobPos = autoObj; // set it to outside ////////////////////////////////////////////////////////////////////
			unitManager.LeaveRoomJob(who);

			//	um.SetJobFromRoom(who, where); // set it to be Outside the 'bot doing nothing /////////////////////////////////
			unitManager.TravelTo(who, who.transform.position, false, false);
			r.Workers.Remove(who);
			r.WorkMultiplier = r.Workers.Count;

			if (r.Type == "shrine")
			{
				roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + shrineCapacity.ToString();
				Worship();
			}
			else if (r.Type == "study")
			{
				roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + studyCapacity.ToString();
				Research();
			}
			else if (r.Type == "refinery")
			{
				roomComponents[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + refineryCapacity.ToString();
				Produce();
			}
			//method does not exist yet
			//info.UpdateUnitViewer();
		}
	}

	void SetupRefinery(int slot)
	{
		GameObject scrollerContent;

		roomComponents[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);

		roomComponents[slot].GetComponent<MiniTabHolder>().roomName.text = "Refinery";

		roomComponents[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);

		Debug.Log(rooms[slot].Type);
		roomComponents[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("refinery", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("refinery", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		roomComponents[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < refineryEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(refineryEntries[i], scrollerContent.transform);
			Instantiate(refineryCosts[i], scrollerContent.transform);

			Button craftOne = Instantiate(refineryButtons[0], scrollerContent.transform);
			craftOne.onClick.RemoveAllListeners();
			craftOne.onClick.AddListener(delegate { Refine(refineryEntries[i2].text.ToString(), 1); });
			Debug.Log(refineryEntries[i].text);

			Button craftFive = Instantiate(refineryButtons[1], scrollerContent.transform);
			craftFive.onClick.RemoveAllListeners();
			craftFive.onClick.AddListener(delegate { Refine(refineryEntries[i2].text.ToString(), 5); });
		}

		if (slot <= 3)
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[0];
		}
		else
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[0];
		}
		Produce();
	}

	void SetupStorage(int slot)
	{
		roomComponents[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		roomComponents[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		roomComponents[slot].GetComponent<MiniTabHolder>().roomName.text = "Storage";
		
		//
		GameObject scrollerContent;

		roomComponents[slot].GetComponent<MiniTabHolder>().capacity.text = "0/0";

		roomComponents[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < storageEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(storageEntries[i], scrollerContent.transform);
			Instantiate(storageDesc[i], scrollerContent.transform);

			Button discardOne = Instantiate(storageButtons[0], scrollerContent.transform);
			discardOne.onClick.RemoveAllListeners();
			discardOne.onClick.AddListener(delegate { Discard(storageEntries[i2].text.ToString(), 1); });

			Button discardFive = Instantiate(storageButtons[1], scrollerContent.transform);
			discardFive.onClick.RemoveAllListeners();
			discardFive.onClick.AddListener(delegate { Discard(storageEntries[i2].text.ToString(), 5); });
		}

		if (slot <= 3)
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[1];
		}
		else
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[1];
		}
	}

	void SetupShrine(int slot)
	{
		roomComponents[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		roomComponents[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		roomComponents[slot].GetComponent<MiniTabHolder>().roomName.text = "Shrine";

		//
		GameObject scrollerContent;

		roomComponents[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("shrine", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("shrine", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		roomComponents[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 1;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < shrineEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(shrineEffectButtons[i], scrollerContent.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { SetActiveEffect(shrineEffectKeys[i2], rooms[slot]); });
			buffer.GetComponentInChildren<Text>().text = shrineEffectDescs[i2];
		}

		if (slot <= 3)
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[2];
		}
		else
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[2];
		}

		rooms[slot].ActiveEffect = "none";
		Worship();
	}

	void SetupStudy(int slot)
	{
		roomComponents[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		roomComponents[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		roomComponents[slot].GetComponent<MiniTabHolder>().roomName.text = "Study";

		//
		GameObject scrollerContent;

		roomComponents[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("study", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("study", rooms[slot]); });
		roomComponents[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		roomComponents[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		roomComponents[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 1;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < studyEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(studyEffectButtons[i], scrollerContent.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { SetActiveEffect(studyEffectKeys[i2], rooms[slot]); });
			buffer.GetComponentInChildren<Text>().text = studyEffectDescs[i2];
		}

		if (slot <= 3)
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[3];
		}
		else
		{
			roomComponents[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[3];
		}

		rooms[slot].ActiveEffect = "none";
		Research();
	}

	public void Refine(string what, int howMany)
	{
		int eff = 0;
		eff = Random.Range(1, 11);

		Debug.Log("ran");
		Debug.Log(what);

		for (int i = 0; i < howMany; i++)
		{
			if (EffectConnector.efficiency > 0)
			{
				if (what == "plate")
				{
					if (ResourceHandling.metal >= 3)
					{
						ResourceHandling.plate++;
						ResourceHandling.metal -= 3;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.metal++;
							}
							else
							{
								ResourceHandling.metal += 2;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
                    }
				}
				else if (what == "bolt")
				{
					if (ResourceHandling.metal >= 1)
					{
						ResourceHandling.bolt++;
						ResourceHandling.metal--;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.metal++;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
                    }
				}
				else if (what == "part")
				{
					if (ResourceHandling.plate >= 2 && ResourceHandling.bolt >= 2)
					{
						ResourceHandling.part++;
						ResourceHandling.plate -= 2;
						ResourceHandling.bolt -= 2;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.bolt += 2;
							}
							else
							{
								ResourceHandling.plate++;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
                    }
				}
				else if (what == "chip")
				{
					if (ResourceHandling.electronics >= 3)
					{
						ResourceHandling.chip++;
						ResourceHandling.electronics -= 3;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.electronics++;
							}
							else
							{
								ResourceHandling.electronics += 2;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
                    }
				}
				else if (what == "wire")
				{
					if (ResourceHandling.electronics >= 1)
					{
						ResourceHandling.wire++;
						ResourceHandling.electronics--;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.electronics++;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
                    }
				}
				else if (what == "board")
				{
					if (ResourceHandling.chip >= 1 && ResourceHandling.wire >= 2)
					{
						ResourceHandling.board++;
						ResourceHandling.chip--;
						ResourceHandling.wire -= 2;
						Debug.Log("Success");
						if (eff <= EffectConnector.efficiency)
						{
							if (eff % 2 == 0)
							{
								ResourceHandling.wire += 2;
							}
							else
							{
								ResourceHandling.chip++;
							}
						}
                        PlayClip("hammer");
                    }
					else
					{
						Debug.Log("Failure");
                        PlayClip("error");
					}
				}
			}
			else
			{
				Debug.Log("No workers, can't refine!");
                PlayClip("error");
            }
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

	//worship and research methods only need to be called when their multiplier changes, or when a unit is assigned successfully. this will definitely save on horsepower
	public void Worship()
	{
		int combinedMultiplier = 0;
		List<string> effects = new List<string>();
		effects.Clear();

		foreach (Room r in rooms)
		{
			if (r.Type == "shrine")
			{
				r.WorkMultiplier = r.Workers.Count;
				Debug.Log("Shrine[" + r.Slot + "]: " + r.WorkMultiplier + "x boost");
				combinedMultiplier += r.WorkMultiplier; //case for multiples of the same room
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
				//Debug.Log("EffectConnecter.unitBaseSpeed : " + EffectConnector.unitBaseSpeed + " , " + combinedMultiplier);
			}
			else if (e == "none")
			{
				EffectConnector.unitSpeed = EffectConnector.unitBaseSpeed + 0; // --------------------------------------------------------------------> forseeable issues with multiple shrines
			}

		}
		effectConnector.Recalculate();
	}

	public void Research()
	{
		int combinedMultiplier = 0;
		List<string> effects = new List<string>();
		effects.Clear();

		foreach (Room r in rooms)
		{
			if (r.Type == "study")
			{
				Debug.Log("Study[" + r.Slot + "]: " + r.WorkMultiplier + "x boost");
				combinedMultiplier += r.WorkMultiplier; //case for multiples of the same room
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
			if (r.Type == "refinery" && r.Workers.Count > 0)
			{
				r.CanRefine = true;
				Debug.Log("Refinery[" + r.Slot + "]: Running");
				EffectConnector.efficiency += r.Workers.Count;
			}
		}
		effectConnector.Recalculate();
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	public void OpenRoom(int clickedSlot)
	{
		roomComponents[clickedSlot].gameObject.SetActive(true);
		Debug.Log("Current tab " + clickedSlot);
	}
	public void OpenController()
	{
		controllerComponents.gameObject.SetActive(true);
	}
	public void OpenGenerator()
	{
		generatorComponents.gameObject.SetActive(true);
	}
	public void TakeToBuild(int clickedSlot)
	{
		roomSlotClicked = clickedSlot;
		auto.OpenTab3();
	}

	public void RepairGenerator()
	{
		if (ResourceHandling.metal >= 100)
		{
			ResourceHandling.metal -= 100;
			generatorRepaired = true;
			generatorComponents.GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
			generatorComponents.GetComponent<MiniTabHolder>().pic.sprite = generatorRepairedSprite;
		}
	}
	public void RepairController()
	{
		if (ResourceHandling.electronics >= 100)
		{
			ResourceHandling.electronics -= 100;
			controllerRepaired = true;
			controllerComponents.GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
			controllerComponents.GetComponent<MiniTabHolder>().pic.sprite = controllerRepairedSprite;
		}
	}

	public void RepairAutomaton()
	{
		isAutomatonRepaired = true;
		Debug.Log("repaired automaton");

		//activates automoton movement script
		StartPhaseTwo.PhaseTwo();
	}
	public void ActivateAutomoton()
	{
		auto.activationButton.gameObject.SetActive(false);
		spawnRes.OpenMapRange();
		autoObj.GetComponent<AutomotonAction>().enabled = true;
		hunterHandler.enabled = true;
		selectItems.enabled = false;
		resourceHandling.SetNewResourceDeposits(spawnRes.GetAllResources());
	}

	public void UpdateRoomDisplay()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			display[i].text = rooms[i].Type;
			displaySprites[i].GetComponent<Image>().sprite = roomComponents[i].GetComponent<MiniTabHolder>().pic.sprite;
		}

		Debug.Log("Updated Rooms");
		auto.OpenTab2();
	}

	public void SetActiveEffect(string buff, Room r)
	{
		r.ActiveEffect = buff;
		Debug.Log("Active effect of [ " + r.Type + " " + r.Slot + " ] is now : " + r.ActiveEffect + ".");
		if (r.Type == "shrine")
		{
			Worship();
		}
		else if (r.Type == "study")
		{
			Research();
		}
	}

    void PlayClip(string str)
    {
        if (str == "wrench")
            audioSource.PlayOneShot(wrenchClip);
        else if (str == "hammer")
            audioSource.PlayOneShot(hammerClip);
        else if (str == "error")
            audioSource.PlayOneShot(errorClip);
    }

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
	}
}
