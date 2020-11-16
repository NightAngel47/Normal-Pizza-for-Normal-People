/*
 * Normal Pizza for Normal People
 * IM 389
 * CustomerLinePos
 * Steven:
 * Handles when customer lines are available
 */

using System;
using System.Collections.Generic;
using UnityEngine;

public class CustomerLinePos : MonoBehaviour
{
    public List<Customer> customersInLine = new List<Customer>();

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Customer customer) 
            && customer.customerAI.CurrentCustomerAIState == CustomerAI.CustomerAIStates.Leaving)
        {
            customersInLine.Remove(customer);
        }
    }
}