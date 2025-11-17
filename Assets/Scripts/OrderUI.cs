using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class OrderUI : MonoBehaviour {
    public Image potionIcon;
    public Text timeText;
    
    private float _orderStartTime;
    
    
    void Awake() {
        _orderStartTime = 0f;
    }

    void Start() {
        potionIcon = GetComponentInChildren<Image>();
        timeText = GetComponentInChildren<Text>();
    }

    void Update() {
        timeText.text = ((int)(Time.time - _orderStartTime)).ToString();
    }

    public void SetOrder(Order order) {
        _orderStartTime = order.startTime;
        potionIcon.sprite = order.potion.potionSprite;
    }
}
