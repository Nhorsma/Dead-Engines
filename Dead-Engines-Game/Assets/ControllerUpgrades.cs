using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUpgrades : MonoBehaviour
{

	//need the buttons to be able to turn off/disable
	public NewControllerClass controller;
    public AutomotonAction automotonAction;

	//don't be scared to put script references here!

    void Start()
    {
        
    }

    void Update()
    {
        
    }


	public void BuildLasers()
	{
		if (ResourceHandling.chip >= 10 && ResourceHandling.board >= 5 && ResourceHandling.data >= 3)
		{
			ResourceHandling.chip -= 10;
			ResourceHandling.board -= 5;
			ResourceHandling.data -= 3;
            automotonAction.canLazer = true;
			controller.lasersButton.gameObject.SetActive(false);
			controller.laserText.text = "laser installed [E]";
		}
		else
		{
			Debug.Log("Not enough resources to build laser");
		}
	}

	public void BuildArtillery()
	{
		if (ResourceHandling.plate >= 10 && ResourceHandling.part >= 5 && ResourceHandling.data >= 3)
		{
			ResourceHandling.plate -= 10;
			ResourceHandling.part -= 5;
			ResourceHandling.data -= 3;
            automotonAction.canBarrage = true;
			controller.artilleryButton.gameObject.SetActive(false);
			controller.artilleryText.text = "artillery installed [R]";
        }
		else
		{
			Debug.Log("Not enough resources to build artillery");
		}
	}

	public void WellOiled()
	{
		if (ResourceHandling.board >= 10 && ResourceHandling.part >= 10 && ResourceHandling.data >= 5)
		{
			ResourceHandling.board -= 10;
			ResourceHandling.part -= 10;
			ResourceHandling.data -= 5;
            automotonAction.wellOiled = true;
			controller.wellOiledButton.gameObject.SetActive(false);
			controller.oiledText.text = "+ fuel efficiency installed";
		}
		else
		{
			Debug.Log("Not enough resources to upgrade to Well-Oiled");
		}
	}

	public void Reinforced()
	{
		if (ResourceHandling.part >= 20 && ResourceHandling.data >= 5)
		{
			ResourceHandling.part -= 20;
			ResourceHandling.data -= 5;
            automotonAction.reinforced = true;
			controller.reinforcedButton.gameObject.SetActive(false);
			controller.reinforcedText.text = "+ defense installed";
		}
		else
		{
			Debug.Log("Not enough resources to upgrade to Reinforced");
		}
	}

	public void Overclocked()
	{
		if (ResourceHandling.board >= 20 && ResourceHandling.data >= 5)
		{
			ResourceHandling.board -= 20;
			ResourceHandling.data -= 5;
            automotonAction.movementSpeed = automotonAction.fastMovementSpeed;
            automotonAction.anim.speed = automotonAction.fastMovementSpeed/automotonAction.startMovementSpeed;
            automotonAction.overclocked = true;
			controller.overclockedButton.gameObject.SetActive(false);
			controller.overclockedText.text = "+ speed installed";
        }
		else
		{
			Debug.Log("Not enough resources to upgrade to Overclocked");
		}
	}

}
