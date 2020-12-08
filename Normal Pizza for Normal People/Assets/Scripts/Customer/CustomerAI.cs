/*
 * Normal Pizza for Normal People
 * IM 491
 * CustomerAI
 * Steven
 * Steven: Handles customer ai, including state, target pos, and movement
 */

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class CustomerAI : MonoBehaviour
{
    private NavMeshAgent agent = null;
    private Customer thisCustomer;
    private CustomerLine customerLine;
    
    public enum CustomerAIStates {Entering, Leaving, AtCounter}
    public CustomerAIStates CurrentCustomerAIState { get; private set; } = CustomerAIStates.Entering;

    [SerializeField] private LayerMask mask;
    [HideInInspector] public CustomerLinePos targetLinePos;
    private Vector3 endPos = Vector3.zero;
    private static bool customerAboutToLeave = false;
    
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        thisCustomer = GetComponent<Customer>();
        customerLine = FindObjectOfType<CustomerLine>();
        
        endPos = transform.position + (Vector3.right * 14);
    }

    private void Start()
    {
        StartCoroutine(WaitingInLine());
    }

    private IEnumerator WaitingInLine()
    {
        if (CurrentCustomerAIState != CustomerAIStates.Entering) yield break;
        
        if (Physics.SphereCast(transform.position, agent.radius, transform.forward, out RaycastHit hit, agent.stoppingDistance * 2, mask))
        {
            if (hit.transform.TryGetComponent(out CustomerAI otherCustomerAI))
            {
                if (otherCustomerAI.CurrentCustomerAIState != CustomerAIStates.Leaving
                    && otherCustomerAI.targetLinePos == targetLinePos)
                {
                    agent.isStopped = true;
                }
                else if (otherCustomerAI.CurrentCustomerAIState == CustomerAIStates.Leaving)
                {
                    customerAboutToLeave = true;
                    yield return new WaitForSeconds(thisCustomer.customerLeaveDelayTime * 1.5f);
                    agent.isStopped = false;
                    customerAboutToLeave = false;
                }
            }
            else
            {
                agent.isStopped = false;
            }
        }

        if (agent.isStopped)
        {
            thisCustomer.customerAudio.ChangeCustomerAudio(CustomerAudio.CustomerAudioStates.Stop);
            yield return new WaitWhile((() => customerAboutToLeave));
            
            CustomerLinePos shortestLinePos = customerLine.FindShortestCustomerLine();
            if (targetLinePos.customersInLine.IndexOf(thisCustomer) > shortestLinePos.customersInLine.Count)
            {
                agent.isStopped = false;
                targetLinePos.customersInLine.Remove(thisCustomer);
                SetTargetLine(shortestLinePos);
                shortestLinePos.customersInLine.Add(thisCustomer);
            }
        }
        
        yield return new WaitForEndOfFrame();

        if (CurrentCustomerAIState == CustomerAIStates.Entering)
        {
            StartCoroutine(WaitingInLine());
        }
    }

    public void ChangeCustomerAIState(CustomerAIStates state)
    {
        CurrentCustomerAIState = state;
    }
    
    public void SetTargetLine(CustomerLinePos customerLinePos)
    {
        targetLinePos = customerLinePos;
        agent.SetDestination(targetLinePos.transform.position);
    }

    public void Leave()
    {
        agent.SetDestination(endPos);
    }
}
