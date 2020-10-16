using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class CheeseTest : MonoBehaviour
{
    public GameObject cheese;
    public GameObject linear;

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        { 
            if (linear.GetComponent<LinearMapping>().value >= .7f)
            {
                var spawnPos = pizza.transform.position;
                var newIngredient = Instantiate(cheese, spawnPos, pizza.transform.rotation, pizza.transform);
                newIngredient.transform.Rotate(Vector3.up, Random.Range(-180, 180), Space.Self);
                pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
            }
        }
    }
}
