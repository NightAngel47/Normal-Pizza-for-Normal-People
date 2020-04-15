using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishSingleton : MonoBehaviour
{
    public static StarfishSingleton instance;

    private StarfishSingleton()
    {

    }

    public StarfishSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new StarfishSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
