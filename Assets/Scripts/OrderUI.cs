using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {
    public Image potionIcon;
    
    void Start() {
        potionIcon = GetComponentInChildren<Image>();
    }

    public void SetOrder(Order order) {
        potionIcon.sprite = order.potion.potionSprite;
    }
}
