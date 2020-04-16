//Object Pooling Example
//Typed up by Owen Schaffer
//Code for this example from https://www.youtube.com/watch?v=tdSmKaJvCoA

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FastPrefabSpawnerWithObjPooling : MonoBehaviour
{

    //We could call our ObjectPooler Singleton like this
    /*
    void FixedUpdate()
    {
        ObjectPooler.instance.SpawnFromPool("Sphere", transform.position, Quaternion.identity);
    }
    */

    //But creating a reference to our ObjectPooler may be faster in terms of performance
    ObjectPooler objectPooler;

    private void Start()
    {
        objectPooler = ObjectPooler.instance;
    }

    void FixedUpdate()
    {
        objectPooler.SpawnFromPool("Sphere", transform.position, Quaternion.identity);
    }
}
