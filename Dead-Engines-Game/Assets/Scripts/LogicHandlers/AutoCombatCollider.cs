using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatCollider : MonoBehaviour
{
    public ResourceHandling resourceHandling;
    public EncampmentHandler encampmentHandler;
    public EnemyHandler enemyHandler;
    public HunterHandler hunterHandler;
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<Enemy>().Health = 0;
        }
        if (other.gameObject.tag == "Encampment")
        {
            encampmentHandler.PlayClip(other.gameObject, "death");
            other.gameObject.GetComponent<Encampment>().Health -= 100;
			encampmentHandler.BeDestroyed();
            SpawnExplosion(other.gameObject);
        }
        if (other.gameObject.tag == "Hunter")
        {
            hunterHandler.DealHunterDamage(other.gameObject);
            SpawnExplosion(other.gameObject);
        }
        if (other.gameObject.tag == "Metal")
        {
            resourceHandling.Extract(other.gameObject, 50);

            if (resourceHandling.resourceQuantities[resourceHandling.GetNumber(other.gameObject)] < 0)
                ResourceHandling.metal -= resourceHandling.resourceQuantities[resourceHandling.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.metal += 50;

            hunterHandler.PlayClip("hit");
            hunterHandler.CheckSpawnHunter();
        }
        if (other.gameObject.tag == "Electronics")
        {
            resourceHandling.Extract(other.gameObject, 50);

            if (resourceHandling.resourceQuantities[resourceHandling.GetNumber(other.gameObject)] < 0)
                ResourceHandling.electronics -= resourceHandling.resourceQuantities[resourceHandling.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.electronics += 50;

            hunterHandler.PlayClip("hit");
            hunterHandler.CheckSpawnHunter();
        }
    }

    void SpawnExplosion(GameObject obj)
    {
        var expl = (GameObject)Instantiate(Resources.Load(explosion.name), new Vector3(obj.transform.position.x, -7, obj.transform.position.z), Quaternion.Euler(90, 0, 0));
        StartCoroutine(TrailOff(5, expl));
    }

    IEnumerator TrailOff(float time, GameObject explosion)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time);
        Destroy(explosion);
    }


}
