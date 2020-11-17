using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatCollider : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
            Destroy(other.gameObject);
        if (other.gameObject.tag == "Hunter")
            GameObject.FindGameObjectWithTag("GameController").GetComponent<HunterHandler>().DealHunterDamage(other.gameObject);

    }
}
