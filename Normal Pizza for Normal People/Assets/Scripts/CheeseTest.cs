using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheeseTest : MonoBehaviour
{
    public GameObject cheeseToSpawn;
    public GameObject linear;
    public GameObject cheeseOne;
    public GameObject cheeseTwo;
    public GameObject cheeseThree;

    public void Start()
    {
        cheeseToSpawn = cheeseOne;
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        { 
            if (linear.GetComponent<LinearMapping>().value >= .7f)
            {
                var spawnPos = pizza.transform.position;
                var newIngredient = Instantiate(cheeseToSpawn, spawnPos, pizza.transform.rotation, pizza.transform);
                newIngredient.transform.Rotate(Vector3.up, Random.Range(-180, 180), Space.Self);
                pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
            }
        }
    }

    public void SelectCheeseOne()
    {
        cheeseToSpawn = cheeseOne;
    }

    public void SelectCheeseTwo()
    {
        cheeseToSpawn = cheeseTwo;
    }

    public void SelectCheeseThree()
    {
        cheeseToSpawn = cheeseThree;
    }
}
