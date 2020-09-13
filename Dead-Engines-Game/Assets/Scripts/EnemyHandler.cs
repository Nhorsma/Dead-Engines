using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawn;
    public List<GameObject> enemiesGM;
    public List<Enemy> enemies;
    public float stoppingDistance;


    private void Start()
    {
        enemiesGM = new List<GameObject>();
        enemies = new List<Enemy>();
        spawn = GetComponent<SpawnRes>();
    }

    private void Update()
    {
        
    }

    public GameObject GetEnemyObject(Enemy ene)
    {
        return enemiesGM[ene.Id];
    }

    public  Enemy GetEnemy(GameObject gm)
    {
        for (int i = 0; i < enemiesGM.Count; i++)
        {
            if (enemiesGM[i] == gm)
                return enemies[i];
        }
        return null;
    }


    void Wander()
    {

    }

    void Persue()
    {

    }

    IEnumerator FireCoolDown(Enemy ene)
    {
        ene.JustShot = true;
        yield return new WaitForSeconds(1f);
        ene.JustShot = false;
    }

    void Fire(Enemy ene)
    {
        Vector3 direction = ene.Target.transform.position - GetEnemyObject(ene).transform.position;

        RaycastHit hit;
        if (Physics.Raycast(GetEnemyObject(ene).transform.position, direction, out hit, 100f))
        {
            if (hit.collider.tag == "Friendly")
                Debug.Log("Bang!");
        }
    }

}
