using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishRandomStart : MonoBehaviour
{
    [SerializeField]
    float cycleStart = 0;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Animator>() != null)
        {
            GetComponent<Animator>().SetFloat("RandomStart", cycleStart);
        }
    }
}
