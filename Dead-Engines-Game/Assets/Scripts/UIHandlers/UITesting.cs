using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITesting : MonoBehaviour
{

	public GameObject room;
	private GameObject content;

	public Text textPrefab;

    void Start()
    {
		content = room.GetComponent<RoomComponents>().scroller.GetComponent<ScrollRect>().content.gameObject;
		Debug.Log("done");
    }

    void Update()
    {
        
    }
}
