using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhaseTwo : MonoBehaviour
{
    public bool endPhaseOne, startPhaseTwo;
    public GameObject automoton, fog;
    public Vector3 phasetwoPos;
    public float speed;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (endPhaseOne == true)
            StartCoroutine(RaiseAuto());
    }

    IEnumerator RaiseAuto()
    {
        endPhaseOne = false;
        //fog.SetActive(false);
        Debug.Log("before");
        yield return new WaitForSeconds(4f);
       // automoton.transform.position = Vector3.MoveTowards(automoton.transform.position, phasetwoPos, speed * Time.deltaTime);
        Debug.Log("after");
        startPhaseTwo = true;
    }

}
