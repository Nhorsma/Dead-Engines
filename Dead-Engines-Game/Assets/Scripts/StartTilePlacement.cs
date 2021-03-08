using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTilePlacement : MonoBehaviour
{

	public GameObject plane;
	public List<GameObject> blocks = new List<GameObject>();

	public List<GameObject> polymorphicEverything = new List<GameObject>();
	public SpawnRes spawnRes;

    void Awake()
    {
		SetTilePosition();
		PlaceObjects();
    }

    void Update()
    {
        
    }

	public void SetTilePosition()
	{
		int r = Random.Range(0, 12);
		plane.transform.position = new Vector3(blocks[r].transform.position.x, -7f, blocks[r].transform.position.z);
	}

	public void PlaceObjects()
	{
		polymorphicEverything[0].transform.position = new Vector3(plane.transform.position.x, plane.transform.position.y - 39.3f, plane.transform.position.z);
	}
}
