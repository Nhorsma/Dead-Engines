using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewStudyClass : NewRoomClass
{

	private int workerCapacity;
	private List<GameObject> workers;
	private string activeEffect;

	public Button assignButton;
	public Button unassignButton;
	public Button defaultEffect;
	public Button roomCostEffect;

	public Text activeText;
	public Text multiplierText;

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
		WorkerCapacity = 3;
		Workers = new List<GameObject>();
		ActiveEffect = "none";
	}

	public void Update()
	{
		capacityText.text = Workers.Count + " / " + WorkerCapacity;
		activeText.text = "active effect: " + ActiveEffect;
		multiplierText.text = "research multiplier: " + roomManager.combinedStudyMultiplier + "x";
	}

	public NewStudyClass()
	{

	}

	public int WorkerCapacity { get => workerCapacity; set => workerCapacity = value; }
	public List<GameObject> Workers { get => workers; set => workers = value; }
	public string ActiveEffect { get => activeEffect; set => activeEffect = value; }

	public void ReplaceOldRoom(int oldSlot)
	{
		//delete from the collection???
		this.Slot = oldSlot;
		this.Type = "study";
		Debug.Log("replaced old room");
		AddButtonEvents();
	}

	public void AddButtonEvents()
	{
		assignButton.onClick.AddListener(delegate { roomManager.Assign(this.Type, this.Slot); });
		unassignButton.onClick.AddListener(delegate { roomManager.Unassign(this.Type, this.Slot); });

		defaultEffect.onClick.AddListener(delegate { roomManager.SetActiveEffect("none", this.Slot); });
		roomCostEffect.onClick.AddListener(delegate { roomManager.SetActiveEffect("roomCost", this.Slot); });
	}

}
