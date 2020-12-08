using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomManager : MonoBehaviour
{
	public SpawnRes spawnRes;

	public List<Room> rooms = new List<Room>();
	public List<Text> display; //change to gameObject later
	public List<GameObject> displaySprites;

	public int roomSlotClicked = 0;
	public AutomatonUI auto;
    public UnitManager um;
	public GameObject autoObj;

	public List<GameObject> miniTabs = new List<GameObject>();
	public GameObject ctrlMiniTab;
	public GameObject genMiniTab;

    public AudioSource audioSource;
    public AudioClip wrenchClip;
    public AudioClip hammerClip;
    public AudioClip errorClip;

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

	public bool isAutomatonRepaired = false;

	public EffectConnector effectConnector;

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

	public List<Sprite> rightRoomSprites; // 4, 5, 6
	public List<Sprite> leftRoomSprites; // 0, 1, 2, 3

	public int refineryCapacity;
	public int shrineCapacity;
	public int studyCapacity;
	//add generator 1 & control 1 later

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

		UpdateRoomDisplay();
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

	public void RepairAutomaton()
	{
		isAutomatonRepaired = true;
		Debug.Log("repaired automaton");

        //activates automoton movement script
        StartPhaseTwo.PhaseTwo();
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

	void SetupRefinery(int slot) //////////////////////////////		miniTabs[slot].GetComponent<MiniTabHolder>().func00.GetComponentInChildren<Text>().text = "Unassign Unit";
	{
		GameObject con;

		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);

		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Refinery";

		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);

		Debug.Log(rooms[slot].Type);
		miniTabs[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("refinery", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("refinery", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		miniTabs[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		con = miniTabs[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < refineryEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(refineryEntries[i], con.transform);
			Instantiate(refineryCosts[i], con.transform);

			Button craftOne = Instantiate(refineryButtons[0], con.transform);
			craftOne.onClick.RemoveAllListeners();
			craftOne.onClick.AddListener(delegate { Refine(refineryEntries[i2].text.ToString(), 1); });
			Debug.Log(refineryEntries[i].text);

			Button craftFive = Instantiate(refineryButtons[1], con.transform);
			craftFive.onClick.RemoveAllListeners();
			craftFive.onClick.AddListener(delegate { Refine(refineryEntries[i2].text.ToString(), 5); });
		}

		if (slot <= 3)
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[0];
		}
		else
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[0];
		}

		Produce();
	}

	void SetupStorage(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Storage";
		
		//
		GameObject con;

		miniTabs[slot].GetComponent<MiniTabHolder>().capacity.text = "0/0";

		miniTabs[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		con = miniTabs[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < storageEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(storageEntries[i], con.transform);
			Instantiate(storageDesc[i], con.transform);

			Button discardOne = Instantiate(storageButtons[0], con.transform);
			discardOne.onClick.RemoveAllListeners();
			discardOne.onClick.AddListener(delegate { Discard(storageEntries[i2].text.ToString(), 1); });

			Button discardFive = Instantiate(storageButtons[1], con.transform);
			discardFive.onClick.RemoveAllListeners();
			discardFive.onClick.AddListener(delegate { Discard(storageEntries[i2].text.ToString(), 5); });
		}

		if (slot <= 3)
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[1];
		}
		else
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[1];
		}
	}

	void SetupShrine(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Shrine";

		//
		GameObject con;

		miniTabs[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("shrine", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("shrine", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		miniTabs[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		con = miniTabs[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;
		con.GetComponent<GridLayoutGroup>().constraintCount = 1;
		con.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < shrineEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(shrineEffectButtons[i], con.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { SetActiveEffect(shrineEffectKeys[i2], rooms[slot]); });
			buffer.GetComponentInChildren<Text>().text = shrineEffectDescs[i2];
		}

		if (slot <= 3)
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[2];
		}
		else
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[2];
		}

		rooms[slot].ActiveEffect = "none";
		Worship();
	}

	void SetupStudy(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Study";

		//
		GameObject con;

		miniTabs[slot].GetComponent<MiniTabHolder>().assign.onClick.AddListener(delegate { Assign("study", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().assign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.onClick.AddListener(delegate { Unassign("study", rooms[slot]); });
		miniTabs[slot].GetComponent<MiniTabHolder>().unassign.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().capacity.text = "0/3";

		miniTabs[slot].GetComponent<MiniTabHolder>().scroller.gameObject.SetActive(true);

		con = miniTabs[slot].GetComponent<MiniTabHolder>().scroller.GetComponent<ScrollRect>().content.gameObject;
		con.GetComponent<GridLayoutGroup>().constraintCount = 1;
		con.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < studyEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(studyEffectButtons[i], con.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { SetActiveEffect(studyEffectKeys[i2], rooms[slot]); });
			buffer.GetComponentInChildren<Text>().text = studyEffectDescs[i2];
		}

		if (slot <= 3)
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = leftRoomSprites[3];
		}
		else
		{
			miniTabs[slot].GetComponent<MiniTabHolder>().pic.sprite = rightRoomSprites[3];
		}

		rooms[slot].ActiveEffect = "none";
		Research();
	}

	//this is a debug function for testing only!
	public void Sup()
	{
		Debug.Log("Sup");
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

			if (r.Type == "shrine")
			{
				if (r.Workers.Count >= shrineCapacity)
				{
					Debug.Log("Room is full");
					return;
				}
				else
				{
					who.Job = where;
					who.JobPos = autoObj;
					um.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + shrineCapacity.ToString();

					Worship();
					Debug.Log("Assigned [" + who.UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
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
					who.Job = where;
					who.JobPos = autoObj;
					um.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + studyCapacity.ToString();

					Research();
					Debug.Log("Assigned [" + who.UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
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
					who.Job = where;
					who.JobPos = autoObj;
					um.SetJobFromRoom(who, where);
					r.Workers.Add(who);
					r.WorkMultiplier = r.Workers.Count;
					miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + refineryCapacity.ToString();

					Produce();
					Debug.Log("Assigned [" + who.UnitName + "] to[" + r.Type + "][" + r.Slot + "]");
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
			Unit who = r.Workers[0]; //first unit
			Debug.Log("Unassigned [" + who.UnitName + "] from [" + r.Type + "][" + r.Slot + "]");
			who.Job = "none";

			//who.JobPos = autoObj; // set it to outside ////////////////////////////////////////////////////////////////////
            um.LeaveRoomJob(who);

            //	um.SetJobFromRoom(who, where); // set it to be Outside the 'bot doing nothing /////////////////////////////////
            um.TravelTo(um.GetUnitObject(who), um.GetUnitObject(who).transform.position, false, false);
			r.Workers.Remove(who);
			r.WorkMultiplier = r.Workers.Count;

			if (r.Type == "shrine")
			{
				miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + shrineCapacity.ToString();
				Worship();
			}
			else if (r.Type == "study")
			{
				miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + studyCapacity.ToString();
				Research();
			}
			else if (r.Type == "refinery")
			{
				miniTabs[r.Slot].GetComponent<MiniTabHolder>().capacity.text = r.Workers.Count.ToString() + " / " + refineryCapacity.ToString();
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

    void PlayClip(string str)
    {
        if (str == "wrench")
            audioSource.PlayOneShot(wrenchClip);
        else if (str == "hammer")
            audioSource.PlayOneShot(hammerClip);
        else if (str == "error")
            audioSource.PlayOneShot(errorClip);
    }

	public void UpdateRoomDisplay()
	{
		for (int i = 0; i < rooms.Count; i++)
		{
			display[i].text = rooms[i].Type;
			displaySprites[i].GetComponent<Image>().sprite = miniTabs[i].GetComponent<MiniTabHolder>().pic.sprite;
		}
		Debug.Log("Updated Rooms");
		auto.OpenTab2();
	}

    public void ActivateAutomoton()
    {
		spawnRes.OpenMapRange();
    }
}
