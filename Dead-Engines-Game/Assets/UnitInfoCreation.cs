using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitInfoCreation : MonoBehaviour
{
	public UnitManager unitManager;
	public GameObject id_prefab;
	public GameObject health_prefab;
	public GameObject job_prefab;
	public GameObject scrollContent;

    void Start()
    {
		UpdateUnitInfo();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void UpdateUnitInfo()
	{
		foreach (GameObject u in unitManager.units)
		{
			var id = Instantiate(id_prefab, scrollContent.transform);
			id.GetComponent<Text>().text = u.GetComponent<Unit>().Id.ToString();
			var health = Instantiate(health_prefab, scrollContent.transform);
			health.GetComponent<Text>().text = u.GetComponent<Unit>().Health.ToString();
			var job = Instantiate(job_prefab, scrollContent.transform);
			job.GetComponent<Text>().text = u.GetComponent<Unit>().Job;
		}
	}
}
