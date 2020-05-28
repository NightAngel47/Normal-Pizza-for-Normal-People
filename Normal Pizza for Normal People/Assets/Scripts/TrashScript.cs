/*
 * Normal Pizza for Normal People
 * IM 389
 * TrashScript
 * Sydney & Steven
 * Sydney: Created initial script and functionality that destroyed game objects
 * Steven: Added audio source and made ingredients disable instead of destroy to work with object pooling
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
