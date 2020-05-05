﻿/*
 * Normal Pizza for Normal People
 * IM 389
 * TrashScript
 * Sydney 
 * Will destroy anything thrown in trash can, or set active in case of ingredients for object pool
 */

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
            if (ingredient)
            {
                ingredient.gameObject.SetActive(false);
            }
            else
            {
                Destroy(col.transform.parent.gameObject, 1f);
            }
        }
    }
}
