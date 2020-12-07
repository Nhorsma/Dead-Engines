using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class UnitManager : MonoBehaviour
{
    public SelectItems si;
    public ResourceHandling rh;
    public EnemyHandler eh;
    public EncampmentHandler encampHandle;
    public Vector3 robotPos;
    public GameObject robot;
    public float stoppingDistance, pickUpDistance;
    public float shootingDistance;
    public float downTime;
    public int unitDamage;

    public GameObject[] unitsGM;         //the gameobjects
    public Unit[] units;                 //the 'unit' info attached to the gameobjects
    [System.NonSerialized]
    public static List<GameObject> selectedUnits;

    AudioSource audioSource;
    public AudioClip goingClip1, goingClip2, confirmPing, deadClip, shootClip,
                        dropOffClop, pickAxeClip;

    Color enemyRed = new Color32(207,67, 74, 100);
    Color resourceGreen = new Color32(69, 207, 69, 100);
    Color selectedYellow = new Color32(255, 255, 0, 100);
    NavMeshAgent nv;

    //public AutomatonUI auto;

    public float unitFireCooldown = 1f;

    //public EffectConnector effConnector;

    public GameObject unitPrefab;

    void Start()
    {
        eh = GetComponent<EnemyHandler>();
        encampHandle = GetComponent<EncampmentHandler>();
        selectedUnits = new List<GameObject>();
        robotPos = new Vector3(robot.transform.position.x,0, robot.transform.position.z);
        audioSource = Camera.main.GetComponent<AudioSource>();

        SetUpUnits(3);
        //auto.UpdateInfoTab();
    }

    void Update()
    {
        RightClick();
        RunAllJobs();
    }

    //-----------------------------------------------------------------------------------------
    //-----------------------------------------------------------------------------------------


    void SetUpUnits(int num)
    {
        unitsGM = new GameObject[num];
        units = new Unit[num];
        for (int i = 0; i < unitsGM.Length; i++)
        {
            units[i] = new Unit(i);
            Vector3 pos = FindSpotToSpawn();
            GameObject u = (GameObject)Instantiate(Resources.Load("unit"), pos, robot.transform.rotation);
            unitsGM[i] = u;
        }
    }


    RaycastHit Hit()
    {
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity))
            return hit;
        return hit;
    }

    void RightClick()
    {
        if (Input.GetMouseButtonDown(1) && Hit().point != null && selectedUnits.Count > 0)
        {
            //right click on anything that has units do a job
            if (Hit().collider != null &&
                Hit().collider.gameObject.tag == "Metal" ||
                Hit().collider.gameObject.tag == "Electronics" ||
                Hit().collider.gameObject.tag == "Enemy" ||
                Hit().collider.gameObject.tag == "Encampment")
            {
                SetJobOfSelected(Hit().collider.gameObject);
                si.UpdateUnitUI(GetUnit(selectedUnits[0])); ///////////////////////////////////////////////////////////////// //
            }
            else
            {
                MoveAllSelected();
            }
            ReadyClip();
        }
    }

    void MoveAllSelected()
    {
        if (selectedUnits.Count == 1)
        {
            ResetJob(GetUnit(selectedUnits[0]));
            TravelTo(selectedUnits[0], Hit().point, false, false);
        }
        else if (selectedUnits.Count > 0)
        {
            for (int i = 0; i < selectedUnits.Count; i++)
            {
                //
                //When more Room jobs are implemented, Fix This to have more Jobs
                //
                if (GetUnit(selectedUnits[i]).Job != "shrine" || GetUnit(selectedUnits[i]).Job != "study"
                    || GetUnit(selectedUnits[i]).Job != "refinery" || GetUnit(selectedUnits[i]).Job != "storage") // && selectedUnits[i].activeSelf
                {
                    ResetJob(GetUnit(selectedUnits[i]));

                    if (i == 0)
                    {
                        ResetJob(GetUnit(selectedUnits[i]));
                        TravelTo(selectedUnits[i], Hit().point, false, false);

                    }
                    else if (i > 0)
                    {
                        ResetJob(GetUnit(selectedUnits[i]));
                        Vector3 prevDes = selectedUnits[i - 1].GetComponent<NavMeshAgent>().destination;
                        Vector3 newDes = new Vector3();
                        if (i % 3 > 0)
                        {
                            newDes = prevDes + new Vector3(0, 0, 3);
                        }
                        else
                        {
                            newDes = selectedUnits[i - 3].GetComponent<NavMeshAgent>().destination + new Vector3(3, 0, 0);
                        }
                        TravelTo(selectedUnits[i], newDes, false, false);
                    }
                }
            }
        }
    }

    void RunAllJobs()
    {
        foreach (Unit unit in units)
        {
            Animator anim = GetUnitObject(unit).GetComponent<Animator>();
            if (unit.Job != "none")//&& !unit.JobPos.Equals(null)
            {
                if (unit.Job == "Combat")
                {
                    Combat(unit);
                }
                else if (unit.Job.Equals("ExtractionMetal") || unit.Job.Equals("ExtractionElectronics"))
                {
                    Extraction(unit, GetResourceID(unit.JobPos), unit.Job);///////////////////////////////////////////////////////////////////////
                }
                else if (unit.Job == "shrine" || unit.Job == "refinery" 
                    || unit.Job == "storage" || unit.Job == "study")
                {
                    float jx = GetUnitObject(unit).transform.position.x;
                    float jz = GetUnitObject(unit).transform.position.z;
                    if (Mathf.Abs(jx - unit.JobPos.transform.position.x) < 1f && Mathf.Abs(jz - unit.JobPos.transform.position.z) < 1f)
                    {
                        GetUnitObject(unit).SetActive(false);
                    }
                }
                else if (unit.Job == "dead")
                {
                    if (unit.CanSpawn)
                    {
                        unit.Health = 10;
                        unit.Job = "none";
                        GetUnitObject(unit).SetActive(true);
                        //TravelTo(GetUnitObject(unit), robotPos + new Vector3(-stoppingDistance*1.5f, 0, -stoppingDistance*1.5f), false, true);
                        GetUnitObject(unit).transform.position = robotPos + new Vector3(-stoppingDistance + Random.Range(-3, 3), 0, -stoppingDistance + Random.Range(-3, 3));
                        unit.CanSpawn = false;
                    }
                }
                else
                {
                    //
                    //When units stop having jobs in Robot, set them back to Active
                    //
                    // GetUnitObject(unit).SetActive(true);
                }
            }
            else
            {
                if (GetUnitObject(unit).GetComponent<NavMeshAgent>().velocity == new Vector3(0, 0, 0))
                    GetUnitObject(unit).GetComponent<Animator>().SetBool("walking", false);
            }
        }
    }

    void SetJobOfSelected(GameObject thing)
    {
        if (thing == null)
            return;

        if (thing.tag == "Metal" ||
            thing.tag == "Electronics")
        {
            int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);

                ResetJob(unit);
                unit.Job = ("Extraction" + thing.tag); //////////////////////////////////////////////////////////////////////////////////////////////
                unit.JobPos = (thing);
                unit.JustDroppedOff = (true);
              
                TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, false, false);
            }
        }
        if (thing.tag == "Enemy" || thing.tag == "Encampment")
        {
            //int ri = GetResourceID(thing);
            foreach (GameObject gm in selectedUnits)
            {
                Unit unit = GetUnit(gm);
                ResetJob(unit);
                unit.Job = "Combat";
                unit.JobPos = thing;
               
                TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true, true);
            }
        }
        PlayClip("ping");
    }

    int GetResourceID(GameObject gm)
    {
        for (int i = 0; i < rh.resDeposits.Length; i++)
        {
            if (rh.resDeposits[i] == gm)
                return i;
        }
        return -1;
    }

    public GameObject GetUnitObject(Unit unit)
    {
        return unitsGM[unit.Id];
    }

    public Unit GetUnit(GameObject gm)
    {
        for (int i = 0; i < unitsGM.Length; i++)
        {
            if (unitsGM[i] == gm)
                return units[i];
        }
        return null;
    }

    void Extraction(Unit unit, int ri, string resource)
    {
        if (unit.JobPos == null)
            return;

        GameObject gm = GetUnitObject(unit);
        Vector3 depPos = rh.resDeposits[ri].transform.position;
        Vector3 uPos = gm.transform.position;

        //reaches resource
        if (unit.JustDroppedOff && Vector3.Distance(gm.transform.position, unit.JobPos.transform.position) < pickUpDistance)
        {
            Extract(ri);
            unit.JustDroppedOff = false;
            TravelTo(gm, robotPos, false, false);
            //Debug.Log(unit.Job + " at " + unit.JobPos.transform.position);
        }
        else if (!unit.JustDroppedOff && Vector3.Distance(gm.transform.position, robotPos) < pickUpDistance) //reaches robot
        {
            //if (effConnector.StockCheck() == true)
            //{
            if (resource.Equals("ExtractionMetal"))
            {
                AddMetal();
                Debug.Log("Got metal");
            }
            else if (resource.Equals("ExtractionElectronics"))
            {
                AddElectronics();
                Debug.Log("Got electronics");
            }

            unit.JustDroppedOff = (true);
            TravelTo(gm, unit.JobPos.transform.position, false, false);
            //Debug.Log(unit.Job + " at "+unit.JobPos.transform.position);
            //}
            //else
            //{
            //	Debug.Log("No storage space");
            //	//make unit drop in unit info
            //}
        }
    }

    void Combat(Unit unit)
    {
        GameObject gm = GetUnitObject(unit);
        NavMeshAgent nv = GetUnitObject(unit).GetComponent<NavMeshAgent>();

        if (unit.JobPos != null &&
            Vector3.Distance(unit.JobPos.transform.position, GetUnitObject(unit).transform.position) > stoppingDistance)
            TravelTo(unitsGM[unit.Id], unit.JobPos.transform.position, true, true);

        if (unit.JobPos != null &&
            Vector3.Distance(gm.transform.position, unit.JobPos.transform.position) < shootingDistance
            && !unit.JustShot)
        {
            Fire(unit);
            StartCoroutine(FireCoolDown(unit));
        }
    }

    IEnumerator FireCoolDown(Unit unit)
    {
        unit.JustShot = true;
        yield return new WaitForSeconds(unitFireCooldown);
        unit.JustShot = false;
    }

    void Fire(Unit unit)
    {
        Vector3 direction = unit.JobPos.transform.position - GetUnitObject(unit).transform.position;
        PlayClip("shoot");
        //RaycastHit hit;
        //      if (Physics.Raycast(GetUnitObject(unit).transform.position, direction, out hit, 100f))
        //      {
        int hitChance = Random.Range(0, 2);
        if (hitChance == 0)
        {
            StartCoroutine(TrailOff(0.05f, GetUnitObject(unit).transform.position, unit.JobPos.transform.position));
            if (unit.JobPos.tag == "Enemy")
            {
                //access's the enemy via the enemyHandler, and reduces the enemie's health by one
                //Debug.Log("enemy : " + eh.GetEnemy(unit.JobPos));
                if (eh.GetEnemy(unit.JobPos) == null || eh.GetEnemy(unit.JobPos).Health <= 0)
                {
                    ResetJob(unit);
                }
                else
                {
                    eh.GetEnemy(unit.JobPos).Health -= unitDamage;
                }

                //Debug.Log("enemy: " + gameObject.GetComponent<EnemyHandler>().GetEnemy(hit.collider.gameObject).Health);
            }
            else if (unit.JobPos.tag == "Encampment")
            {
                gameObject.GetComponent<EncampmentHandler>().GetEncampment(unit.JobPos).Health -= unitDamage;
                if (encampHandle.GetEncampment(unit.JobPos).Health <= unitDamage)
                {
                    gameObject.GetComponent<EncampmentHandler>().BeDestroyed();
                    ResetJob(unit);
                }
                else
                {
                    gameObject.GetComponent<EncampmentHandler>().GetEncampment(unit.JobPos).Health -= unitDamage;
                }
                //Debug.Log("camp: "+gameObject.GetComponent<EncampmentHandler>().GetEncampment(hit.collider.gameObject).Health);
            }
        }
    }

    public void UnitDown(Unit unit)
    {
        if (unit.Health <= 0)
        {
            PlayClip("dead");
            GetUnitObject(unit).GetComponent<Animator>().SetBool("knockedOut", true);
            Debug.Log(unit + " is dead");
            unit.Job = "dead";
            unit.JobPos = null;
            if (selectedUnits.Contains(GetUnitObject(unit)))
            {
                si.RemoveSpecific(GetUnitObject(unit)); //
            }
            StartCoroutine(WaitToRespawn(unit));
        }
    }

    IEnumerator WaitToRespawn(Unit unit)
    {
        if (!unit.CanSpawn)
        {
//            yield return new WaitForSeconds(3f);
            GetUnitObject(unit).SetActive(false);
            GetUnitObject(unit).transform.position = robotPos;
            yield return new WaitForSeconds(downTime);
            GetUnitObject(unit).GetComponent<Animator>().SetBool("knockedOut", false);
            unit.CanSpawn = true;
            ReadyClip();
        }
    }

    public void TravelTo(GameObject b, Vector3 place, bool stop, bool randomize)
    {
        if (b.activeSelf == true && b.GetComponent<NavMeshAgent>() != null)
        {
            NavMeshAgent a = b.GetComponent<NavMeshAgent>();
            b.GetComponent<Animator>().SetBool("walking", true);
            if (stop)
                a.stoppingDistance = stoppingDistance;
            else
                a.stoppingDistance = 0f;
            if (randomize)
                place += new Vector3(Random.Range(-stoppingDistance / 2, stoppingDistance / 2), 0, Random.Range(-stoppingDistance / 2, stoppingDistance / 2));

            a.SetDestination(place);
        }

    }

    void Extract(int id)
    {
        //rh.resQuantities[id] -= 1;
        rh.Extract(id, 1);
        PlayClip("pickaxe");
    }

    void AddMetal()
    {
        ResourceHandling.metal++;
        PlayClip("drop");
    }

    void AddElectronics()
    {
        ResourceHandling.electronics++;
        PlayClip("drop");
    }

    public Unit ReturnJoblessUnit()
    {
        foreach (Unit u in units)
        {
            if (u.Job == "none")
            {
                return u;
            }
        }
        return null;
    }

    public void SetJobFromRoom(Unit unit, string roomJob)
    {
        PlayClip("ping");
        TravelTo(GetUnitObject(unit), robotPos/*unit.JobPos.transform.position*/, false, false);
    }

    public void LeaveRoomJob(Unit unit)
    {
        ReadyClip();
        ResetJob(unit);

        if (!AutomotonAction.endPhaseOne)
        {
            GetUnitObject(unit).transform.position = FindSpotToSpawn();
            GetUnitObject(unit).SetActive(true);
        }
    }

    GameObject BulletTrail(Vector3 start, Vector3 end)
    {
        float x, y, z;
        x = Random.Range(-1.2f, 1.2f);
        y = Random.Range(-1.2f, 1.2f);
        z = Random.Range(-1.2f, 1.2f);
        Quaternion offset = Quaternion.Euler(x, y, z);

        Vector3 dif = (start - end) / 2;
        Quaternion angle = Quaternion.LookRotation(start - end);
        GameObject trail = (GameObject)Instantiate(Resources.Load("BulletTrail"), start - dif, angle * offset);

        trail.transform.localScale = new Vector3(0.05f, 0.05f, Vector3.Distance(start, end));
        return trail;
    }

    IEnumerator TrailOff(float time, Vector3 start, Vector3 end)
    {
        GameObject t = BulletTrail(start, end);
        yield return new WaitForSeconds(time);
        Destroy(t);
    }

    public void FindUnit(Unit unit)
    {
        Debug.Log("No one is here");
    }

    public Vector3 FindSpotToSpawn()
    {
        return robotPos + new Vector3(-pickUpDistance + Random.Range(-3, 3), 0, -pickUpDistance + Random.Range(-3, 3));
    }

    void ReadyClip()
    {
            if (!audioSource.isPlaying)
                if (Random.Range(0, 2) == 0)
                {
                     audioSource.PlayOneShot(goingClip1);
                }
                else
                {
                    audioSource.PlayOneShot(goingClip2);
                }
    }

    void PlayClip(string str)
    {
        if (str.Equals("ping"))
        {
            audioSource.PlayOneShot(confirmPing);
            if (Random.Range(0, 2) == 0)
            {
                audioSource.PlayOneShot(goingClip1);
            }
            else
            {
                audioSource.PlayOneShot(goingClip2);
            }
        }
        else if (str.Equals("drop"))
        {
            audioSource.PlayOneShot(dropOffClop);
        }
        else if (str.Equals("pickaxe"))
        {
            audioSource.PlayOneShot(pickAxeClip);
        }
        else if (str.Equals("shoot"))
        {
            audioSource.PlayOneShot(shootClip);
        }
        else if(str.Equals("dead"))
        {
            audioSource.PlayOneShot(deadClip);
        }
    }

    public void SetJobCircleColor(Unit unit, Color colorChange)
    {
        if (unit.JobPos != null && unit.JobPos.GetComponentInChildren<SpriteRenderer>() != null)
        {
            unit.JobPos.GetComponentInChildren<SpriteRenderer>().color = colorChange;
        }
    }

    public void ResetColor(Unit unit)
    {
        if(unit.Job == "Combat")
            SetJobCircleColor(unit, enemyRed);
        else if (unit.JobPos != null && (unit.JobPos.tag == "Metal" || unit.JobPos.tag == "Electronics"))
            SetJobCircleColor(unit, resourceGreen);
    }

    void ResetJob(Unit unit)
    {
        ResetColor(unit);
        unit.Job = "none";
        unit.JobPos = null;
    }


    public void PhaseTwoUnits()
    {
        foreach(GameObject unitObject in unitsGM)
        {
            unitObject.SetActive(false);
        }
    }
}