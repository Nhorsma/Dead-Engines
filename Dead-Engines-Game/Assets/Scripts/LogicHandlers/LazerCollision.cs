using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCollision : MonoBehaviour
{
    public ResourceHandling resourceHandling;
    public EncampmentHandler encampmentHandler;
    public EnemyHandler enemyHandler;
    public HunterHandler hunterHandler;
    public AudioHandler audioHandler;

    public float spawnBuffer, damage;

    private void Awake()
    {
        resourceHandling = FindObjectOfType<ResourceHandling>();
        encampmentHandler = FindObjectOfType<EncampmentHandler>();
        //enemyHandler = FindObjectOfType<EnemyHandler>();
        enemyHandler = (EnemyHandler)FindObjectOfType(typeof(EnemyHandler));
        hunterHandler = FindObjectOfType<HunterHandler>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

    private void OnTriggerEnter(Collider other)
    {
            Debug.Log("shooting " + other);
            if (other.gameObject.tag == "Enemy")
            {
                enemyHandler.TakeDamage(100, other.gameObject);
            }
            if (other.gameObject.tag == "Encampment")
            {
                other.gameObject.GetComponent<Encampment>().Health -= 100;
                encampmentHandler.BeDestroyed();
                audioHandler.PlayClip(other.gameObject, "explosion");
            }
            if (other.gameObject.tag == "Hunter")
            {
                hunterHandler.DealHunterDamage(other.gameObject, (int)damage);
            }
        }
}
