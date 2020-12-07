using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatCollider : MonoBehaviour
{
    public ResourceHandling recHandle;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "Encampment")
            Destroy(other.gameObject);
        if (other.gameObject.tag == "Hunter")
            GameObject.FindGameObjectWithTag("GameController").GetComponent<HunterHandler>().DealHunterDamage(other.gameObject);
        if(other.gameObject.tag == "Metal")
        {
            recHandle.Extract(other.gameObject, 50);

            if (recHandle.resQuantities[recHandle.GetNumber(other.gameObject)] < 0)
                ResourceHandling.metal -= recHandle.resQuantities[recHandle.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.metal += 50;
        }
        if (other.gameObject.tag == "Electronics")
        {
            recHandle.Extract(other.gameObject, 50);

            if (recHandle.resQuantities[recHandle.GetNumber(other.gameObject)] < 0)
                ResourceHandling.electronics -= recHandle.resQuantities[recHandle.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.electronics += 50;
        }


    }
}
