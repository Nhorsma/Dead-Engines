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

	public List<GameObject> miniTabs = new List<GameObject>();
	public GameObject ctrlMiniTab;
	public GameObject genMiniTab;

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
		if (room == "refinery")
		{
			rooms[roomSlotClicked] = new Room("refinery", roomSlotClicked, 1);
			SetupRefinery(roomSlotClicked);
		}
		else if (room == "storage")
		{
			rooms[roomSlotClicked] = new Room("storage", roomSlotClicked, 1);
			SetupStorage(roomSlotClicked);
		}
		if (room == "shrine")
		{
			rooms[roomSlotClicked] = new Room("shrine", roomSlotClicked, 1);
			SetupShrine(roomSlotClicked);
		}
		if (room == "study")
		{
			rooms[roomSlotClicked] = new Room("study", roomSlotClicked, 1);
			SetupStudy(roomSlotClicked);
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
	}

	void SetupStorage(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Storage";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Discard This";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Discard That";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Discard Other";

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

	void SetupShrine(int slot)
	{
		miniTabs[slot].GetComponent<MiniTabHolder>().build.gameObject.SetActive(false);
		miniTabs[slot].GetComponent<MiniTabHolder>().upgrade.gameObject.SetActive(true);
		miniTabs[slot].GetComponent<MiniTabHolder>().roomName.text = "Shrine";

		miniTabs[slot].GetComponent<MiniTabHolder>().func1.GetComponentInChildren<Text>().text = "Worship This";
		miniTabs[slot].GetComponent<MiniTabHolder>().func2.GetComponentInChildren<Text>().text = "Worship That";
		miniTabs[slot].GetComponent<MiniTabHolder>().func3.GetComponentInChildren<Text>().text = "Worship Other";

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
