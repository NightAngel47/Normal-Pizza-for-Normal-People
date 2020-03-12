using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pizzaPrefab;
    [SerializeField]
    private Transform pizzaSpawnPos;

    public bool dont = false;

    public void SpawnPizza()
    {
        if (dont == false)
        {
            Instantiate(pizzaPrefab, pizzaSpawnPos.position, Quaternion.identity);
        }
    }
}
