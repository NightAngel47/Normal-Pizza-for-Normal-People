//======= Copyright (c) Valve Corporation, All rights reserved. ===============

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Valve.VR.InteractionSystem.Sample
{
    public class TargetHitEffect1 : MonoBehaviour
    {
        public Collider targetCollider1;
        public Collider targetCollider2;

        private GameObject[] g = new GameObject[5];

        public GameObject spawnObjectOnCollision;

        public bool colorSpawnedObject = true;

        public bool destroyOnTargetCollision = true;

        private AudioSource audioSource;

        private void Start()
        {
            //targetCollider1 = GameObject.FindGameObjectWithTag("pizza1").GetComponentInChildren<MeshCollider>();
            //targetCollider2 = GameObject.FindGameObjectWithTag("pizza2").GetComponentInChildren<MeshCollider>();
            audioSource = gameObject.GetComponent<AudioSource>();
        }

        private void OnCollisionEnter(Collision collision)
        {
            //print(collision.gameObject.transform.GetChild(0).tag);
            if (collision.gameObject.transform.GetChild(0).CompareTag("pizza") == true)
            {
                targetCollider1 = collision.gameObject.transform.GetChild(0).GetComponent<MeshCollider>();
                audioSource.Play();
            }

            //if (collision.transform.GetChild(0).CompareTag("pizza2"))
            //{
            //    targetCollider2 = collision.transform.GetChild(0).GetComponent<MeshCollider>();
            //}

            if (collision.collider == targetCollider1)
            {
                ContactPoint contact = collision.contacts[0];
                GameObject target = collision.gameObject;
                RaycastHit hit;

                float backTrackLength = 1f;
                Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
                if (collision.collider.Raycast(ray, out hit, 2))
                {
                    if (colorSpawnedObject && ToppingSpawner.pickedUp == false)
                    {
                        float randx = Random.Range(-.2f, .28f);
                        float randz = Random.Range(-.128f, .259f);

                        GameObject spawned = GameObject.Instantiate(spawnObjectOnCollision);
                        spawned.transform.SetParent(targetCollider1.transform);
                        spawned.transform.rotation = Quaternion.Euler(0, 0, 0);
                        spawned.transform.localPosition = new Vector3(randx, 3f, randz);
                        //spawned.transform.localScale = new Vector3(3.5f, 3.5f, 3.5f);
                    }
                }

                

                Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

                if (destroyOnTargetCollision)
                    Destroy(this.gameObject);
            }

            //if (collision.collider == targetCollider2)
            //{
            //    ContactPoint contact = collision.contacts[0];
            //    GameObject target = collision.gameObject;
            //    RaycastHit hit;

            //    float backTrackLength = 1f;
            //    Ray ray = new Ray(contact.point - (-contact.normal * backTrackLength), -contact.normal);
            //    if (collision.collider.Raycast(ray, out hit, 2))
            //    {
            //        if (colorSpawnedObject && ToppingSpawner.pickedUp == false)
            //        {
            //            float randx = Random.Range(-.2f, .28f);
            //            float randz = Random.Range(-.128f, .44f);

            //            GameObject spawned = GameObject.Instantiate(spawnObjectOnCollision);
            //            spawned.transform.SetParent(targetCollider2.transform);
            //            spawned.transform.rotation = Quaternion.Euler(0, 0, 0);
            //            spawned.transform.localPosition = new Vector3(randx, 1.2f, randz);
            //            spawned.transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
            //        }
            //    }

            //    Debug.DrawRay(ray.origin, ray.direction, Color.cyan, 5, true);

            //    if (destroyOnTargetCollision)
            //        Destroy(this.gameObject);
            //}
        }
    }
}
