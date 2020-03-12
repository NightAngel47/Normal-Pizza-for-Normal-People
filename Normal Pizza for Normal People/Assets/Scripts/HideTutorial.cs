using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;

public class HideTutorial : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.parent.TryGetComponent(out HandCollider handCollider))
        {
            gameObject.SetActive(false);
        }
    }
}
