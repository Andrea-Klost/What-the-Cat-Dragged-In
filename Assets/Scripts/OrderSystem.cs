using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public struct Order {
    public Potion potion;
    public float startTime;
}

public class OrderSystem : MonoBehaviour {
    private static OrderSystem instance; // Singleton class
    
    [Header("Level")]
    [Tooltip("Time the player gets to complete the level in seconds")]
    public float timeLimit = 120;
    
    [Header("Orders")]
    public int maxOrdersAtOnce = 5;
    public float minTimeBetweenOrders = 10f;
    public float maxTimeBetweenOrders = 30f;
    
    [Header("Scoring")]
    public int ingredientMultiplier = 20;
    [Tooltip("Time limit for the player to get a time bonus " 
             + "(i.e. if the player takes timeThreshold or more to complete and order, their time bonus is 0)")]
    public int timeThreshold = 25;
    
    [Header("Dynamic")]
    [SerializeField] private List<Order> _currentOrders;
    [SerializeField] private float _timeLastOrder;
    [SerializeField] private float _timeUntilNextOrder;
    [SerializeField] private bool _levelStarted;
    
    private Potion[] _availableRecipes;
    
    
    [SerializeField] private int _score;
     public int Score {
         get { return _score; }
         private set { _score = value; }
     }

     void Awake() {
         instance = this;
         _score = 0;
         _timeLastOrder = 0f;
         _levelStarted = false;
     }

     void Start() {
        _availableRecipes = CauldronBrewing.GET_RECIPE_LIST();
    }

    void Update() {
        if (!_levelStarted)
            return;
        
        if (Time.time - _timeLastOrder >= _timeUntilNextOrder) {
            CreateNewOrder();
        }
        
        
    }

    void StartLevel() {
        if (_levelStarted)
            return;
        
        _timeUntilNextOrder = 5;
        _levelStarted = true;
    }
    
    void CreateNewOrder() {
        if (_currentOrders.Count == maxOrdersAtOnce)
            return;

        Order order = new Order();
        order.potion = _availableRecipes[UnityEngine.Random.Range(0, _availableRecipes.Length)];
        order.startTime = _timeLastOrder = Time.time;
        OrderPanelUI.ADD_ORDER(order); // Add order to UI
        _currentOrders.Add(order);
        _timeUntilNextOrder = UnityEngine.Random.Range(minTimeBetweenOrders, maxTimeBetweenOrders);
    }

    int FindPotionInOrders(Potion potion) {
        for (int i = 0; i < _currentOrders.Count; i++) {
            if (_currentOrders[i].potion.gameObject.CompareTag(potion.gameObject.tag))
                return i;
        }
        
        return -1;
    }

    void AssignScore(Order order) {
        float timeTaken = Time.time - order.startTime;
        _score += order.potion.recipe.Count * ingredientMultiplier + 
                  Convert.ToInt32(Math.Round(Math.Max(0, -timeTaken + timeThreshold)));
    }
    
    public static bool POTION_DELIVERED(Potion potion) {
        int orderIndex = instance.FindPotionInOrders(potion);
        if (orderIndex == -1) // Delivered potion not part of an active order
            return false;

        // If at order cap delay next order
        if (instance._currentOrders.Count == instance.maxOrdersAtOnce) {
            instance._timeLastOrder = Time.time;
        }

        instance.AssignScore(instance._currentOrders[orderIndex]);
        OrderPanelUI.REMOVE_ORDER(orderIndex);
        instance._currentOrders.RemoveAt(orderIndex);
        
        return true;
    }

    public static void START_LEVEL() {
        instance.StartLevel();
    }

    public static int GET_SCORE() {
        return instance.Score;
    }
}
