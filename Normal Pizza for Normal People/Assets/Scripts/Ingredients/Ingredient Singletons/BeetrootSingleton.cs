using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeetrootSingleton : MonoBehaviour
{
    public static BeetrootSingleton instance;

    private BeetrootSingleton()
    {

    }

    public BeetrootSingleton GetInstance()
    {
        if (instance == null)
        {
            instance = new BeetrootSingleton();
        }
        return instance;
    }

    public void RemoveInstance()
    {
        instance = null;
    }
}
