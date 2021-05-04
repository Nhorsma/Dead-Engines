using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewRoomClass : MonoBehaviour
{
	//data
	private int slot;
	private string type;
	private int level;
	private int health;
	private int defense;
	private string other;

	//components
	public GameObject roomTab;
	public Text nameText;
	public Text capacityText;
	public Image roomPic;
	public Button buildButton;
	public Button upgradeButton;

	public int Slot { get => slot; set => slot = value; }
	public string Type { get => type; set => type = value; }
	public int Level { get => level; set => level = value; }
	public int Health { get => health; set => health = value; }
	public int Defense { get => defense; set => defense = value; }
	public string Other { get => other; set => other = value; }

	public NewRoomClass()
	{

	}

}
