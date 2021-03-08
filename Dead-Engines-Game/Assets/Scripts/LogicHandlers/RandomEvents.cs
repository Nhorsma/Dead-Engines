using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RandomEvents : MonoBehaviour
{
	//every 3 mins that the game has been running, random event
	//if unit capacity has been met, runs every 5 mins
	// (at least for phase 1)

	public UnitManager unitManager;

	public float randomEventCounter = 0f; //3 mins in seconds, 60*3
	public float randomEventHit = 180f; //

	public Text promptText;
	public string prompt;
	public List<string> rumors = new List<string>();

	bool finished = false;

	string contract = "";

	public Button yesButton;
	public Button noButton;
	public Button okayButton;

	public GameObject eventPanel;

    void Start()
    {
		promptText.text = "";
		eventPanel.SetActive(false);
    }

    void Update()
    {
		randomEventCounter += Time.deltaTime;
		//Debug.Log(randomEventCounter);
		if (randomEventCounter > randomEventHit)
		{
			TriggerEvent();
			randomEventCounter = 0;
		}

		if (finished)
		{
			DoContract(contract);
			finished = false;
		}

		if (Input.GetKeyDown(KeyCode.Backslash))
		{
			TriggerEvent();
			randomEventCounter = 0;
		}
    }

	void TriggerEvent()
	{
		yesButton.gameObject.SetActive(false);
		noButton.gameObject.SetActive(false);
		okayButton.gameObject.SetActive(false);

		yesButton.onClick.RemoveAllListeners();
		noButton.onClick.RemoveAllListeners();

		int r = 0;
		r = Random.Range(0, 2);					// turned off trader for now
		if (r == 0)
		{
			//unit
			UnitEvent();
		}
		else if (r == 1)
		{
			//rumor
			RumorEvent();
		}
		else if (r == 2)
		{
			//trade
			TradeEvent();
		}
	}

	void UnitEvent()
	{
		prompt = "A scraggly survivor approaches. Should we take him in?";
		Debug.Log(prompt);
		//turn yes & no buttons on [contract]
		yesButton.gameObject.SetActive(true);
		yesButton.onClick.AddListener(delegate { SetContract("add unit"); });
		noButton.gameObject.SetActive(true);
		noButton.onClick.AddListener(delegate { SetContract("dismiss unit"); });

		promptText.text = prompt;
		eventPanel.SetActive(true);
		//1/3 chance of mutiny event later ---->
	}

	void RumorEvent()
	{
		int rumor = 0;
		int asked = 0;
		asked = Random.Range(0, 2);

		rumor = Random.Range(0, 16); //15 rumors

		//even numbered rumors are free, odds cost resources to hear
		if (rumor%2 == 0)
		{
			int rumRand = Random.Range(0, 5);
			prompt = "A passerby said, " + rumors[rumRand];
			//turn okay button on
			okayButton.gameObject.SetActive(true);
		}
		else if (rumor % 2 != 0)
		{
			if (asked == 0)
			{
				prompt = "A wanderer arrives. He's willing to share information for 10 metal...";
				//turn metal specific yes & no button on [contract]
				yesButton.gameObject.SetActive(true);
				yesButton.onClick.AddListener(delegate { SetContract("paid metal rumor"); });
				noButton.gameObject.SetActive(true);
				noButton.onClick.AddListener(delegate { SetContract("no paid rumor"); });
			}
			else if (asked == 1)
			{
				prompt = "A wanderer arrives. He's willing to share information for 5 electronics...";
				//turn electronic specific yes & no button on [contract]
				yesButton.gameObject.SetActive(true);
				yesButton.onClick.AddListener(delegate { SetContract("paid electronics rumor"); });
				noButton.gameObject.SetActive(true);
				noButton.onClick.AddListener(delegate { SetContract("no paid rumor"); });
			}
		}

		promptText.text = prompt;
		eventPanel.SetActive(true);
		Debug.Log(prompt);
	}

	void TradeEvent()
	{
		//change promptText.text
		prompt = "A trader has arrived. Browse his wares?";
		Debug.Log(prompt);
		//turn yes & no buttons on
		yesButton.gameObject.SetActive(true);
		yesButton.onClick.AddListener(delegate { SetContract("trade"); });
		noButton.gameObject.SetActive(true);
		noButton.onClick.AddListener(delegate { SetContract("no trade"); });
		//okayButton.gameObject.SetActive(true);

		promptText.text = prompt;
		eventPanel.SetActive(true);
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
			Debug.Log(what);
			unitManager.TakeInUnit(); //no mutiny yet
		}
		else if (what == "dismiss unit")
		{
			Debug.Log(what);
		}
		else if (what == "trade")
		{
			promptText.text = "He takes off his sock and tries to sell it to you. Turns out he only sells wears, not wares.";
			yesButton.gameObject.SetActive(false);
			noButton.gameObject.SetActive(false);
			okayButton.gameObject.SetActive(true);
			return;
			//Debug.Log(what);
		}
		else if (what == "no trade")
		{
			Debug.Log(what);
		}
		else if (what == "free rumor")
		{
			Debug.Log(what);
		}
		else if (what == "no paid rumor")
		{
			Debug.Log(what);
		}
		else if (what == "paid metal rumor")
		{
			Debug.Log(what);
			int randRumor = Random.Range(5, 10);
			//change text to the new rumor
			promptText.text = rumors[randRumor];
			ResourceHandling.metal -= 10; // errrrr set to 10 for now, fix logic later
										 
			//turn off yes/no buttons & turn on okay button
			yesButton.gameObject.SetActive(false);
			noButton.gameObject.SetActive(false);
			okayButton.gameObject.SetActive(true);
			return;
		}
		else if (what == "paid electronics rumor")
		{
			int randRumor = Random.Range(5, 10);
			//change text to the new rumor
			promptText.text = rumors[randRumor];
			ResourceHandling.electronics -= 5; // same as ^
											   
			//turn off yes/no buttons & turn on okay button
			yesButton.gameObject.SetActive(false);
			noButton.gameObject.SetActive(false);
			okayButton.gameObject.SetActive(true);
			return;
		}
		CloseBox();
	}

	public void CloseBox() // okay button method
	{
		eventPanel.SetActive(false);
		//close event box
	}

}
