using UnityEngine;
using System.Collections;
using System.Collections.Generic; //for using dictionary and queues
public class PoolManager : MonoBehaviour
{
    Dictionary<int, Queue<ObjectInstance>> poolDictionary = new Dictionary<int, Queue<ObjectInstance>>();   //Dictionary is Generic which stores Key-Value-Pairs 

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

        GameObject poolHolder = new GameObject(prefab.name + " Pool");
        poolHolder.transform.parent = transform;

        if (!poolDictionary.ContainsKey(poolKey))       //if none exists
        {
            poolDictionary.Add(poolKey, new Queue<ObjectInstance>());           //make one

            for (int i = 0; i < poolSize; i++)                              //instantiate prefabs and fill the pool
            {
                ObjectInstance newObject = new ObjectInstance(Instantiate(prefab) as GameObject);
                //newObject.SetActive(false);                                 //to keep them hidden
                poolDictionary[poolKey].Enqueue(newObject);                 //add the new object to the pool
                newObject.SetParent(poolHolder.transform);
            }
        }
    }

    public void ReuseObject(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        int poolKey = prefab.GetInstanceID();

        if (poolDictionary.ContainsKey(poolKey))
        {
            ObjectInstance objectToReuse = poolDictionary[poolKey].Dequeue();   //remove from top
            poolDictionary[poolKey].Enqueue(objectToReuse);                 //insterted at bottom
            objectToReuse.Reuse(position, rotation);
        }
    }

    public class ObjectInstance
    {

        GameObject gameObject;
        Transform transform;

        bool hasPoolObjectComponent;
        PoolObject poolObjectScritp;

        //gameObject.SetActive(false);

        public ObjectInstance(GameObject objectInstance)
        {
            gameObject = objectInstance;
            transform = gameObject.transform;
            gameObject.SetActive(false);

            if (gameObject.GetComponent<PoolObject>())
            {
                hasPoolObjectComponent = true;
                poolObjectScritp = gameObject.GetComponent<PoolObject>();
            }
        }

        public void Reuse(Vector3 position, Quaternion rotation)
        {
            if (hasPoolObjectComponent)
            {
                poolObjectScritp.OnObjectReuse();
            }
            gameObject.SetActive(true);                                  //make it visible
            transform.position = position;
            transform.rotation = rotation;
        }

        public void SetParent(Transform parent)
        {
            transform.parent = parent;
        }
    }
}
