﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerCollision : MonoBehaviour
{
    public ResourceHandling resourceHandling;
    public EncampmentHandler encampmentHandler;
    public EnemyHandler enemyHandler;
    public HunterHandler hunterHandler;
    public AudioHandler audioHandler;
    public GameObject automaton;

    public float spawnBuffer, damage;
    public GameObject bigExplosion;

    private void Awake()
    {
        resourceHandling = FindObjectOfType<ResourceHandling>();
        encampmentHandler = FindObjectOfType<EncampmentHandler>();
        enemyHandler = FindObjectOfType<EnemyHandler>();
        hunterHandler = FindObjectOfType<HunterHandler>();
        audioHandler = FindObjectOfType<AudioHandler>();
        automaton = GameObject.FindGameObjectWithTag("Robot");

        damage = 100f;
        Physics.IgnoreCollision(automaton.GetComponent<BoxCollider>(), automaton.transform.root.GetComponent<BoxCollider>());
    }

    private void OnTriggerEnter(Collider other)
    {
        Physics.IgnoreCollision(automaton.GetComponent<BoxCollider>(), automaton.transform.root.GetComponent<BoxCollider>());
        Debug.Log("lazer to " + other);
            if (other.gameObject.tag == "Enemy")
            {
                enemyHandler.TakeDamage(100, other.gameObject);
                audioHandler.PlayClipIgnore(other.gameObject, "explosion");
                SpawnExplosion(other.gameObject);
            }
            if (other.gameObject.tag == "Encampment")
            {
                other.gameObject.GetComponent<Encampment>().Health -= 100;
                encampmentHandler.BeDestroyed();
                audioHandler.PlayClipIgnore(other.gameObject, "explosion");
                SpawnExplosion(other.gameObject);
            }
            if (other.gameObject.tag == "Hunter")
            {
                hunterHandler.DealHunterDamage(other.gameObject, (int)damage);
                audioHandler.PlayClipIgnore(other.gameObject, "explosion");
                SpawnExplosion(other.gameObject);
            }
    }

    void SpawnExplosion(GameObject obj)
    {
        var expl = (GameObject)Instantiate(Resources.Load("BigExplosionEffect"), new Vector3(obj.transform.position.x, -7, obj.transform.position.z), Quaternion.Euler(90, 0, 0));
        StartCoroutine(TrailOff(3, expl));
    }

    IEnumerator TrailOff(float time, GameObject explosion)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time);
        Destroy(explosion);
    }
}
