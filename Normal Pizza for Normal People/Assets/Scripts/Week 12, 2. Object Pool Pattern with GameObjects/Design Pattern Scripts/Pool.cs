//Object Pooling Example
//Typed up by Owen Schaffer
//Code for this example from https://www.youtube.com/watch?v=tdSmKaJvCoA
using UnityEngine;
using System.Collections;

[System.Serializable]
public class Pool
{

    //All of these need to be set in the inspector for each pool
    public string tag;
    public GameObject prefab;
    public int size;
    
}
