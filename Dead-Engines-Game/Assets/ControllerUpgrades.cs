using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerUpgrades : MonoBehaviour
{

	//need the buttons to be able to turn off/disable
	public Button buildLasersButton;
	public Button buildArtilleryButton;
	public Button wellOiledButton;
	public Button reinforcedButton;
	public Button overclockedButton;

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
			//set laser availability to true
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
			//set artillery availability to true
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
			//set automaton fuel multiplier to .5
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
			//set automaton def multiplier to 2
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
			//set automaton sprint availability to true
		}
		else
		{
			Debug.Log("Not enough resources to upgrade to Overclocked");
		}
	}

}
