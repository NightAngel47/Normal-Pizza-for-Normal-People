using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideTutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialToHide  = null;
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out IngredientHitEffect ingredient))
        {
            tutorialToHide.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
