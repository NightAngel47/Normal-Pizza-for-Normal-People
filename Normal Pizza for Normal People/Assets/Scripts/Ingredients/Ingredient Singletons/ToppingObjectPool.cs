/*
 * Normal Pizza for Normal People
 * IM 389
 * ToppingObjectPool
 * Sydney 
 * Create how the pools are handled. Creates pools of objects on start, sets one active when called, and add object back to pool when its done
 */

using System.Collections.Generic;
using UnityEngine;

public class ToppingObjectPool : MonoBehaviour
{
    public List<ToppingPools> pools;

    public Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        FillPoolsWithInactiveObjects();
    }

    // Fill the pools with inactive objects on Start()
    private void FillPoolsWithInactiveObjects()
    {
        foreach (ToppingPools pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {

                GameObject obj = Instantiate(pool.prefab);

                obj.SetActive(false);

                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.GetComponentInChildren<Renderer>().material = objectToSpawn.GetComponent<IngredientHitEffect>().mat;
        objectToSpawn.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        poolDictionary[tag].Enqueue(objectToSpawn);

        return objectToSpawn;
    }

    public void ReturnObjectToPool(string tag, GameObject objectToReturn)
    {
        objectToReturn.SetActive(false);

        poolDictionary[tag].Enqueue(objectToReturn);

    }
}
