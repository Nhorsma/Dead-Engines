using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyHandler : MonoBehaviour
{
    SpawnRes spawnResources;
    UnitManager unitManager;
    EncampmentHandler encampmentHandler;

    public List<GameObject> enemies;

    public float stoppingDistance, shootingRange, tresspassingRange;

    public AudioSource audioSource;
    public AudioClip attackClip1, attackClip2, dieClip1, dieClip2, shootClip;

    private void Start()
    {
        unitManager = GetComponent<UnitManager>();	// fishy
        enemies = new List<GameObject>();
        spawnResources = GetComponent<SpawnRes>();	// fishy
        encampmentHandler = GetComponent<EncampmentHandler>();	// fishy
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
            if(e.GetComponent<Enemy>().Health <= 0)
            {
                //Debug.Log(e.GetComponent<Enemy>().Id);
                PlayClip(e.GetComponent<Enemy>().Camp, "die");
                enemies.Remove(e);
                Destroy(e);
                e.GetComponent<Enemy>().Camp.GetComponent<Encampment>().OnField--; // fishy
                break;
            }
            if(e.GetComponent<Enemy>().Target!=null)
            {
                Persue(e, e.GetComponent<Enemy>());
            }
            else
            {
                Protect(e);
            }
        }
    }

	/*
	 * i was hoping i wouldn't have to loop through every friendly game object to see if they were in range...
	 * ...but you leave me no choice
     * if tresspassing, move to firing range
     * if in firing range and still in tresspassing, shoot and follow
     * if in firing range but not tresspassing, shoot
     * if not in firing range nor tresspassing, find spot
    */
	void Protect(GameObject enemy)
    {
        foreach (GameObject u in unitManager.units)
        {
            if (Vector3.Distance(u.transform.position, enemy.GetComponent<Enemy>().Resource.transform.position) < tresspassingRange
                || Vector3.Distance(u.transform.position, enemy.GetComponent<Enemy>().Camp.transform.position) < tresspassingRange)
            {
				enemy.GetComponent<Enemy>().Target = u;
                TravelTo(enemy, u.transform.position, true, true);
                AssignAnimation(enemy, "isShooting", false);
                break;
            }
        }
    }

    void Persue(GameObject enemy, Enemy enemy_data)
    {
        if (enemy==null || enemy_data.Target == null)
		{
			return;
            AssignAnimation(enemy, "walking", false);
        }
        float dis = Vector3.Distance(enemy.transform.position, enemy_data.Target.transform.position);

        if(enemy_data.Target != null) //fishy
		{
            AssignAnimation(enemy, "inCombat", true);
			if (dis < shootingRange && !enemy_data.JustShot)
			{
				AssignAnimation(enemy, "firing", true);
                Fire(enemy, enemy_data);
				StartCoroutine(FireCoolDown(enemy_data));
			}
			else if (dis > shootingRange && dis < tresspassingRange)
			{
				AssignAnimation(enemy, "firing", false);
                TravelTo(enemy, enemy_data.Target.transform.position, true, true);
			}
			else if (dis > tresspassingRange && dis > shootingRange)
			{
				AssignAnimation(enemy, "firing", false);
                FindSpot(enemy);
				enemy_data.Target = null;
			}
		}
    }

	public void FindSpot(GameObject enemy)
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
			TravelTo(enemy, enemy.GetComponent<Enemy>().Camp.transform.position + rand, true, false);
		}
	}

	/// <summary>
	/// UTILITY FUNCTIONS --------------------------------------------------------------------------------------------------------------------------->
	/// </summary>

	void Fire(GameObject enemy, Enemy enemy_data)
	{
		if (enemy_data.Target != null)
		{
			Vector3 direction = enemy_data.Target.transform.position - enemy.transform.position;

			//    RaycastHit hit;
			//   if (Physics.Raycast(GetEnemyObject(ene).transform.position, direction, out hit, 100f))
			//   {
			PlayClip(enemy_data.Camp, "shoot");
			StartCoroutine(TrailOff(0.05f, enemy.transform.position, enemy_data.Target.transform.position));

			int hitChance = Random.Range(0, 2);
			if (hitChance == 0)
			{
				enemy_data.Target.GetComponent<Unit>().Health--;
				unitManager.UnitDown(enemy_data.Target);
			}
		}
	}
	IEnumerator FireCoolDown(Enemy enemy_data)
    {
        enemy_data.JustShot = true;
        yield return new WaitForSeconds(2f);
        enemy_data.JustShot = false;
    }

    void TravelTo(GameObject enemy, Vector3 place, bool stop, bool randomize)
    {
        if (enemy != null && enemy.GetComponent<NavMeshAgent>() != null)
        {
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

    GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);
        GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }
    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
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
