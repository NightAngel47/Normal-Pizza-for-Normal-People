﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTransistioner : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerClickHandler, IPointerUpHandler
{
    public Color32 norm = Color.white;
    public Color32 hover = Color.gray;
    public Color32 down = Color.white;

    private Image img = null;

    private void Awake()
    {
        img = GetComponent<Image>();    
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        img.color = hover;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        img.color = down;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        img.color = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        img.color = norm;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
