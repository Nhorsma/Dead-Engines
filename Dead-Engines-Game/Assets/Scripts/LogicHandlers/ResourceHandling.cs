using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandling : MonoBehaviour
{
    public static int metal;
    public static int electronics;

    public static int oil;
    public static int data;

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

    public SpawnRes spawnRes;
    public int resourcesLeft;

	public List<GameObject> resTranslator = new List<GameObject>();

    public List<GameObject> resourceObjects = new List<GameObject>();
    public List<Resource> resourceData = new List<Resource>();

	public static int storageUsed = 0;

    void Start()
    {
		resTranslator = spawnRes.GetAllResources();

        SetUpResources();

        fuelDropped = fuelTaken = false;
    }

	void Update()
	{
		//debug stuff
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			metal += 100;
			electronics += 100;
            oil += 100;
            data += 100;
			chip += 100;
			part += 100;
			board += 100;
			plate += 100;
			bolt += 100;
			wire += 100;
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
			int tick = 0;
			foreach (GameObject o in resTranslator)
			{
				if (o != null)
				{
					resourceObjects.Add(o);
					resourceData.Add(o.GetComponent<Resource>());
					resourceData[tick].Id = tick;
					resourceData[tick].Type = resourceObjects[tick].tag;
					if (o.GetComponent<Resource>().Type == "Crater")
					{
						resourceData[tick].Quantity = 1;
					}
					else if (o.GetComponent<Resource>().Type == "Oil")
					{
						resourceData[tick].Quantity = 50;
					}
					else
					{
						resourceData[tick].Quantity = startQuantity;
					}
					tick++;
				}
			}
        }
    }

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>
/*
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
    */

	public void Extract(GameObject deposit, int amount)
	{
        //int i = GetNumber(deposit);
        int i = deposit.GetComponent<Resource>().Id;
        resourceData[i].Subtract(amount);
		if (resourceData[i].Quantity <= 0)
		{
			resourceObjects[i].SetActive(false);
		}
	}

	public void Extract(int id, int amount)
	{
		resourceData[id].Subtract(amount);
		if (resourceData[id].Quantity <= 0)
		{
            resourceObjects[id].SetActive(false);
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

    public void SetNewResourceDeposits(List<GameObject> newDeps)
    {
        resourceObjects = newDeps;

        for (int i = 0; i < newDeps.Count; i++)
        {
            resourceData.Add(new Resource(newDeps[i].tag, startQuantity, i, newDeps[i]));
        }
    }


	public int GetNumber(GameObject gm)
	{
        if(gm.GetComponent<Resource>()!=null)
            return gm.GetComponent<Resource>().Id;
        return -1;
	}
}
