//Object Pooling Example
//Typed up by Owen Schaffer
//Code for this example from https://www.youtube.com/watch?v=tdSmKaJvCoA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowPrefabSpawner : MonoBehaviour
{

    public GameObject prefabToSpawn;

    void FixedUpdate()
    {
        Instantiate(prefabToSpawn, transform.position, Quaternion.identity);
    }
}
