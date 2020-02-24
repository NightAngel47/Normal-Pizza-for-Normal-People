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

    public void SpawnPizza()
    {
        Instantiate(pizzaPrefab, pizzaSpawnPos.position, Quaternion.identity);
    }
}
