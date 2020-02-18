﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine.Events;

public class InteractableHover : MonoBehaviour
{
	[RequireComponent(typeof(Interactable1))]
	public class InteractableHoverEvents : MonoBehaviour
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
}
