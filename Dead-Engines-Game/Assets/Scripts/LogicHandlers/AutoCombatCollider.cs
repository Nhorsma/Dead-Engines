using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatCollider : MonoBehaviour
{
    public ResourceHandling resourceHandling;
    public EncampmentHandler encampmentHandler;
    public EnemyHandler enemyHandler;
    public HunterHandler hunterHandler;
    public AudioHandler audioHandler;
    public GameObject smallExplosion, bigExplosion;

    public float spawnBuffer, damage;
	bool canTrigger = false;
    public bool canCollect;

    void Update()
	{
        if (!canTrigger)
        {
            StartCoroutine(HitSomething());
        }
	}

    private void OnTriggerEnter(Collider other)
    {
		if (canTrigger)
		{
            Debug.Log("stepping on " + other);
			if (other.gameObject.tag == "Enemy")
			{
                enemyHandler.TakeDamage(100, other.gameObject);
                canTrigger = false;
            }
			if (other.gameObject.tag == "Encampment")
			{
				other.gameObject.GetComponent<Encampment>().Health -= 100;
				encampmentHandler.BeDestroyed();
                audioHandler.PlayClip(other.gameObject, "explosion");
                SpawnExplosion(other.gameObject,"big");
                canTrigger = false;
            }
			if (other.gameObject.tag == "Hunter")
			{
				hunterHandler.DealHunterDamage(other.gameObject, 100);
				SpawnExplosion(other.gameObject,"big");
                canTrigger = false;
            }
			if (canCollect && other.gameObject.tag == "Metal")
			{
                int left = resourceHandling.resourceData[resourceHandling.GetNumber(other.gameObject)].Quantity;
                if (left>50)
                {
                    resourceHandling.Extract(other.gameObject, 50);
                    ResourceHandling.metal += 50;
                }
                else
                {
                    resourceHandling.Extract(other.gameObject,left);
                    ResourceHandling.metal += left;
                }
                canTrigger = false;
                audioHandler.PlayClip(other.gameObject, "explosion");
                hunterHandler.CheckSpawnHunter();
            }
			if (canCollect && other.gameObject.tag == "Electronics")
			{
                int left = resourceHandling.resourceData[resourceHandling.GetNumber(other.gameObject)].Quantity;
                if (left > 50)
                {
                    resourceHandling.Extract(other.gameObject, 50);
                    ResourceHandling.electronics += 50;
                }
                else
                {
                    resourceHandling.Extract(other.gameObject, left);
                    ResourceHandling.electronics += left;
                }
                canTrigger = false;
                audioHandler.PlayClip(other.gameObject, "explosion");
                hunterHandler.CheckSpawnHunter();
            }
		}
    }

    void SpawnExplosion(GameObject obj, string size)
    {
        string type = "";
        if (size == "big")
            type = bigExplosion.name;
        else
            type = smallExplosion.name;
        var expl = (GameObject)Instantiate(Resources.Load(type), new Vector3(obj.transform.position.x, -7, obj.transform.position.z), Quaternion.Euler(90, 0, 0));
        StartCoroutine(TrailOff(3, expl));
    }

    IEnumerator HitSomething()
    {
        canTrigger = false;
        yield return new WaitForSeconds(spawnBuffer);
        canTrigger = true;
    }

    IEnumerator TrailOff(float time, GameObject explosion)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time);
        Destroy(explosion);
    }


}
