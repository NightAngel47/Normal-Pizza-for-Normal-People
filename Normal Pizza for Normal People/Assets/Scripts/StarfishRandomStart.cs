using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarfishRandomStart : MonoBehaviour
{
    [SerializeField]
    float cycleStart = 0;
    [SerializeField]
    float animSpeed = 0.5f;
    private Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetFloat("RandomStart", cycleStart);
        anim.SetFloat("Speed", animSpeed);
        
    }
}
