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

	public Button craft_bolt;
	public Button craft_plate;
	public Button craft_part;
	public Button craft_wire;
	public Button craft_chip;
	public Button craft_board;

	public Text efficiency_text;

	public RoomManager roomManager;

	public void Start()
	{
		roomManager = FindObjectOfType<RoomManager>();
		WorkerCapacity = 3;
		Workers = new List<GameObject>();
	}

	public void Update()
	{
		efficiency_text.text = "efficiency rate: " + roomManager.efficiency + "x";
		capacityText.text = Workers.Count + " / " + WorkerCapacity;
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
		this.Health = 50;
		this.Defense = 10;
		this.Other = "none";
		Debug.Log("replaced old room");
		AddButtonEvents();
	}

	public void AddButtonEvents()
	{
		assignButton.onClick.AddListener(delegate { roomManager.Assign(this.Type, this.Slot); });
		unassignButton.onClick.AddListener(delegate { roomManager.Unassign(this.Type, this.Slot); });

		craft_bolt.onClick.AddListener(delegate { roomManager.Refine("bolt", 1); });
		craft_plate.onClick.AddListener(delegate { roomManager.Refine("plate", 1); });
		craft_part.onClick.AddListener(delegate { roomManager.Refine("part", 1); });
		craft_wire.onClick.AddListener(delegate { roomManager.Refine("wire", 1); });
		craft_chip.onClick.AddListener(delegate { roomManager.Refine("chip", 1); });
		craft_board.onClick.AddListener(delegate { roomManager.Refine("board", 1); });
	}

}
