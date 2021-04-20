using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabCreation : MonoBehaviour
{

	public NewRoomClass newRoom;
	public NewRefinery newRef;
	public static List<NewRoomClass> rooms = new List<NewRoomClass>();

	public void Update()
	{
		if (Input.GetKeyDown(KeyCode.T))
		{
			CreateEmptyTab();
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			Replace(0);
			foreach (NewRoomClass r in rooms)
			{
				if (r.Slot == 0)
				{
					Debug.Log(r.Type);
				}
			}
		}
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
		}
		Debug.Log("Done");
	}

	public void Replace(int slot)
	{
		var replacement = Instantiate(newRef, this.transform);
		replacement.ReplaceOldRoom(slot);
		for (int i = 0; i < rooms.Count; i++)
		{
			if (rooms[i].Slot == slot)
			{
				var temp = rooms[i];
				rooms.Remove(temp);
				Destroy(temp.gameObject);
			}
		}
		rooms.Add(replacement);
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
