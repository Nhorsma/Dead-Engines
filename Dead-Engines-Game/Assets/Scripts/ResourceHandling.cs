using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandling : MonoBehaviour
{
    public static int metal;
    public static int electronics;
    public int startQuantity;

    GameObject[] selected;
    GameObject[] resDeposits;
    int[] resQuantities;
    public SpawnRes spawn;

    void Start()
    {
        resDeposits = spawn.GetResources();
        resQuantities = new int[resDeposits.Length];
        StartQuantities();
    }

    void Update()
    {
        
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------

    void StartQuantities()
    {
        for (int i = 0; i < resQuantities.Length; i++)
        {
            resQuantities[i] = startQuantity;
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
