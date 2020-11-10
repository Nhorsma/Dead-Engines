using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
	//every 3 mins that the game has been running, random event
	//if unit capacity has been met, runs every 5 mins
	// (at least for phase 1)

	public float randomEventCounter = 0f; //3 mins in seconds, 60*3
	public float randomEventHit = 180f; //

	public Text promptText;
	public List<string> rumors = new List<string>();

	bool finished = false;

	string contract = "";

    void Start()
    {
		promptText.text = "";
    }

    void Update()
    {
		randomEventCounter += Time.deltaTime;
		if (randomEventCounter > randomEventHit)
		{
			TriggerEvent();
		}

		if (finished)
		{
			DoContract(contract);
			finished = false;
		}
    }

	void TriggerEvent()
	{
		int r = 0;
		r = Random.Range(0, 2); //both are inclusive apparently
		if (r == 0)
		{
			//unit
			
		}
		else if (r == 1)
		{
			//rumor
		}
		else if (r == 2)
		{
			//trade
		}
	}

	void UnitEvent()
	{
		promptText.text = "A scraggly survivor approaches...";
		//turn yes & no buttons on

		//1/3 chance of mutiny event later ---->
	}

	void RumorEvent()
	{
		int rumor = 0;
		int asked = 0;
		asked = Random.Range(0, 1);

		rumor = Random.Range(0, 14); //15 rumors

		//even numbered rumors are free, odds cost resources to hear
		if (rumor%2 == 0)
		{
			promptText.text = "A wanderer arrives. He speaks of " + rumors[rumor];
			//turn okay button on
		}
		else if (rumor % 2 != 0)
		{
			if (asked == 0)
			{
				promptText.text = "A wanderer arrives. He's willing to share information for " + rumor + " metal...";
				//turn metal specific yes & no button on
			}
			else if (asked == 1)
			{
				promptText.text = "A wanderer arrives. He's willing to share information for " + rumor + " electronics...";
				//turn electronic specific yes & no button on
			}
		}
	}

	void TradeEvent()
	{
		//change promptText.text
		//turn yes & no buttons on
	}

	public void SetContract(string what) //some event buttons will pass strings
	{
		contract = what;
		finished = true;
	}

	void DoContract(string what)
	{
		if (what == "add unit")
		{
			//add unit
		}
		else if (what == "dismiss unit")
		{
			return;
		}
		else if (what == "trade")
		{
			//open trade box
		}
		else if (what == "no trade")
		{
			return;
		}
		else if (what == "free rumor")
		{

			return;
		}
		else if (what == "no paid rumor")
		{
			return;
		}
		else if (what == "paid metal rumor")
		{
			ResourceHandling.metal -= 10; // errrrr set to 10 for now, fix logic later
			//turn off button
			//turn on "okay" button -> close box
			//change text
		}
		else if (what == "paid electronics rumor")
		{
			ResourceHandling.electronics -= 5; // same as ^
			//turn off button
			//turn on "okay" button -> close box
			//change text
		}
	}

	public void CloseBoxOnOkay() // okay button method
	{
		//close event box
	}

}
