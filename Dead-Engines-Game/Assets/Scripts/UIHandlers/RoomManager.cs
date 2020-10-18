using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{

	public List<Room> rooms = new List<Room>();
	public List<Text> display; //change to gameObject later

	public int roomSlotClicked = 0;
	public AutomatonUI auto;
    public UnitManager um;
	public GameObject autoObj;

	public List<GameObject> miniTabs = new List<GameObject>();
	public GameObject ctrlMiniTab;
	public GameObject genMiniTab;

	public float refineryCost_M;
	public float refineryCost_E;

	public float storageCost_M;
	public float storageCost_E;

	public float shrineCost_M;
	public float shrineCost_E;

	public float studyCost_M;
	public float studyCost_E;

	//origs
	public float refineryCost_Mo;
	public float refineryCost_Eo;

	public float storageCost_Mo;
	public float storageCost_Eo;

	public float shrineCost_Mo;
	public float shrineCost_Eo;

	public float studyCost_Mo;
	public float studyCost_Eo;

	public int efficiency = 0;

	public EffectConnector effectConnector;

	void Start()
    {
		for (int i = 0; i < 7; i++)
		{
			rooms.Add(new Room("empty", i, 0));
		}
		refineryCost_Mo = refineryCost_M;
		refineryCost_Eo = refineryCost_E;

		storageCost_Mo = storageCost_M;
		storageCost_Eo = storageCost_E;

		shrineCost_Mo = shrineCost_M;
		shrineCost_Eo = shrineCost_E;

		studyCost_Mo = studyCost_M;
		studyCost_Eo = studyCost_E;
}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void OpenMiniTab(int clickedSlot)
	{
		miniTabs[clickedSlot].gameObject.SetActive(true);
		Debug.Log("Current tab " + clickedSlot);
	}

	public void OpenController()
	{
		ctrlMiniTab.gameObject.SetActive(true);
	}

	public void OpenGenerator()
	{
		genMiniTab.gameObject.SetActive(true);
	}

	public void TakeToBuild(int clickedSlot)
	{
		roomSlotClicked = clickedSlot;
		auto.OpenTab3();
	}

	public void Build(string room)
	{
		if (room == "refinery" && ResourceHandling.metal >= refineryCost_M && ResourceHandling.electronics >= refineryCost_E)
		{
			ResourceHandling.metal -= (int)refineryCost_M;
			ResourceHandling.electronics -= (int)refineryCost_E;
			rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1);
			SetupRefinery(roomSlotClicked);
		}
		else if (room == "storage" && ResourceHandling.metal >= storageCost_M && ResourceHandling.electronics >= refineryCost_E)
		{
			ResourceHandling.metal -= (int)storageCost_M;
			ResourceHandling.electronics -= (int)storageCost_E;
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
			SetupStorage(roomSlotClicked);
		}
		else if (room == "shrine" && ResourceHandling.metal >= shrineCost_M && ResourceHandling.electronics >= shrineCost_E)
		{
			ResourceHandling.metal -= (int)shrineCost_M;
			ResourceHandling.electronics -= (int)shrineCost_E;
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
			SetupShrine(roomSlotClicked);
		}
		else if (room == "study" && ResourceHandling.metal >= studyCost_M && ResourceHandling.electronics >= studyCost_E)
		{
			ResourceHandling.metal -= (int)studyCost_M;
			ResourceHandling.electronics -= (int)studyCost_E;
			rooms[roomSlotClicked] = new Room("study", roomSlotClicked, 1);
			SetupStudy(roomSlotClicked);
		}
		else
		{
			Debug.Log("Not enough resources to build a " + room + ".");
		}
		UpdateRoomDisplay();
	}

	void SetupRefinery(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);

		miniTabs[slot].GetComponent<MiniTabHolder>().func0.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func0.GetComponentInChildren<Text>().text = "Assign Unit";
		miniTabs[slot].GetComponent<MiniTabHolder>().func0.onClick.AddListener(delegate { Assign("refinery", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().func0.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Refinery";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Refine Bolt";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Refine Plate";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Refine Part";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(delegate{ Refine("bolt"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(delegate { Refine("plate"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(delegate { Refine("part"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func4.GetComponentInChildren<Text>().text = "Refine Wire";
		miniTabs[slot].GetComponent<MiniTabHolder>().func5.GetComponentInChildren<Text>().text = "Refine Chip";
		miniTabs[slot].GetComponent<MiniTabHolder>().func6.GetComponentInChildren<Text>().text = "Refine Board";

		miniTabs[slot].GetComponent<MiniTabHolder>().func4.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func4.onClick.AddListener(delegate { Refine("wire"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func4.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func5.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func5.onClick.AddListener(delegate { Refine("chip"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func5.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func6.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func6.onClick.AddListener(delegate { Refine("board"); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func6.gameObject.SetActive(true);
	}

	void SetupStorage(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Storage";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Sup";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Discard 1 Electronics";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Discard 1 Metal";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);


		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(delegate { Discard("electronics", 1); }); ///////////////////////////////

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(delegate { Discard("electronics", 1); }); ///////////////////////////////

	}

	void SetupShrine(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Shrine";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Assign Unit";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Buff1";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Buff2";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(delegate { Assign("shrine", rooms[slot]); } ); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.gameObject.SetActive(true);
	}

	void SetupStudy(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Study";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Assign Unit";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Buff1";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Buff2";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(delegate { Assign("study", rooms[slot]); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.gameObject.SetActive(true);
	}

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
	}

	public void Refine(string what)
	{
		int eff = 0;
		eff = Random.Range(1, 11);

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
				}
				else
				{
					Debug.Log("Failure");
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
				}
				else
				{
					Debug.Log("Failure");
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
				}
				else
				{
					Debug.Log("Failure");
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
				}
				else
				{
					Debug.Log("Failure");
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
				}
				else
				{
					Debug.Log("Failure");
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
				}
				else
				{
					Debug.Log("Failure");
				}
			}
		}
		else
		{
			Debug.Log("No workers, can't refine!");
		}
		
	}

	//needs to be overhauled
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

	public void Assign(string where, Room r)
	{
		
		if (um.ReturnJoblessUnit() == null)
		{
			Debug.Log("No worker available");
			return;
		}
		else
		{
			Unit who = um.ReturnJoblessUnit();
			Debug.Log("Assigned [" + who.UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
			who.Job = where;
			who.JobPos = autoObj;
			um.SetJobFromRoom(who, where);
			r.Workers.Add(who);
			r.WorkMultiplier = r.Workers.Count;

			if (r.Type == "shrine")
			{
				r.ActiveEffect = "unitSpeed";
				Worship();
			}
			else if (r.Type == "study")
			{
				r.ActiveEffect = "roomCost";
				Research();
			}
			else if (r.Type == "refinery")
			{
				Produce();
			}

			//method does not exist yet
			//info.UpdateUnitViewer();
		}
	}

	//worship and research methods only need to be called when their multiplier changes, or when a unit is assigned successfully
	//this will definitely save on horsepower
	public void Worship()
	{
		int combinedMultiplier = 0;
		List<string> effects = new List<string>();
		effects.Clear();

		foreach (Room r in rooms)
		{
			if (r.Type == "shrine" && r.Workers.Count > 0)
			{
				Debug.Log("Shrine[" + r.Slot + "]: " + r.WorkMultiplier + "x boost");
				combinedMultiplier += r.WorkMultiplier; //case for multiples of the same room
				effects.Add(r.ActiveEffect);
			}
		}
		Debug.Log("Total Shrine Multiplier: " + combinedMultiplier);

		//calculate effects
		foreach (string e in effects)
		{
			if (e == "none")
			{
				return;
			}
			else if (e == "unitSpeed")	//-----------------------> r.ActiveEffect will be set by a button that is activated once room is upgraded
			{
				EffectConnector.unitSpeed = EffectConnector.unitBaseSpeed + combinedMultiplier; ////////////////////////////////////////////////////////////
				//Debug.Log("Unit Speed: " + EffectConnector.unitSpeed);
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
			if (r.Type == "study" && r.Workers.Count > 0)
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

			if (e == "none")
			{
				return;
			}
			else if (e == "roomCost")    //-----------------------> r.ActiveEffect will be set by a button that is activated once room is upgraded
			{
				Debug.Log("hit?");
				EffectConnector.roomCost = combinedMultiplier + 1;
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

	public void UpdateRoomDisplay()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			display[i].text = rooms[i].Type;
		}
		Debug.Log("Updated Rooms");
		auto.OpenTab2();
	}
}
