using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuFlare : MonoBehaviour
{
	public Light flash;
	public float flashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		flash.intensity += 1 * flashSpeed;
		if (flash.intensity > 1.75f)
		{
			flash.intensity = 1.7f;
		}
    }
}
