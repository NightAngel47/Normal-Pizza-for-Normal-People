using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HideTutorial : MonoBehaviour
{
    [SerializeField] private GameObject tutorialToHide;
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out HandCollider handCollider))
        {
            tutorialToHide.SetActive(false);
            gameObject.SetActive(false);
        }
    }
}
