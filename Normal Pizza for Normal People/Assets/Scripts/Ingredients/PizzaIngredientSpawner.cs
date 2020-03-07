﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaIngredientSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pizzaIngredientToSpawn;

    private bool hasIngredient = false;

    private bool isSpawning = false;

    private bool handInside = false;

    private GameObject temp;

    private void Start()
    {
        //StartCoroutine(SpawnIngredient());
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            hasIngredient = true;
        }

        if(col.GetComponentInParent<HandCollider>() && hasIngredient == false)
        {
            StartCoroutine("SpawnIngredient");
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            hasIngredient = false;
        }
    }
    
    private IEnumerator SpawnIngredient()
    {
        hasIngredient = true;
        temp = Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);
        yield return new WaitForEndOfFrame();
    }
}
