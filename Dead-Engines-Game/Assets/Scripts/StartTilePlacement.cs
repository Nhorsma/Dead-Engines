using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTilePlacement : MonoBehaviour
{

	public GameObject plane;
	public List<GameObject> blocks = new List<GameObject>();

	public List<GameObject> polymorphicEverything = new List<GameObject>();
	//public SpawnRes spawnRes;

    void Awake()
    {
		SetTilePosition();
		//PlaceObjects();
    }

    void Update()
    {
        
    }

	public void SetTilePosition()
	{
		int r = Random.Range(0, 12);
		plane.transform.position = new Vector3(blocks[r].transform.position.x, -0.09f, blocks[r].transform.position.z);
		//spawnRes.enabled = true;
	}

	//public void PlaceObjects()
	//{
	//	polymorphicEverything[0].transform.position = plane.transform.position;

	//	//polymorphicEverything[1].transform.position = new Vector3(plane.transform.position.x+10, plane.transform.position.y, plane.transform.position.z+10);
	//	//polymorphicEverything[2].transform.position = new Vector3(plane.transform.position.x + 12, plane.transform.position.y, plane.transform.position.z + 12);
	//	//polymorphicEverything[3].transform.position = new Vector3(plane.transform.position.x + 14, plane.transform.position.y, plane.transform.position.z + 14);
	//}
}
