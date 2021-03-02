using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptLoader : MonoBehaviour
{

	// DO NOT TOUCH THIS SCRIPT!

	public StartTilePlacement startTilePlacement;
	public SpawnRes spawnRes;
	public GameObject block1, block2, block3, block_test;
	public AutomatonUI automatonUI;
	public GameObject automatonObj;
	//public AutomotonAction automatonAction;

	void Start()
    {
		StartCoroutine(Buffer());
    }

	IEnumerator Buffer()
	{
		Debug.Log("start loading");

		startTilePlacement.enabled = true;
		Debug.Log(automatonObj.transform.position);

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		spawnRes.enabled = true;

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		block1.SetActive(true);

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		block2.SetActive(true);
		automatonUI.enabled = true;

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		block3.SetActive(true);

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		block_test.SetActive(true);

		Debug.Log("start buffer");
		yield return new WaitForSeconds(1f);
		Debug.Log("buffer complete");

		spawnRes.TurnOffThinners();

		Debug.Log("end loading");
	}

}
