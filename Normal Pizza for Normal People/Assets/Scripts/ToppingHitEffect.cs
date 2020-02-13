using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem.Sample;

public class ToppingHitEffect : MonoBehaviour
{
    public Collider pizzaCollider;

    public GameObject spawnObjectOnCollision;

    public bool destroyOnTargetCollision = true;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent.TryGetComponent(out PizzaScript pizza))
        {
            pizzaCollider = collision.gameObject.GetComponent<MeshCollider>();
            audioSource.Play();
        }

        if (collision.collider == pizzaCollider)
        {
            ContactPoint contact = collision.contacts[0];
            RaycastHit hit;

            float backTrackLength = 1f;
            Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
            if (collision.collider.Raycast(ray, out hit, 2))
            {
                if (ToppingSpawner.pickedUp == false)
                {
                    float randx = Random.Range(-.2f, .28f);
                    float randz = Random.Range(-.128f, .259f);

                    GameObject spawned = Instantiate(spawnObjectOnCollision);
                    spawned.transform.SetParent(pizzaCollider.transform);
                    spawned.transform.rotation = Quaternion.Euler(0, 0, 0);
                    spawned.transform.localPosition = new Vector3(randx, 3f, randz);
                }
            }
            
            //Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

            if (destroyOnTargetCollision)
                Destroy(this.gameObject);
        }
    }
}
