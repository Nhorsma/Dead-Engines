using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewBarracksClass : NewRoomClass
{
	public List<GameObject> contentToClear = new List<GameObject>();

	public GameObject retrain_prefab;
	public GameObject id_prefab;
	public GameObject type_prefab;

	public RoomManager roomManager;
	public UnitManager unitManager;

	public GameObject scrollContent;

	public string setType = "standard";
	public Text indicator;

	public void Update()
	{
		
	}

	public NewBarracksClass()
	{

	}

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "barracks";
		this.Health = 0;
		this.Defense = 15;
		this.Other = "handFeetBoost";
		Debug.Log("replaced old room");
		roomManager = FindObjectOfType<RoomManager>();
		unitManager = FindObjectOfType<UnitManager>();
		UpdateUnitInfo();
	}

	public void UpdateUnitInfo()
	{

		foreach (Transform child in scrollContent.transform) // help from someone else on the unity forum!
		{
			contentToClear.Add(child.gameObject);
		}

		for (int i = 0; i < contentToClear.Count; i++)
		{
			Destroy(contentToClear[i]);
		}

		//Debug.Log(roomManager.efficiency);
		//Debug.Log(unitManager.units.Count);

		foreach (GameObject u in unitManager.units)
		{
			var id = Instantiate(id_prefab, scrollContent.transform);
			id.GetComponent<Text>().text = u.GetComponent<Unit>().Id.ToString();
			var type = Instantiate(type_prefab, scrollContent.transform);
			type.GetComponent<Text>().text = u.GetComponent<Unit>().Type.ToString();
			var retrain = Instantiate(retrain_prefab, scrollContent.transform);
			retrain.GetComponent<Button>().onClick.AddListener(delegate { roomManager.UseBarracks(u.GetComponent<Unit>(), setType, this.Slot); });
		}
	}

	public void SetType(string type)
	{
		if (type == "standard")
		{
			setType = "standard";
			indicator.text = "current: standard";
		}
		else if (type == "defensive")
		{
			setType = "defensive";
			indicator.text = "current: defensive";
		}
		else if (type == "ambusher")
		{
			setType = "ambusher";
			indicator.text = "current: ambusher";
		}
	}

}
