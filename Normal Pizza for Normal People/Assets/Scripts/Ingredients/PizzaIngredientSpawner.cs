using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class PizzaIngredientSpawner : MonoBehaviour
{
    [SerializeField]
    public GameObject pizzaIngredientToSpawn;

    public GameObject singleton;

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
            StartCoroutine(nameof(SpawnIngredient));
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            hasIngredient = false;

            switch (pizzaIngredientToSpawn.name)
            {
                case "Pineapple Topping Interactable":
                    gameObject.GetComponent<PineappleSingleton>().RemoveInstance();
                    break;

                case "Tidepod Topping Interactable":
                    gameObject.GetComponent<TidepodSingleton>().RemoveInstance();
                    break;

                case "Starfish Topping Interactable":
                    gameObject.GetComponent<StarfishSingleton>().RemoveInstance();
                    break;

                case "Honeycomb Topping Interactable":
                    gameObject.GetComponent<HoneycombSingleton>().RemoveInstance();
                    break;

                case "Beetroot Topping Interactable":
                    gameObject.GetComponent<BeetrootSingleton>().RemoveInstance();
                    break;

                case "Cactus Topping Interactable":
                    gameObject.GetComponent<CactusSingleton>().RemoveInstance();
                    break;
            }
        }
    }
    
    private IEnumerator SpawnIngredient()
    {
        hasIngredient = true;
        temp = Instantiate(pizzaIngredientToSpawn, transform.position, Quaternion.identity);

        switch (pizzaIngredientToSpawn.name)
        {
            case "Pineapple Topping Interactable":
                gameObject.GetComponent<PineappleSingleton>().GetInstance();
                break;

            case "Tidepod Topping Interactable":
                gameObject.GetComponent<TidepodSingleton>().GetInstance();
                break;

            case "Starfish Topping Interactable":
                gameObject.GetComponent<StarfishSingleton>().GetInstance();
                break;

            case "Honeycomb Topping Interactable":
                gameObject.GetComponent<HoneycombSingleton>().GetInstance();
                break;

            case "Beetroot Topping Interactable":
                gameObject.GetComponent<BeetrootSingleton>().GetInstance();
                break;

            case "Cactus Topping Interactable":
                gameObject.GetComponent<CactusSingleton>().GetInstance();
                break;
        }

        yield return new WaitForEndOfFrame();
    }
}
