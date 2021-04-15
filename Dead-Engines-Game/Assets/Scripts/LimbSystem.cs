using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbSystem : MonoBehaviour
{

	public List<int> headStats = new List<int>();
	public List<int> torsoStats = new List<int>();
	public List<int> leftArmStats = new List<int>();
	public List<int> rightArmStats = new List<int>();
	public List<int> leftLegStats = new List<int>();
	public List<int> rightLegStats = new List<int>();

	public bool performedUpdate = false;

    void Start()
    {
        for (int i = 0; i < 3; i++)
		{
			headStats.Add(1);
			torsoStats.Add(1);
			leftArmStats.Add(1);
			rightArmStats.Add(1);
			leftLegStats.Add(1);
			rightLegStats.Add(1);
		}
    }

    void Update()
    {
		if (performedUpdate)
		{
			//update relevant UI
			//recalculate combined numbers
			Debug.Log("performedUpdate");
			performedUpdate = false;
		}
    }

	public int CheckAttack(string part)
	{
		switch (part)
		{
			case "head":
				return headStats[0];
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

	public int CheckDefense(string part)
	{
		switch (part)
		{
			case "head":
				return headStats[1];
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
			case "head":
				return headStats[2];
			case "torso":
				return torsoStats[2];
			case "leftArm":
				return leftArmStats[2];
			case "rightArm":
				return rightArmStats[2];
			case "leftLeg":
				return leftLegStats[2];
			case "rightLeg":
				return rightLegStats[2];
			default:
				return 0;
		}
	}

	public void ChangeAttack(string part, int delta)
	{
		switch (part)
		{
			case "head":
				headStats[0] += delta;
				break;
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

	public void ChangeDefense(string part, int delta)
	{
		switch (part)
		{
			case "head":
				headStats[1] += delta;
				break;
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
			case "head":
				headStats[2] += delta;
				break;
			case "torso":
				torsoStats[2] += delta;
				break;
			case "leftArm":
				leftArmStats[2] += delta;
				break;
			case "rightArm":
				rightArmStats[2] += delta;
				break;
			case "leftLeg":
				leftLegStats[2] += delta;
				break;
			case "rightLeg":
				rightLegStats[2] += delta;
				break;
			default:
				break;
		}
	}

}
