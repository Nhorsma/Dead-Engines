using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabCreation : MonoBehaviour
{

	public NewRoomClass newRoom;
	public NewRefinery refinery;
	public NewStorageClass storage;
	public NewShrineClass shrine;
	public NewStudyClass study;
	public NewInfirmaryClass infirmary;
	public NewDormitoryClass dormitory;
	public NewBarracksClass barracks;
	public NewCloneryClass clonery;
	public static List<NewRoomClass> rooms = new List<NewRoomClass>();

	public RoomManager roomManager;

	public void Start()
	{
		CreateEmptyTab();
		foreach (NewRoomClass r in rooms)
		{
			r.roomTab.SetActive(false);
		}
	}

	public void Update()
	{
		//if (Input.GetKeyDown(KeyCode.T))
		//{
		//	CreateEmptyTab();
		//}
		//if (Input.GetKeyDown(KeyCode.R))
		//{
		//	Replace(0, "refinery");
		//	Debug.Log(FindSlot(0).Type);
		//}
	}

	public void CreateEmptyTab()
	{
		for (int i = 0; i < 17; i++)
		{
			var r = Instantiate(newRoom, this.transform);
			r.Slot = i;
			r.Type = "empty";
			r.Level = 1;
			rooms.Add(r);
			r.buildButton.onClick.AddListener(delegate { roomManager.TakeToBuild(r.Slot); });
		}
		Debug.Log("Done");
	}

	public void Replace(int slot, string type)
	{
		switch (type)
		{
			case "refinery":
				var rep1 = Instantiate(refinery, this.transform);
				rep1.ReplaceOldRoom(slot);
				rooms.Add(rep1);
				break;
			case "storage":
				var rep2 = Instantiate(storage, this.transform);
				rep2.ReplaceOldRoom(slot);
				rooms.Add(rep2);
				break;
			case "shrine":
				var rep3 = Instantiate(shrine, this.transform);
				rep3.ReplaceOldRoom(slot);
				rooms.Add(rep3);
				break;
			case "study":
				var rep4 = Instantiate(study, this.transform);
				rep4.ReplaceOldRoom(slot);
				rooms.Add(rep4);
				break;
			case "infirmary":
				var rep5 = Instantiate(infirmary, this.transform);
				rep5.ReplaceOldRoom(slot);
				rooms.Add(rep5);
				break;
			case "dormitory":
				var rep6 = Instantiate(dormitory, this.transform);
				rep6.ReplaceOldRoom(slot);
				rooms.Add(rep6);
				break;
			case "barracks":
				var rep7 = Instantiate(barracks, this.transform);
				rep7.ReplaceOldRoom(slot);
				rooms.Add(rep7);
				break;
			case "clonery":
				var rep8 = Instantiate(clonery, this.transform);
				rep8.ReplaceOldRoom(slot);
				rooms.Add(rep8);
				break;
			default:
				break;
		}

		var temp = FindSlot(slot);
		rooms.Remove(temp);
		Destroy(temp.gameObject);
	}

	public static NewRoomClass FindSlot(int slot)
	{
		foreach (NewRoomClass r in rooms)
		{
			if (r.Slot == slot)
			{
				return r;
			}
		}
		return null;
	}

}
