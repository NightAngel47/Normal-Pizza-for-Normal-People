using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TidepodSingleton : MonoBehaviour
{
    public static TidepodSingleton instance;

    private TidepodSingleton()
    {

    }

    public TidepodSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new TidepodSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
