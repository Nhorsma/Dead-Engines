using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawningPoolController : MonoBehaviour
{
    public Dictionary<string, Queue<GameObject>> poolDictionary;
    public List<Pool> pools;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        SetUpPools();
    }

    void SetUpPools()
    {
        Queue<GameObject> tempPool = new Queue<GameObject>();

        foreach (Pool pool in pools)
        {
            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.SetActive(false);
                tempPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, tempPool);
        }
    }
}
