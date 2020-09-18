using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceHandling : MonoBehaviour
{
    public int metal;
    public int electronics;
    public int startQuantity;

    public GameObject[] resDeposits;
    public int[] resQuantities;
    public SpawnRes spawn;

    void Start()
    {
        resDeposits = new GameObject[spawn.GetResources().Length];
        resQuantities = new int[resDeposits.Length];
        SetUpResources();
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
