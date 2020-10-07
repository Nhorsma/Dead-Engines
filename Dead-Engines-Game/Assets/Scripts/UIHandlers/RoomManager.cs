﻿using System.Collections;
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

	public int refineryCost_M;
	public int refineryCost_E;

	public int storageCost_M;
	public int storageCost_E;

	public int shrineCost_M;
	public int shrineCost_E;

	public int studyCost_M;
	public int studyCost_E;

	public UnitManager unitManager;

	void Start()
    {
		for (int i = 0; i < 7; i++)
		{
			rooms.Add(new Room("empty", i, 0));
		}
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
			ResourceHandling.metal -= refineryCost_M;
			ResourceHandling.electronics -= refineryCost_E;
			rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1);
			SetupRefinery(roomSlotClicked);
		}
		else if (room == "storage" && ResourceHandling.metal >= storageCost_M && ResourceHandling.electronics >= refineryCost_E)
		{
			ResourceHandling.metal -= storageCost_M;
			ResourceHandling.electronics -= storageCost_E;
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
			SetupStorage(roomSlotClicked);
		}
		else if (room == "shrine" && ResourceHandling.metal >= shrineCost_M && ResourceHandling.electronics >= shrineCost_E)
		{
			ResourceHandling.metal -= shrineCost_M;
			ResourceHandling.electronics -= shrineCost_E;
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
			SetupShrine(roomSlotClicked);
		}
		else if (room == "study" && ResourceHandling.metal >= studyCost_M && ResourceHandling.electronics >= studyCost_E)
		{
			ResourceHandling.metal -= studyCost_M;
			ResourceHandling.electronics -= studyCost_E;
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

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Discard 1 Metal";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Discard 1 Electronics";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Discard Other";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(delegate { Discard("metal", 1); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.onClick.AddListener(delegate { Discard("electronics", 1); }); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.gameObject.SetActive(true);

		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.onClick.AddListener(Sup); ///////////////////////////////
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.gameObject.SetActive(true);
	}

	void SetupShrine(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Shrine";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Assign Unit";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Worship That";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Worship Other";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(delegate { Assign("shrine", rooms[slot]); }); ///////////////////////////////
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

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Study This";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Study That";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Study Other";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.RemoveAllListeners();
		miniTabs[slot].GetComponent<MiniTabHolder>().func1.onClick.AddListener(Sup); ///////////////////////////////
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
		if (what == "plate")
		{
			if (ResourceHandling.metal >= 3)
			{
				ResourceHandling.plate++;
				ResourceHandling.metal -= 3;
				Debug.Log("Success");
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
			}
			else
			{
				Debug.Log("Failure");
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
		Debug.Log("Fix me! [Assign] to [" + r.Type + "] [" + r.Slot + "]");

        Unit who = um.ReturnJoblessUnit();
        if(who==null)
        {
            Debug.Log("no worker available");
            return;
        }
        who.Job = where;
        who.JobPos = autoObj;
        um.SetJobFromRoom(who, where);
        //r.workers.Add(who);
        //r.work_multiplier = r.workers.Count;

        // method does not exist yet
        //info.UpdateUnitViewer();
    }

    public void Worship()
	{

	}

	public void Study()
	{
        
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
