using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawn;
    public List<GameObject> enemiesGM;
    public List<Enemy> enemies;
    public float stoppingDistance, persueRange, patrolRange;


    private void Start()
    {
        enemiesGM = new List<GameObject>();
        enemies = new List<Enemy>();
        spawn = GetComponent<SpawnRes>();
    }

    private void Update()
    {
        RunJobs();
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


    void RunJobs()
    {
        foreach (Enemy e in enemies)
        {
            if (DetectTargets(e)!=null)
            {
                e.Target = DetectTargets(e);
                Fire(e);
            }
            else
            {
                GoToResource(e);
            }
        }
    }


    void GoToResource(Enemy e)
    {
        Vector3 rand = new Vector3(Random.Range(1, 3), Random.Range(1, 3), Random.Range(1, 3));
        TravelTo(GetEnemyObject(e), e.Protect.transform.position+rand, true);
    }

    void Persue(Enemy e, GameObject gm)
    {
        e.Target = gm;
        TravelTo(GetEnemyObject(e), e.Target.transform.position, true);
    }

    GameObject DetectTargets(Enemy e)
    {
        Collider range = GetEnemyObject(e).GetComponentInChildren<Collider>();
        if(range == null)
        {
            Debug.Log("its null");
            return null;
        }
        if(range != null && range.gameObject.tag.Equals("Friendly"))
        {
            return range.gameObject;
        }
        return null;
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
