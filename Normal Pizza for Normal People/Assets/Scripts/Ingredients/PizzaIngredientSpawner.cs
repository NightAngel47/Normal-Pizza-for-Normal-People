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

    private bool handInside = false;

    private GameObject temp;

    private void Start()
    {
        //StartCoroutine(SpawnIngredient());
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.GetComponentInParent<HandCollider>() && isSpawning == false)
        {
            isSpawning = true;
            handInside = true;
            StartCoroutine("SpawnIngredient");
        }
    }

    private void OnTriggerStay(Collider col)
    {
        if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient) && handInside == false)
        {
            Destroy(temp);
        }

        //if(col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        //{
        //    //isSpawning = false;
        //    inContainer = true;
        //}
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.GetComponentInParent<HandCollider>() && col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient) && isSpawning == true)
        {
            isSpawning = false;
            handInside = false;
        }

        if(col.GetComponentInParent<HandCollider>())
        {
            handInside = false;
        }
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
        temp = Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(0.25f);
        //isSpawning = false;
    }
}
