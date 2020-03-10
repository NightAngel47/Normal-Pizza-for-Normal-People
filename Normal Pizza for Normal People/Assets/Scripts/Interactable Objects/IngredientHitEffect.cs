using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        var pizza = collision.collider.GetComponentInParent<PizzaBehaviour>();

        if (!pizza) return;
        ContactPoint contact = collision.contacts[0];

        float backTrackLength = 1f;
        Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
        if (collision.collider.Raycast(ray, out RaycastHit hit, 2))
        {
            var pizzaTransform = pizza.transform;
            var spawnPos = pizzaTransform.position;
            var randRot = pizzaTransform.rotation.eulerAngles;
            randRot += new Vector3(0, Random.Range(-180f, 180f) - randRot.y, 0);
            var newIngredient = Instantiate(spawnObjectOnCollision, spawnPos, Quaternion.Euler(randRot), pizza.transform);
            pizza.AddPizzaIngredient(newIngredient.GetComponent<PizzaIngredient>());
        }

        Destroy(gameObject);
    }
}
