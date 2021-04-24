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
    public GameObject bigExplosion;

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
        StartCoroutine(TrailOff(5, expl));
    }

    IEnumerator TrailOff(float time, GameObject explosion)
    {
        explosion.GetComponent<ParticleSystem>().Play();
        yield return new WaitForSeconds(time);
        Destroy(explosion);
    }
}
