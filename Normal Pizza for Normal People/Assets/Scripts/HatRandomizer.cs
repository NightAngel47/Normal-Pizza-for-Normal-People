using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Randomzies what hat a customer wears
public class HatRandomizer : MonoBehaviour
{
    [SerializeField]
    GameObject[] hats;
    // Start is called before the first frame update
    void Start()
    {
        int hat = Random.Range(0, hats.Length+1);
        if(hat < hats.Length)
        {
            hats[hat].SetActive(true);
        }
    }
}
