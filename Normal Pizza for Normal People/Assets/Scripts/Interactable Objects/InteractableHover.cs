﻿using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Valve.VR.InteractionSystem;

[RequireComponent(typeof(Interactable1))]
public class InteractableHover : MonoBehaviour
{
	public UnityEvent onHandHoverBegin;
	public UnityEvent onHandHoverEnd;
	public UnityEvent onAttachedToHand;
	public UnityEvent onDetachedFromHand;

	//-------------------------------------------------
	private void OnHandHoverBegin()
	{
		onHandHoverBegin.Invoke();
	}


	//-------------------------------------------------
	private void OnHandHoverEnd()
	{
		onHandHoverEnd.Invoke();
	}


	//-------------------------------------------------
	private void OnAttachedToHand(Hand hand)
	{
		onAttachedToHand.Invoke();
	}


	//-------------------------------------------------
	private void OnDetachedFromHand(Hand hand)
	{
		onDetachedFromHand.Invoke();
	}
}
