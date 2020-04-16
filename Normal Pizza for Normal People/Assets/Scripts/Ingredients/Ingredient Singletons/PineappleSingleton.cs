using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleSingleton : Singleton
{
    public static PineappleSingleton instance;

    public PineappleSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = gameObject.AddComponent<PineappleSingleton>();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
        Destroy(gameObject.GetComponent<PineappleSingleton>());
    }
}
