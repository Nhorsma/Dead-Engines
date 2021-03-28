using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetupRoom : MonoBehaviour
{

	public List<RoomComponents> roomComponents;
	public RoomManager roomManager;

	public List<Sprite> rightRoomSprites; // 4, 5, 6
	public List<Sprite> leftRoomSprites; // 0, 1, 2, 3

	//room-specific data used for construction
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

	public List<Text> infirmaryEntries;
	public List<Text> infirmaryDescs;

	void Start()
    {
        
    }

    void Update()
    {
        
    }

	public void Setup(Room room)
	{
		int slot = room.Slot;
		roomComponents[slot].pic.sprite = leftRoomSprites[0]; // fix later, unsure what this does, might just initialise?
		roomComponents[slot].roomName.text = room.Type;
		roomComponents[slot].build.gameObject.SetActive(false);
		roomComponents[slot].upgrade.gameObject.SetActive(true);

		if (room.Type == "storage" || room.Type == "clonery" || room.Type == "barracks" || room.Type == "dormitory")
		{
			switch (room.Type)
			{
				case "storage":
					roomComponents[slot].capacity.text = "0 / " + room.StorageCapacity.ToString();
					SetupStorage(slot);
					break;
				case "clonery":
					SetupClonery(slot);
					break;
				case "barracks":
					SetupBarracks(slot);
					break;
				case "dormitory":
					SetupDormitory(slot);
					break;
			}

		}
		else
		{
			roomComponents[slot].capacity.text = "0 / " + room.WorkerCapacity.ToString();
			roomComponents[slot].assign.onClick.AddListener(delegate { roomManager.Assign(room.Type, slot); });
			roomComponents[slot].unassign.onClick.AddListener(delegate { roomManager.Unassign(room.Type, slot); });
			roomComponents[slot].capacity.gameObject.SetActive(true);
			roomComponents[slot].assign.gameObject.SetActive(true);
			roomComponents[slot].unassign.gameObject.SetActive(true);
			switch (room.Type)
			{
				case "refinery":
					SetupRefinery(slot);
					roomManager.Produce();
					break;
				case "shrine":
					SetupShrine(slot);
					roomManager.Worship();
					break;
				case "study":
					SetupStudy(slot);
					roomManager.Research();
					break;
				case "infirmary":
					roomComponents[slot].assign.gameObject.SetActive(false);
					roomComponents[slot].unassign.gameObject.SetActive(false);
					SetupInfirmary(slot);
					break;
			}
		}
	}

	// need to clean
	void SetupRefinery(int slot)
	{
		GameObject scrollerContent;
		scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < refineryEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(refineryEntries[i], scrollerContent.transform);
			Instantiate(refineryCosts[i], scrollerContent.transform);

			Button craftOne = Instantiate(refineryButtons[0], scrollerContent.transform);
			craftOne.onClick.RemoveAllListeners();
			craftOne.onClick.AddListener(delegate { roomManager.Refine(refineryEntries[i2].text.ToString(), 1); });
			Debug.Log(refineryEntries[i].text);

			Button craftFive = Instantiate(refineryButtons[1], scrollerContent.transform);
			craftFive.onClick.RemoveAllListeners();
			craftFive.onClick.AddListener(delegate { roomManager.Refine(refineryEntries[i2].text.ToString(), 5); });
		}

		roomComponents[slot].scroller.gameObject.SetActive(true);

		LeftOrRight(slot, 0);
	}

	// need to clean
	void SetupStorage(int slot)
	{
		GameObject scrollerContent;

		scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;

		for (int i = 0; i < storageEntries.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Instantiate(storageEntries[i], scrollerContent.transform);
			Instantiate(storageDesc[i], scrollerContent.transform);

			Button discardOne = Instantiate(storageButtons[0], scrollerContent.transform);
			discardOne.onClick.RemoveAllListeners();
			discardOne.onClick.AddListener(delegate { roomManager.Discard(storageEntries[i2].text.ToString(), 1); });

			Button discardFive = Instantiate(storageButtons[1], scrollerContent.transform);
			discardFive.onClick.RemoveAllListeners();
			discardFive.onClick.AddListener(delegate { roomManager.Discard(storageEntries[i2].text.ToString(), 5); });
		}

		roomComponents[slot].scroller.gameObject.SetActive(true);

		LeftOrRight(slot, 1);
	}

	// need to clean
	void SetupShrine(int slot)
	{
		roomComponents[slot].build.gameObject.SetActive(false);
		roomComponents[slot].upgrade.gameObject.SetActive(true);
		roomComponents[slot].roomName.text = "Shrine";

		//
		GameObject scrollerContent;

		roomComponents[slot].capacity.text = "0/3";

		roomComponents[slot].scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 1;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < shrineEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(shrineEffectButtons[i], scrollerContent.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { roomManager.SetActiveEffect(shrineEffectKeys[i2], slot); });
			buffer.GetComponentInChildren<Text>().text = shrineEffectDescs[i2];
		}

		LeftOrRight(slot, 2);

		//rooms[slot].ActiveEffect = "none";
		roomManager.Worship();
	}

	// need to clean
	void SetupStudy(int slot)
	{
		roomComponents[slot].build.gameObject.SetActive(false);
		roomComponents[slot].upgrade.gameObject.SetActive(true);
		roomComponents[slot].roomName.text = "Study";

		//
		GameObject scrollerContent;

		roomComponents[slot].capacity.text = "0/3";

		roomComponents[slot].scroller.gameObject.SetActive(true);

		scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 1;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(225, 50);

		for (int i = 0; i < studyEffectButtons.Count; i++)
		{
			var i2 = i; // wow what bullshit thanks unity
			Button buffer = Instantiate(studyEffectButtons[i], scrollerContent.transform);
			buffer.onClick.RemoveAllListeners();
			buffer.onClick.AddListener(delegate { roomManager.SetActiveEffect(studyEffectKeys[i2], slot); });
			buffer.GetComponentInChildren<Text>().text = studyEffectDescs[i2];
		}

		LeftOrRight(slot, 3);

		//rooms[slot].ActiveEffect = "none";
		roomManager.Research();
	}

	public void SetupInfirmary(int slot)
	{
		GameObject scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 2;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(112.5f, 50);

		for (int i = 0; i < roomManager.rooms[slot].WorkerCapacity; i++) //3 entries : bed 0, 1, 2
		{
			Instantiate(infirmaryEntries[0], scrollerContent.transform); // bed 0-2

			Instantiate(infirmaryDescs[0], scrollerContent.transform); // bedrest() -> refreshes the room
		}

		//findSick button here

		roomComponents[slot].scroller.gameObject.SetActive(true);

		LeftOrRight(slot, 4); //change to real 4 later 
	}

	public void SetupClonery(int slot)
	{
		GameObject scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 2;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(112.5f, 50);

		roomComponents[slot].scroller.gameObject.SetActive(true);

		LeftOrRight(slot, 4); //change to 5 later 
	}

	public void SetupBarracks(int slot)
	{
		GameObject scrollerContent = roomComponents[slot].scroller.GetComponent<ScrollRect>().content.gameObject;
		scrollerContent.GetComponent<GridLayoutGroup>().constraintCount = 2;
		scrollerContent.GetComponent<GridLayoutGroup>().cellSize = new Vector2(112.5f, 50);

		roomComponents[slot].scroller.gameObject.SetActive(true);

		LeftOrRight(slot, 4); //change to 6 later 
	}

	public void SetupDormitory(int slot)
	{
		LeftOrRight(slot, 4); //change to 7 later 
	}

	public void LeftOrRight(int slot, int spriteInList)
	{
		if (slot <= 3)
		{
			Debug.Log(spriteInList);
			roomComponents[slot].pic.sprite = leftRoomSprites[spriteInList];
		}
		else
		{
			roomComponents[slot].pic.sprite = rightRoomSprites[spriteInList];
		}
	}

}
