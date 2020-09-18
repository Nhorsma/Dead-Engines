using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawn;
    public List<GameObject> enemiesGM;
    public List<Enemy> enemies;
    public float stoppingDistance, persueRange;


    private void Start()
    {
        enemiesGM = new List<GameObject>();
        enemies = new List<Enemy>();
        spawn = GetComponent<SpawnRes>();
    }

    private void Update()
    {
        Wander();
    }


    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------



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
        if(enemies.Count>0)
        foreach (Enemy e in enemies)
        {
            //Debug.Log(e.Protect);
            TravelTo(GetEnemyObject(e), e.Protect.transform.position, false);

        }
    }

    void Persue(Enemy e, GameObject gm)
    {
        e.Target = gm;
        TravelTo(GetEnemyObject(e), e.Target.transform.position, true);
    }

    void CheckPersue()
    {
        /*
        GameObject[] u = GetComponent<UnitManager>().unitsGM;
        foreach(GameObject unit in u)
        {
            foreach(GameObject enemy in enemiesGM)
            {
                if(Vector3())
            }
        }
        */
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

    void TravelTo(GameObject a, Vector3 place, bool stop)
    {
        if (a != null && a.GetComponent<NavMeshAgent>() != null)
        {
            if(stop)
                a.GetComponent<NavMeshAgent>().stoppingDistance = stoppingDistance;
            a.GetComponent<NavMeshAgent>().SetDestination(place);
        }
    }

}
