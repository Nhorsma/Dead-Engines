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

   // public GameObject[] resourceDeposits;
   // public int[] resourceQuantities;
    public SpawnRes spawnRes;
    public int resourcesLeft;

	public List<GameObject> resTranslator = new List<GameObject>();

    public List<GameObject> resourceObj = new List<GameObject>();
    public List<Resource> resourceData = new List<Resource>();

    void Start()
    {
        //resourcesLeft = 3;   //represents how many deposits have not been fully depleted

		resTranslator = spawnRes.GetAllResources();

        //resourceDeposits = new GameObject[resTranslator.Count]; //sorry had to change because of list vs array
        //resourceQuantities = new int[resourceDeposits.Length];
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
    {/*
		if (spawnRes.isActiveAndEnabled)
		{
			for (int i = 0; i < resourceQuantities.Length; i++)
			{
				resourceQuantities[i] = startQuantity;

			}
			for (int i = 0; i < resTranslator.Count; i++)
			{
				resourceDeposits[i] = resTranslator[i];
			}
			//resourceDeposits = spawnRes.GetAllResources();		// this won't work because GetAllResources returns a list now
		}
        */


        if (spawnRes.isActiveAndEnabled)
        {
            Debug.Log(" resTranslator count: " + resTranslator.Count);
            for (int i = 0; i < resTranslator.Count; i++)
            {
                if (resTranslator[i] != null)
                {
                    resourceObj.Add(resTranslator[i]);
                    resourceData.Add(resourceObj[i].GetComponent<Resource>());
                    resourceData[i].Quantity = startQuantity;
                    resourceData[i].Id = i;
                    resourceData[i].Type = resourceObj[i].tag;
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
        resourceData[i].Subtract(1);
		if (resourceData[i].Quantity <= 0)
		{
			resourceObj[i].SetActive(false);
		}
	}

	public void Extract(int id, int amount)
	{
		resourceData[id].Subtract(amount);
		if (resourceData[id].Quantity <= 0)
		{
            resourceObj[id].SetActive(false);
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
        resourceObj = newDeps;

        for (int i = 0; i < newDeps.Count; i++)
        {
            resourceData.Add(new Resource(newDeps[i].tag, startQuantity, i, newDeps[i]));
        }

        /*
		GameObject[] newDeposits = new GameObject[newDeps.Count];

		for (int i = 0; i < newDeps.Count; i++)
		{
			newDeposits[i] = newDeps[i];
		}

        int[] newQuantity = new int[newDeposits.Length];
        
        for (int i = 0; i < newQuantity.Length; i++)
        {
            newQuantity[i] = startQuantity;
        }
        


       
        for (int i = 0; i < ; i++)
        {
            int id = GetNumber(resourceDeposits[i]);
            int amount = resourceQuantities[id];
            newQuantity[id] = amount;
        }

		resourceDeposits = newDeposits;
		resourceQuantities = newQuantity;
        */
    }


	public int GetNumber(GameObject gm)
	{/*
		int i = 0;
		while (i < resourceDeposits.Length && !resourceDeposits[i].Equals(gm))
		{
			i++;
            }
		return i;
        */
        if(gm.GetComponent<Resource>()!=null)
            return gm.GetComponent<Resource>().Id;
        return -1;
	}
}
