using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandling : MonoBehaviour
{
    public static int metal;
    public static int electronics;
    public static int fuelRods;

	public static int plate;
	public static int bolt;
	public static int part;

	public static int wire;
	public static int chip;
	public static int board;
		
    public int startQuantity;

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
    }

	void Update()
	{
		//debug stuff
		if (Input.GetKeyDown(KeyCode.Alpha0))
		{
			metal += 100;
			electronics += 100;
		}
	}

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------

    void SetUpResources()
    {
        for (int i = 0; i < resQuantities.Length; i++)
        {
            resQuantities[i] = startQuantity;
            
        }
        resDeposits = spawn.GetResources();
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
        while(!resDeposits[i].Equals(gm))
        {
            i++;
        }
        return i;
    }

    public void Extract(GameObject gm)
    {
        int i = GetNumber(gm);
        resQuantities[i] -= 1;
    }


}
