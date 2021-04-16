using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private int id;
    private string type;
    private int quantity;
    private Encampment level;
    private GameObject obj;

    public Resource(string t, int q, int i, GameObject o)
    {
        id = i;
        type = t;
        quantity = q;
        level = null;
        obj = o;
    }

    public void Subtract(int amount)
    {
        if (quantity <= amount)
            quantity = 0;
        quantity -= amount;
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Type
    {
        get { return type; }
        set { type = value; }
    }

    public int Quantity
    {
        get { return quantity; }
        set { quantity = value; }
    }

    public Encampment Level
    {
        get { return level; }
        set { level = value; }
    }
}
