using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CactusSingleton : MonoBehaviour
{
    public static CactusSingleton instance;

    private CactusSingleton()
    {

    }

    public CactusSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new CactusSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
