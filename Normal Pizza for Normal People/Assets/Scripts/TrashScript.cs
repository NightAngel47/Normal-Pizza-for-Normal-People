using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider col)
    {
        if (col.transform.parent.TryGetComponent(out IngredientHitEffect ingredient) || col.transform.parent.TryGetComponent(out PizzaBehaviour pizza))
        {
            Destroy(col);
        }
    }
}
