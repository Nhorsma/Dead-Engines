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

	public List<GameObject> contentToClear = new List<GameObject>();

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
		
		foreach (Transform child in scrollContent.transform) // help from someone else on the unity forum!
		{
			contentToClear.Add(child.gameObject);
		}

		for (int i = 0; i < contentToClear.Count; i++)
		{
			Destroy(contentToClear[i]);
		}

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
