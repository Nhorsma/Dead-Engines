using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageCheck : MonoBehaviour
{
	public RoomManager roomManager;
	public int storageSpace;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	//update storageSpace
	//should refining perform CanStore()?
	public bool CanStore()
	{
		foreach (Storage s in roomManager.rooms)
		{
			if (s.Level == 1)
			{
				storageSpace += 200;
			}
			else if (s.Level == 2)
			{
				storageSpace += 250;
			}
			else if (s.Level == 3)
			{
				storageSpace += 500;
			}
			storageSpace -= s.Stored.Count;
		}

		if (storageSpace > 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
