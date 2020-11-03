/*
 * Normal Pizza for Normal People
 * IM 389
 * CustomerLinePos
 * Steven:
 * Handles when customer lines are available
 */

using System.Collections.Generic;
using UnityEngine;

public class CustomerLinePos : MonoBehaviour
{
    public int customersInLine = 0;

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Customer customer) && customer.activeOrder == false)
        {
            customersInLine--;
        }
    }
}