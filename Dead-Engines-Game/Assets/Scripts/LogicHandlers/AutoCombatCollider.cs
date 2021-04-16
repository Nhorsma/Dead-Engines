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
    public GameObject explosion;

    public float spawnBuffer, damage;
	bool canTrigger = false;
    public bool canCollect;

    private void Awake()
    {
        resourceHandling = FindObjectOfType<ResourceHandling>();
        encampmentHandler = FindObjectOfType<EncampmentHandler>();
        //enemyHandler = FindObjectOfType<EnemyHandler>();
        enemyHandler = (EnemyHandler)FindObjectOfType(typeof(EnemyHandler));
        hunterHandler = FindObjectOfType<HunterHandler>();
        audioHandler = FindObjectOfType<AudioHandler>();
    }

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
				other.gameObject.GetComponent<Enemy>().Health = 0;
                canTrigger = false;
            }
			if (other.gameObject.tag == "Encampment")
			{
				other.gameObject.GetComponent<Encampment>().Health -= 100;
				encampmentHandler.BeDestroyed();
                audioHandler.PlayClip(other.gameObject, "explosion");
                SpawnExplosion(other.gameObject);
                canTrigger = false;
            }
			if (other.gameObject.tag == "Hunter")
			{
				hunterHandler.DealHunterDamage(other.gameObject, (int)damage);
				SpawnExplosion(other.gameObject);
                canTrigger = false;
            }
			if (canCollect && other.gameObject.tag == "Metal")
			{
				resourceHandling.Extract(other.gameObject, 50);
                int left = resourceHandling.resourceData[resourceHandling.GetNumber(other.gameObject)].Quantity;

                if (left < 0)
					ResourceHandling.metal -= left; //subtracting a negative is positive
				else
					ResourceHandling.metal += 50;
                canTrigger = false;

                audioHandler.PlayClip(other.gameObject, "explosion");
                hunterHandler.CheckSpawnHunter();
            }
			if (canCollect && other.gameObject.tag == "Electronics")
			{
				resourceHandling.Extract(other.gameObject, 50);
                int left = resourceHandling.resourceData[resourceHandling.GetNumber(other.gameObject)].Quantity;

                if (left < 0)
					ResourceHandling.electronics -= left; //subtracting a negative is positive
				else
					ResourceHandling.electronics += 50;
                canTrigger = false;

                audioHandler.PlayClip(other.gameObject, "explosion");
				hunterHandler.CheckSpawnHunter();
			}
		}
    }

    void SpawnExplosion(GameObject obj)
    {
        var expl = (GameObject)Instantiate(Resources.Load(explosion.name), new Vector3(obj.transform.position.x, -7, obj.transform.position.z), Quaternion.Euler(90, 0, 0));
        StartCoroutine(TrailOff(5, expl));
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
