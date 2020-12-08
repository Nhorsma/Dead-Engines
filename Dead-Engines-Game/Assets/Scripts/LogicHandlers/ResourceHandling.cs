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

    public GameObject[] resDeposits;
    public int[] resQuantities;
    public SpawnRes spawn;
    public int recsLeft;

    void Start()
    {
        recsLeft = 3;   //represents how many deposits have not been fully depleted
        resDeposits = new GameObject[spawn.GetResources().Length];
        resQuantities = new int[resDeposits.Length];
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

    void SetUpResources()
    {
		if (spawn.isActiveAndEnabled)
		{
			for (int i = 0; i < resQuantities.Length; i++)
			{
				resQuantities[i] = startQuantity;

			}
			resDeposits = spawn.GetResources();
		}
    }

    void UpdateQuantities(GameObject deposit)
    {
        int i = 0;
        GameObject resource = null;
        while(!resDeposits[i].Equals(deposit))
        {
            i++;
        }
        resource = resDeposits[i];
    }

    public int GetNumber(GameObject gm)
    {
        int i = 0;
        while(!resDeposits[i].Equals(gm) && i<resDeposits.Length)
        {
            i++;
        }
        return i;
    }

    public void Extract(GameObject gm, int amount)
    {
        int i = GetNumber(gm);
        resQuantities[i] -= amount;
        if (resQuantities[i] <= 0)
        {
            resDeposits[i].SetActive(false);
        }
    }

    public void Extract(int id, int amount)
    {
        resQuantities[id] -= amount; 
        if (resQuantities[id]<=0)
        {
            resDeposits[id].SetActive(false);
        }
    }

    //called when Fuel resource has been seized and
    //on-route to the automoton
    public void TakenFuel()
    {
        fuelTaken = true;
    }

    //called when fuel resource has arrived at automoton
    //"fuelDropped" activates the ability to upgrade the generator
    public void DroppedOffFuel()
    {
        if (fuelTaken)
            fuelDropped = true;
    }

    public void SetNewResourceDeposits(GameObject[] newDeps)
    {
        GameObject[] newDeposits = newDeps;
        int[] newQuant = new int[newDeposits.Length];
        
        for (int i = 0; i < newQuant.Length; i++)
        {
            newQuant[i] = startQuantity;
        }
       
        for (int i = 0; i < resDeposits.Length; i++)
        {
            int id = GetNumber(resDeposits[i]);
            int amount = resQuantities[id];
            newQuant[id] = amount;
        }

    resDeposits = newDeposits;
    resQuantities = newQuant;

    }

}
