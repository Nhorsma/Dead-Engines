using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThinOut : MonoBehaviour
{

	private void OnTriggerEnter(Collider other)
	{

		if (other.gameObject.CompareTag("Metal"))
		{
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Electronics"))
		{
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Encampment"))
		{
			Destroy(this.gameObject);
		}
		if (other.gameObject.CompareTag("Robot"))
		{
			Destroy(this.gameObject);
		}
	}

}
