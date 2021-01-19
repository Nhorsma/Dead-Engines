using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandling : MonoBehaviour
{
    public static int metal;
    public static int electronics;

	public static int plate;
	public static int bolt;
	public static int part;

	public static int wire;
	public static int chip;
	public static int board;
		
    public int startQuantity;

    public GameObject fuel;
    public bool fuelTaken;
    public bool fuelDropped;

    public GameObject[] resourceDeposits;
    public int[] resourceQuantities;
    public SpawnRes spawnRes;
    public int resourcesLeft;

    void Start()
    {
        resourcesLeft = 3;   //represents how many deposits have not been fully depleted
        resourceDeposits = new GameObject[spawnRes.GetResources().Length];
        resourceQuantities = new int[resourceDeposits.Length];
        SetUpResources();

        fuelDropped = fuelTaken = false;
    }

	void Update()
	{
		//debug stuff
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			metal += 10;
			electronics += 10;
		}
	}

	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	/// <summary>
	/// INITIALIZE --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void SetUpResources()
    {
		if (spawnRes.isActiveAndEnabled)
		{
			for (int i = 0; i < resourceQuantities.Length; i++)
			{
				resourceQuantities[i] = startQuantity;

			}
			resourceDeposits = spawnRes.GetResources();
		}
    }

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void UpdateQuantities(GameObject deposit)
    {
        int i = 0;
        GameObject resource = null;
        while(!resourceDeposits[i].Equals(deposit))
        {
            i++;
        }
        resource = resourceDeposits[i];
    }

	public void Extract(GameObject deposit, int amount)
	{
		int i = GetNumber(deposit);
		resourceQuantities[i] -= amount;
		if (resourceQuantities[i] <= 0)
		{
			resourceDeposits[i].SetActive(false);
		}
	}

	public void Extract(int id, int amount)
	{
		resourceQuantities[id] -= amount;
		if (resourceQuantities[id] <= 0)
		{
			resourceDeposits[id].SetActive(false);
		}
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	//called when Fuel resource has been seized and on-route to the automoton
	public void TakenFuel()
    {
        fuelTaken = true;
    }
	//called when fuel resource has arrived at automoton "fuelDropped" activates the ability to upgrade the generator
	public void DroppedOffFuel()
    {
        if (fuelTaken)
            fuelDropped = true;
    }

    public void SetNewResourceDeposits(GameObject[] newDeps)
    {
        GameObject[] newDeposits = newDeps;
        int[] newQuantity = new int[newDeposits.Length];
        
        for (int i = 0; i < newQuantity.Length; i++)
        {
            newQuantity[i] = startQuantity;
        }
       
        for (int i = 0; i < resourceDeposits.Length; i++)
        {
            int id = GetNumber(resourceDeposits[i]);
            int amount = resourceQuantities[id];
            newQuantity[id] = amount;
        }

    resourceDeposits = newDeposits;
    resourceQuantities = newQuantity;

    }
	public int GetNumber(GameObject gm)
	{
		int i = 0;
		while (!resourceDeposits[i].Equals(gm) && i < resourceDeposits.Length)
		{
			i++;
		}
		return i;
	}
}
