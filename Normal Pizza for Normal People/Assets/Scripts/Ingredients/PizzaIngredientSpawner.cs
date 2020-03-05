using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PizzaIngredientSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pizzaIngredientToSpawn;

    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnIngredient());
    }

    private void OnTriggerExit(Collider col)
    {
        if (!col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient)) return;
        
        if (!isSpawning)
        {
            StartCoroutine(SpawnIngredient());
        }
    }
    
    private IEnumerator SpawnIngredient()
    {
        isSpawning = true;
        Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.5f);
        isSpawning = false;
    }
}
