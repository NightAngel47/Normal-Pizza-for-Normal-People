using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        var pizza = collision.collider.GetComponentInParent<PizzaBehaviour>();
        if (pizza)
        {
            ContactPoint contact = collision.contacts[0];
            RaycastHit hit;

            float backTrackLength = 1f;
            Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
            if (collision.collider.Raycast(ray, out hit, 2))
            {
                var newIngredient = Instantiate(spawnObjectOnCollision, pizza.transform);
                pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

            Destroy(gameObject);
        }
    }
}
