using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoCombatCollider : MonoBehaviour
{
    public ResourceHandling recHandle;
    public EncampmentHandler campHandle;
    public EnemyHandler enemyHandle;
    public HunterHandler huntHandle;
    public GameObject explosion;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            enemyHandle.GetEnemy(other.gameObject).Health = 0;
        }
        if (other.gameObject.tag == "Encampment")
        {
            campHandle.PlayClip(campHandle.GetEncampment(other.gameObject), "death");
            campHandle.GetEncampment(other.gameObject).Health -= 100;
            campHandle.BeDestroyed();
            SpawnExplosion(other.gameObject);
        }
        if (other.gameObject.tag == "Hunter")
        {
            huntHandle.DealHunterDamage(other.gameObject);
            SpawnExplosion(other.gameObject);
        }
        if (other.gameObject.tag == "Metal")
        {
            recHandle.Extract(other.gameObject, 50);

            if (recHandle.resQuantities[recHandle.GetNumber(other.gameObject)] < 0)
                ResourceHandling.metal -= recHandle.resQuantities[recHandle.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.metal += 50;

            huntHandle.PlayClip("hit");
            huntHandle.CheckSpawnHunter();
        }
        if (other.gameObject.tag == "Electronics")
        {
            recHandle.Extract(other.gameObject, 50);

            if (recHandle.resQuantities[recHandle.GetNumber(other.gameObject)] < 0)
                ResourceHandling.electronics -= recHandle.resQuantities[recHandle.GetNumber(other.gameObject)]; //subtracting a negative is positive
            else
                ResourceHandling.electronics += 50;

            huntHandle.PlayClip("hit");
            huntHandle.CheckSpawnHunter();
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
