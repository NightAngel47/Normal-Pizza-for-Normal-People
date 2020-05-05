/*
 * Normal Pizza for Normal People
 * IM 389
 * ToppingPools
 * Sydney 
 * Sets up the object pool
 */

using System;
using UnityEngine;

[Serializable]
public class ToppingPools
{
    //All of these need to be set in the inspector for each pool
    public string tag;
    public GameObject prefab;
    public int size;
}
