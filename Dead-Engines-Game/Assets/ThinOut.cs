using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinOut : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Bumpable"))
		{
			Destroy(this.gameObject);
		}
	}

}
