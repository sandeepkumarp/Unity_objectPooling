using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for using dictionary and queues
public class PoolManager : MonoBehaviour
{
    Dictionary<int, Queue<GameObject>> poolDictionary = new Dictionary<int, Queue<GameObject>>();   //Dictionary is Generic which stores Key-Value-Pairs 

    static PoolManager _instance;

    public static PoolManager instance              //implementing singleton pattern
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<PoolManager>();
            }
            return _instance;
        }
    }

    public void CreatePool(GameObject prefab, int poolSize)
    {
        int poolKey = prefab.GetInstanceID();           // reference id for prefab for which pool is being made
        if (!poolDictionary.ContainsKey(poolKey))       //if none exists
        {
            poolDictionary.Add(poolKey, new Queue<GameObject>());           //make one

            for (int i = 0; i < poolSize; i++)                              //instantiate prefabs and fill the pool
            {
                GameObject newObject = Instantiate(prefab) as GameObject;
                newObject.SetActive(false);                                 //to keep them hidden
                poolDictionary[poolKey].Enqueue(newObject);                 //add the new object to the pool
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            GameObject objectToReuse = poolDictionary[poolKey].Dequeue();   //remove from top
            poolDictionary[poolKey].Enqueue(objectToReuse);                 //insterted at bottom
            objectToReuse.SetActive(true);                                  //make it visible
            objectToReuse.transform.position = position;
            objectToReuse.transform.rotation = rotation;
        }
    }
}
