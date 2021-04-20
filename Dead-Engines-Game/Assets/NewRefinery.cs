using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRefinery : NewRoomClass
{

	public Text refinerySpecificText;

	public NewRefinery()
	{

	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "refinery";
		refinerySpecificText.text = "ref";
		Debug.Log("replaced old room");
	}

}
