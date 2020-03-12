using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient) || col.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            audioSource.Play();
            Destroy(col.transform.parent.gameObject, 1f);
        }
    }
}
