using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneycombSingleton : MonoBehaviour
{
    public static HoneycombSingleton instance;

    private HoneycombSingleton()
    {

    }

    public HoneycombSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new HoneycombSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
