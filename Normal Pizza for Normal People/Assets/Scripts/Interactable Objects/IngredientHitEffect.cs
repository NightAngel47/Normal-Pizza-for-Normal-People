using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IngredientHitEffect : MonoBehaviour
{
    public GameObject spawnObjectOnCollision;

    private void OnCollisionEnter(Collision collision)
    {
        //TODO change PizzaScript to whatever new script will be
        if (collision.collider.GetComponentInParent<PizzaScript>())
        {
            ContactPoint contact = collision.contacts[0];
            RaycastHit hit;

            float backTrackLength = 1f;
            Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
            if (collision.collider.Raycast(ray, out hit, 2))
            {
                //TODO modify to work better
                Instantiate(spawnObjectOnCollision);
            }

            Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

            Destroy(this.gameObject);
        }
    }
}
