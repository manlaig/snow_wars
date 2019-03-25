using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [SerializeField] GameObject objectToPool;
    [SerializeField] int amountToPool = 10;
    List<GameObject> pool;

    void Start()
    {
        pool = new List<GameObject>();
        FillPool();
    }

    void FillPool()
    {
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject go = Instantiate(objectToPool);
            go.SetActive(false);
            pool.Add(go);
        }
    }
	
    public GameObject Get()
    {
        if(pool.Count == 0)
        {
            FillPool();
        }
        GameObject ins = pool[pool.Count - 1];
        ins.SetActive(true);
        pool.RemoveAt(pool.Count - 1);
        return ins;
	}

    public void Putback(GameObject obj)
    {
        obj.SetActive(false);
        pool.Add(obj);
    }
}
