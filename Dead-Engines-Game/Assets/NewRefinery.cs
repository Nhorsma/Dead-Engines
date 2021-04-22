using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRefinery : NewRoomClass
{
	private int workerCapacity;
	private List<GameObject> workers;

	public Button assignButton;
	public Button unassignButton;

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
		WorkerCapacity = 3;
		Workers = new List<GameObject>();
	}

	public NewRefinery()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "refinery";
		nameText.text = "ref";
		Debug.Log("replaced old room");
		TurnOnButtons();
		AddButtonEvents();
	}

	public void TurnOnButtons()
	{
		assignButton.enabled = true;
		unassignButton.enabled = true;
	}

	public void AddButtonEvents()
	{
		assignButton.onClick.AddListener(delegate { roomManager.Assign(this.Type, this.Slot); });
		unassignButton.onClick.AddListener(delegate { roomManager.Unassign(this.Type, this.Slot); });
	}

}
