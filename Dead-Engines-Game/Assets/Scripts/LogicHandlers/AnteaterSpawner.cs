using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnteaterSpawner : MonoBehaviour
{
    public HunterHandler handler;

    private void Start()
    {
        handler = FindObjectOfType<HunterHandler>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Robot")
        {
            handler.SpawnAnteater();
            handler.isDeployed = true;
            gameObject.SetActive(false);
        }
    }

}
