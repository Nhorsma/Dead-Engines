using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPhaseTwo : MonoBehaviour
{
    public static bool endPhaseOne;
    public GameObject automoton, fog;
    public Vector3 phaseOnePos, phaseTwoPos;
    public Animation climbOut;
    public Animator anim;
    public AutomotonAction aa;

    void Start()
    {        
        automoton.transform.position = phaseOnePos;
        anim = automoton.GetComponent<Animator>();
        aa = automoton.GetComponent<AutomotonAction>();
        aa.enabled = false;
        endPhaseOne = false;
    }

    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            endPhaseOne = true;

        if (endPhaseOne == true)
            StartCoroutine(RaiseAuto());
    }
    

    IEnumerator RaiseAuto()
    {
        automoton.transform.position = phaseTwoPos;
        anim.SetBool("StartPhaseTwo", true);
        yield return new WaitForSeconds(1f);
        while (anim.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            yield return null;      
        endPhaseOne = false;
        anim.SetBool("StartPhaseTwo", false);
        aa.enabled = true;
    }

}
