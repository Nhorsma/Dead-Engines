using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawnResources;
    UnitManager unitManager;
    EncampmentHandler encampmentHandler;
    public GameObject automoton;
    public SpawningPoolController spawnPool;

    public List<GameObject> enemies;

    public float stoppingDistance, shootingRange, tresspassingRange;

    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, dieClip1, dieClip2, shootClip;

    Vector3 robotPos;

    private void Start()
    {
        unitManager = GetComponent<UnitManager>();	// fishy
        enemies = new List<GameObject>();
        spawnResources = GetComponent<SpawnRes>();	// fishy
        encampmentHandler = GetComponent<EncampmentHandler>();  // fishy
        robotPos = new Vector3(automoton.transform.position.x, 0, automoton.transform.position.z);
    }

    private void Update()
    {
        RunJobs();
    }

	//-----------------------------------------------------------------------------------------
	//-----------------------------------------------------------------------------------------

	/// <summary>
	/// MAIN FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void RunJobs()
    {
        foreach (GameObject e in enemies)
        {
            Enemy enemy_data = e.GetComponent<Enemy>();
            if (enemy_data.Health <= 0)
            {
                PlayClip(enemy_data.CampObj, "die");
                enemies.Remove(e);
                Destroy(e);
                enemy_data.CampData.OnField--; // fishy
                break;
            }
            if(enemy_data.CampData.EnemyJobs=="guard")
            {
                if (unitsTresspassing(enemy_data.CampObj) != null)
                {
                    if (enemy_data.Target == null)
                        TravelTo(e, unitsTresspassing(enemy_data.CampObj).transform.position, true, true);
                    else
                        AttackUnit(e);
                }
                else
                {
                    FindSpot(e);
                }
            }
            else if(enemy_data.CampData.EnemyJobs=="destroy")
            {
                if (enemy_data.Target == null)
                    AttackRobot(e);
                else
                    AttackUnit(e);
            }
            else
            {
                FindSpot(e);
            }
        }
    }

    GameObject unitsTresspassing(GameObject encampment)
    {
        foreach(GameObject u in unitManager.units)
        {
            if(Vector3.Distance(u.transform.position,encampment.transform.position)<tresspassingRange
                || Vector3.Distance(u.transform.position, encampment.GetComponent<Encampment>().ClosestResource.transform.position) < tresspassingRange)
            {
                return u;
            }
        }
        return null;
    }

	/*
	 * i was hoping i wouldn't have to loop through every friendly game object to see if they were in range...
	 * ...but you leave me no choice
     * if tresspassing, move to firing range
     * if in firing range and still in tresspassing, shoot and follow
     * if in firing range but not tresspassing, shoot
     * if not in firing range nor tresspassing, find spot
    */

	void ProtectEncampment(GameObject enemy)
    {
        AssignAnimation(enemy, "isShooting", false);
        FindSpot(enemy);
    }

    void AttackUnit(GameObject enemy)
    {
        Enemy enemy_data = enemy.GetComponent<Enemy>();
        if (enemy==null || enemy_data.Target == null || enemy_data.Target.GetComponent<Unit>().Health<=0)
		{
            AssignAnimation(enemy, "inCombat", false);
            enemy_data.Target = null;
            return;
        }
        AssignAnimation(enemy, "inCombat", true);

        if (enemy_data.Target != null)
        {
            //shooting
            if (!enemy_data.JustShot)
            {
                AssignAnimation(enemy, "firing", true);
                Fire(enemy, enemy_data);
            }
        }
    }

    public void AttackRobot(GameObject enemy_object)
    {
        if (enemy_object.GetComponent<Enemy>().Target == null)
        {
            TravelTo(enemy_object, robotPos, true, true);
        }
    }

	public void FindSpot(GameObject enemy)
	{
        if (Vector3.Distance(enemy.gameObject.transform.position, enemy.GetComponent<Enemy>().transform.position) > stoppingDistance)
        {

            int chance = Random.Range(0, 2);
            if (chance == 1)
            {
                Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
                TravelTo(enemy, enemy.GetComponent<Enemy>().Resource.transform.position + rand, true, false);
            }
            else
            {
                Vector3 rand = new Vector3(Random.Range(-3, 3), Random.Range(-3, 3), Random.Range(-3, 3));
                TravelTo(enemy, enemy.GetComponent<Enemy>().CampObj.transform.position + rand, true, false);
            }
        }
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void Fire(GameObject enemy, Enemy enemy_data)
	{
		if (enemy_data.Target != null && enemy_data.Target.GetComponent<Unit>().Health>0)
		{
            float hitChance = Random.Range(0f, 2f);

            Vector3 direction = enemy_data.Target.transform.position - enemy.transform.position;
			PlayClip(enemy_data.CampObj, "shoot");
            AssignAnimation(enemy, "firing", true);
            StartCoroutine(TrailOff(0.05f, enemy.transform.position, enemy_data.Target.transform.position));
	
			if (hitChance>0.5f)
			{
				enemy_data.Target.GetComponent<Unit>().Health--;
				unitManager.UnitDown(enemy_data.Target);

                if (enemy_data.Target.GetComponent<Unit>().Health <= 0)
                    enemy.GetComponent<Enemy>().Target = null;
            }

            StartCoroutine(FireCoolDown(hitChance, enemy_data));
        }
    }
	IEnumerator FireCoolDown(float extratime, Enemy enemy_data)
    {
        enemy_data.JustShot = true;
        yield return new WaitForSeconds(1f+extratime/2);
        enemy_data.JustShot = false;
    }

    void TravelTo(GameObject enemy, Vector3 place, bool stop, bool randomize)
    {
        if (enemy != null && enemy.GetComponent<NavMeshAgent>() != null)
        {
            Debug.Log("lets ago");
            NavMeshAgent nav = enemy.GetComponent<NavMeshAgent>();
            if (stop)
			{
				nav.stoppingDistance = this.stoppingDistance;
			}
            if (randomize)
			{
				place += new Vector3(Random.Range(-stoppingDistance, stoppingDistance), 0, Random.Range(-stoppingDistance, stoppingDistance));
			}
            AssignAnimation(enemy, "walking", true);
            nav.SetDestination(place);
        }
    }

    public GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);

        GameObject trail = spawnPool.poolDictionary["trails"].Dequeue();
        trail.transform.position = start - dif;
        trail.transform.rotation = angle * offset;
        trail.SetActive(true);
        //GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle * offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        spawnPool.poolDictionary["trails"].Enqueue(t);
        t.SetActive(false);
    }

    void PlayClip(GameObject encampment, string str)
    {
        AudioSource tempSource = encampment.GetComponent<AudioSource>();
        if (str.Equals("attack"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(attackClip1);
            else
                tempSource.PlayOneShot(attackClip2);
        }
        else if (str.Equals("die"))
        {
            if (Random.Range(0, 2) == 0)
                tempSource.PlayOneShot(dieClip1);
            else
                tempSource.PlayOneShot(dieClip2);
        }
        else if (str.Equals("shoot"))
            tempSource.PlayOneShot(shootClip);
    }
    void AssignAnimation(GameObject enemy, string anim, bool play)
    {
        if (enemy.GetComponent<Animator>() != null)
            enemy.GetComponent<Animator>().SetBool(anim, play);
    }

}
