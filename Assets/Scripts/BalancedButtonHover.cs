﻿using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BalancedButtonHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    private Text myText;

    void Start()
    {
        myText = GetComponentInChildren<Text>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        myText.text = "Normal hit/Dmg %";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        myText.text = "Balanced Attack";
    }
}