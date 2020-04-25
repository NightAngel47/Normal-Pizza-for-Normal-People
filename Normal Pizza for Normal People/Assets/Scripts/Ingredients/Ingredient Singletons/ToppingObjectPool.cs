using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToppingObjectPool : MonoBehaviour
{
    // A list of Pool objects based on the Pool class
    public List<ToppingPools> pools;

    // A Dictionary with a string as the Key and a Queue of GameObjects as the values
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
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
            // Create a queue of game objects to hold the prefabs in the pool
            Queue<GameObject> objectPool = new Queue<GameObject>();

            // For each prefab up to the size of the pool (set in inspector)...
            for (int i = 0; i < pool.size; i++)
            {
                // Instantiate the prefab (also set in inspector) and assign it to obj
                GameObject obj = Instantiate(pool.prefab);
                // Set obj as inactive
                obj.SetActive(false);
                // Enqueue or add obj to the queue of objects (to the back of the line)
                objectPool.Enqueue(obj);
            }
            // Add the queue of objects to the dictionary of pools with that pool's 
            // string tag (set in inspector) as a label
            poolDictionary.Add(pool.tag, objectPool);
        }
    }


    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        // Check to make sure the Dictionary contains the pool with the tag passed into SpawnFromPool
        if (!poolDictionary.ContainsKey(tag))
        {
            // If you get this error, be sure you set the Pool's tag correctly in the inspector
            Debug.LogWarning("Pool with tag " + tag + " doesn't exist.");
            return null;
        }

        // Dequeue or remove the object to spawn (from the front of the line)
        GameObject objectToSpawn = poolDictionary[tag].Dequeue();

        // Set object to spawn to active
        objectToSpawn.SetActive(true);
        // Set the position and rotation of the object to what was passed into SpawnFromPool()
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        objectToSpawn.GetComponentInChildren<Renderer>().material = objectToSpawn.GetComponent<IngredientHitEffect>().mat;
        objectToSpawn.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

        // Add the object back to the queue of objects (to the back of the line)
        poolDictionary[tag].Enqueue(objectToSpawn);

        // Return the object to spawn
        return objectToSpawn;
    }

    public void ReturnObjectToPool(string tag, GameObject objectToReturn)
    {
        // Set obj as inactive
        objectToReturn.SetActive(false);

        // Add the object back to the queue of objects (to the back of the line)
        poolDictionary[tag].Enqueue(objectToReturn);

    }
}
