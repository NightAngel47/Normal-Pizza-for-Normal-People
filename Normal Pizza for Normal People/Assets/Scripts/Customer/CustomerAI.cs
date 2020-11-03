/*
 * Normal Pizza for Normal People
 * IM 491
 * CustomerAI
 * Steven
 * Steven: Handles customer ai, including state, target pos, and movement
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private NavMeshAgent agent = null;
    
    public enum CustomerAIStates {Entering, Leaving, Stopped}

    public CustomerAIStates CurrentCustomerAIState { get; private set; } = CustomerAIStates.Entering;
    
    private Vector3 targetLinePos = Vector3.zero;
    private Vector3 endPos = Vector3.zero;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        
        endPos = transform.position + (Vector3.right * 14);
    }

    // Update is called once per frame
    void Update()
    {
        if (CurrentCustomerAIState != CustomerAIStates.Leaving)
        {
            if (Physics.SphereCast(transform.position, 0.5f, transform.forward, out RaycastHit hit, 0.5f))
            {
                if (hit.collider.TryGetComponent(out CustomerAI customerAI) && customerAI.CurrentCustomerAIState != CustomerAIStates.Leaving)
                {
                    agent.SetDestination(transform.position);
                }
            }
            else if(agent.destination != targetLinePos)
            {
                agent.SetDestination(targetLinePos);
            }
        }
    }

    public void ChangeCustomerAIState(CustomerAIStates state)
    {
        CurrentCustomerAIState = state;
    }
    
    public void SetTargetLine(Vector3 customerLinePos)
    {
        targetLinePos = customerLinePos;
        agent.SetDestination(targetLinePos);
        ChangeCustomerAIState(CustomerAIStates.Entering);
    }

    public void Leave()
    {
        agent.SetDestination(endPos);
    }
}
