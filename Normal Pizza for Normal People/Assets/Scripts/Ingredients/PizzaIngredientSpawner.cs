using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaIngredientSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject pizzaIngredientToSpawn;

    private bool inContainer = false;

    private bool isSpawning = false;

    private void Start()
    {
        StartCoroutine(SpawnIngredient());
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.GetComponentInParent<HandCollider>())
        {
            StartCoroutine("SpawnIngredient");
        }
    }

    private void OnTriggerStay(Collider col)
    {
        //if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        //{
        //    //isSpawning = false;
        //    inContainer = true;
        //}
    }

    private void OnTriggerExit(Collider col)
    {
        ////if (!col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient)) return;

        //if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        //{
        //    inContainer = false;

        //    if (!isSpawning && inContainer == false)
        //    {
        //        StartCoroutine(SpawnIngredient());
        //    }
        //}
    }
    
    private IEnumerator SpawnIngredient()
    {
        isSpawning = true;
        Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.25f);
        isSpawning = false;
    }
}
