using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Order {
    public Potion potion;
    public float startTime;
}

public class OrderSystem : MonoBehaviour {
    private static OrderSystem instance; // Singleton class
    
    [Header("Inscribed")]
    [Tooltip("Time the player gets to complete the level in seconds")]
    public float timeLimit = 120;
    public int maxOrdersAtOnce = 5;
    
    [Header("Dynamic")]
    [SerializeField]
    private List<Order> _currentOrders;
    private Potion[] _availableRecipes;
    [SerializeField] private int _score;
     public int Score {
         get { return _score; }
         private set { _score = value; }
     }
    
    void Start() {
        instance = this;
        _availableRecipes = CauldronBrewing.GET_RECIPE_LIST();
        StartLevel();
    }
    
    void StartLevel() {
        // TODO Set up system for creating orders at random times
        Invoke(nameof(CreateNewOrder), 0);           
    }
    
    void CreateNewOrder() {
        if (_currentOrders.Count == maxOrdersAtOnce)
            return;
        
        Order order = new Order();
        order.potion = _availableRecipes[UnityEngine.Random.Range(0, _availableRecipes.Length)];
        order.startTime = Time.time;
        _currentOrders.Add(order);
    }

    int FindPotionInOrders(Potion potion) {
        for (int i = 0; i < _currentOrders.Count; i++) {
            if (_currentOrders[i].potion == potion)
                return i;
        }
        
        return -1;
    }

    void AssignScore(Order order) {
        // TODO Set up scoring system based on number of ingredients and time it took the player to complete
    }
    
    public static void POTION_DELIVERED(Potion potion) {
        int orderIndex = instance.FindPotionInOrders(potion);
        if (orderIndex == -1) // Delivered potion not part of an active order
            return;

        instance.AssignScore(instance._currentOrders[orderIndex]);
        instance._currentOrders.RemoveAt(orderIndex);
    }
}
