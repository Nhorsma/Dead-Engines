using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LimbSystem : MonoBehaviour
{

	public List<int> torsoStats = new List<int>();
	public List<int> leftArmStats = new List<int>();
	public List<int> rightArmStats = new List<int>();
	public List<int> leftLegStats = new List<int>();
	public List<int> rightLegStats = new List<int>();

	public bool performedUpdate = false;

	public Text torsoHealth, torsoDefense;
	public List<string> torsoStrings = new List<string>();
	public List<Text> torsoRooms = new List<Text>();
	public Text leftArmHealth, leftArmDefense;
	public List<string> leftArmStrings = new List<string>();
	public List<Text> leftArmRooms = new List<Text>();
	public Text rightArmHealth, rightArmDefense;
	public List<string> rightArmStrings = new List<string>();
	public List<Text> rightArmRooms = new List<Text>();
	public Text leftLegHealth, leftLegDefense;
	public List<string> leftLegStrings = new List<string>();
	public List<Text> leftLegRooms = new List<Text>();
	public Text rightLegHealth, rightLegDefense;
	public List<string> rightLegStrings = new List<string>();
	public List<Text> rightLegRooms = new List<Text>();

	void Start()
    {
        for (int i = 0; i < 2; i++)
		{
			torsoStats.Add(0);
			leftArmStats.Add(0);
			rightArmStats.Add(0);
			leftLegStats.Add(0);
			rightLegStats.Add(0);
		}
	}

    void Update()
    {
		if (performedUpdate)
		{
			//update relevant UI
			UpdateLimbDisplay();
			Debug.Log("performedUpdate");
			performedUpdate = false;
		}
		//if (Input.GetKeyDown(KeyCode.C))
		//{
		//	//checker
		//	Debug.Log(TabCreation.FindSlot(0).Type);
		//}
    }

	public void UpdateLimbDisplay()
	{
		torsoHealth.text = torsoStats[0].ToString();
		torsoDefense.text = torsoStats[1].ToString();

		leftArmHealth.text = leftArmStats[0].ToString();
		leftArmDefense.text = leftArmStats[1].ToString();

		rightArmHealth.text = rightArmStats[0].ToString();
		rightArmDefense.text = rightArmStats[1].ToString();

		leftLegHealth.text = leftLegStats[0].ToString();
		leftLegDefense.text = leftLegStats[1].ToString();

		rightLegHealth.text = rightLegStats[0].ToString();
		rightLegDefense.text = rightLegStats[1].ToString();

		UpdateLimbRooms();
	}

	public int CheckDefense(string part)
	{
		switch (part)
		{
			case "torso":
				return torsoStats[1];
			case "leftArm":
				return leftArmStats[1];
			case "rightArm":
				return rightArmStats[1];
			case "leftLeg":
				return leftLegStats[1];
			case "rightLeg":
				return rightLegStats[1];
			default:
				return 0;
		}
	}

	public int CheckHealth(string part)
	{
		switch (part)
		{
			case "torso":
				return torsoStats[0];
			case "leftArm":
				return leftArmStats[0];
			case "rightArm":
				return rightArmStats[0];
			case "leftLeg":
				return leftLegStats[0];
			case "rightLeg":
				return rightLegStats[0];
			default:
				return 0;
		}
	}

	public void ChangeDefense(string part, int delta)
	{
		switch (part)
		{
			case "torso":
				torsoStats[1] += delta;
				break;
			case "leftArm":
				leftArmStats[1] += delta;
				break;
			case "rightArm":
				rightArmStats[1] += delta;
				break;
			case "leftLeg":
				leftLegStats[1] += delta;
				break;
			case "rightLeg":
				rightLegStats[1] += delta;
				break;
			default:
				break;
		}
	}

	public void ChangeHealth(string part, int delta)
	{
		switch (part)
		{
			case "torso":
				torsoStats[0] += delta;
				break;
			case "leftArm":
				leftArmStats[0] += delta;
				break;
			case "rightArm":
				rightArmStats[0] += delta;
				break;
			case "leftLeg":
				leftLegStats[0] += delta;
				break;
			case "rightLeg":
				rightLegStats[0] += delta;
				break;
			default:
				break;
		}
	}

	public void UpdateLimbRooms()
	{
		torsoStrings.Clear();
		rightArmStrings.Clear();
		leftArmStrings.Clear();
		rightLegStrings.Clear();
		leftLegStrings.Clear();

		for (int i = 0; i < 17; i++)
		{
			if (i < 5) //0-4 torso
			{
				torsoStrings.Add(TabCreation.FindSlot(i).Type.ToString() + ": " + TabCreation.FindSlot(i).Health.ToString() + " health, " + TabCreation.FindSlot(i).Defense.ToString() + " defense");
			}
			else if (i < 8) //5-7 rightarm
			{
				rightArmStrings.Add(TabCreation.FindSlot(i).Type.ToString() + ": " + TabCreation.FindSlot(i).Health.ToString() + " health, " + TabCreation.FindSlot(i).Defense.ToString() + " defense");
			}
			else if (i < 11) //8-10 leftarm
			{
				leftArmStrings.Add(TabCreation.FindSlot(i).Type.ToString() + ": " + TabCreation.FindSlot(i).Health.ToString() + " health, " + TabCreation.FindSlot(i).Defense.ToString() + " defense");
			}
			else if (i < 14) //11-13 rightleg
			{
				rightLegStrings.Add(TabCreation.FindSlot(i).Type.ToString() + ": " + TabCreation.FindSlot(i).Health.ToString() + " health, " + TabCreation.FindSlot(i).Defense.ToString() + " defense");
			}
			else if (i < 17) //left leg
			{
				leftLegStrings.Add(TabCreation.FindSlot(i).Type.ToString() + ": " + TabCreation.FindSlot(i).Health.ToString() + " health, " + TabCreation.FindSlot(i).Defense.ToString() + " defense");
			}
		}

		for (int i = 0; i < 5; i++)
		{
			Debug.Log(torsoStrings[i]);
			torsoRooms[i].text = torsoStrings[i];
			if (i < 3)
			{
				leftArmRooms[i].text = leftArmStrings[i];
				rightArmRooms[i].text = rightArmStrings[i];
				leftLegRooms[i].text = leftLegStrings[i];
				rightLegRooms[i].text = rightLegStrings[i];
			}
		}
	}

}
