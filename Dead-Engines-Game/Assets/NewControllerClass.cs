using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewControllerClass : NewRoomClass
{

	public Button wellOiledButton;
	public Button reinforcedButton;
	public Button overclockedButton;
	public Button lasersButton;
	public Button artilleryButton;

	public Text laserText;
	public Text reinforcedText;
	public Text overclockedText;
	public Text artilleryText;
	public Text oiledText;

	public void Start()
	{
	}

	public void Update()
	{

	}

	public NewControllerClass()
	{

	}

	public void ToggleButtons()
	{
		buildButton.gameObject.SetActive(false);

		wellOiledButton.gameObject.SetActive(true);
		oiledText.gameObject.SetActive(true);
		reinforcedButton.gameObject.SetActive(true);
		reinforcedText.gameObject.SetActive(true);
		overclockedButton.gameObject.SetActive(true);
		overclockedText.gameObject.SetActive(true);
		lasersButton.gameObject.SetActive(true);
		laserText.gameObject.SetActive(true);
		artilleryButton.gameObject.SetActive(true);
		artilleryText.gameObject.SetActive(true);
	}

}
