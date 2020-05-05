﻿using UnityEngine;

public class CustomerLinePos : MonoBehaviour
{
    public bool isOpen = true;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Customer customer))
        {
            isOpen = true;
        }
    }
}