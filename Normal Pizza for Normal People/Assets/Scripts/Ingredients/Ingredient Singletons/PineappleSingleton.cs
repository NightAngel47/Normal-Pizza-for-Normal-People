using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PineappleSingleton 
{
    public static PineappleSingleton instance;

    private PineappleSingleton()
    {

    }

    public PineappleSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new PineappleSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
