using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderPanelUI : MonoBehaviour {
    private static OrderPanelUI instance;

    public GameObject prefabOrderUI;

    private List<OrderUI> _managedOrders;
    
    void Awake() {
        instance = this;
        _managedOrders = new List<OrderUI>();
    }
    
    void AddOrder(Order order) {
        GameObject orderUI = Instantiate(prefabOrderUI, this.gameObject.transform);
        OrderUI uiScript = orderUI.GetComponent<OrderUI>();
        uiScript.SetOrder(order);

        Vector3 position = orderUI.transform.localPosition;
        position.x += 150 * _managedOrders.Count;
        orderUI.transform.localPosition = position;
        
        _managedOrders.Add(uiScript);
    }

    void RemoveOrder(int index) {
        Destroy(_managedOrders[index].gameObject);
        _managedOrders.RemoveAt(index);
        // Shift other order left
        Vector3 position;
        for (int i = index; i < _managedOrders.Count; i++) {
            position = _managedOrders[i].gameObject.transform.localPosition;
            position.x -= 150;
            _managedOrders[i].gameObject.transform.localPosition = position;
        }
    }
    
    public static void ADD_ORDER(Order order) {
        instance.AddOrder(order);
    }

    public static void REMOVE_ORDER(int index) {
        instance.RemoveOrder(index);
    }
}
