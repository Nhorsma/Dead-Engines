using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhaseTwo : MonoBehaviour
{
    public static bool endPhaseOne;
    public static AutomotonAction automatonAction;
    public static HunterHandler hunterHandler;

	public AutomatonUI auto;
	public SpawnRes spawnRes;
	public GameObject autoObj;
	public SelectItems selectItems;
	public ResourceHandling resourceHandling;

	public static bool isAutomatonRepaired = false;

	public SphereCollider sphereThinner;

    void Start()
    {
        automatonAction = GameObject.FindGameObjectWithTag("Robot").GetComponent<AutomotonAction>();
        hunterHandler = GetComponent<HunterHandler>();
        automatonAction.enabled = false;
        hunterHandler.enabled = false;
        endPhaseOne = false;
    }

    public static void PhaseTwo()
    {
        automatonAction.enabled = true;
        hunterHandler.enabled = true;
        endPhaseOne = true;
    }

	public void RepairAutomaton()
	{
		isAutomatonRepaired = true;
		Debug.Log("repaired automaton");

		//activates automoton movement script
		PhaseTwo();
	}

	public void ActivateAutomoton()
	{
		sphereThinner.enabled = false;	// so the automaton doesn't delete resources inadvertently
		auto.activationButton.gameObject.SetActive(false);
		autoObj.GetComponent<AutomotonAction>().enabled = true;
		hunterHandler.enabled = true;
		//resourceHandling.SetNewResourceDeposits(spawnRes.GetAllResources());
	}

}
