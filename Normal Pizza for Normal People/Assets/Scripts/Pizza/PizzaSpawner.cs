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
    [SerializeField]
    private Transform pizzaSpawnPos2;

    private bool spawnRight = false;

    public void SpawnPizza()
    {
        if (spawnRight == false)
        {
            Instantiate(pizzaPrefab, pizzaSpawnPos.position, Quaternion.identity);
            spawnRight = true;
        }

        else if(spawnRight == true)
        {
            Instantiate(pizzaPrefab, pizzaSpawnPos2.position, Quaternion.identity);
            spawnRight = false;
        }
    }
}
