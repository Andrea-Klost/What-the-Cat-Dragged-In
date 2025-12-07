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
    public float timeLimit = 300;
    public int scoreGoal = 500;
    public GameObject startText;
    public int levelNumber = 1;
    
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
    [SerializeField] private float _timeStart;
    [SerializeField] private bool _levelStarted;
    [SerializeField] private bool _levelEnded;
    [SerializeField] private DeliveryLocation[] _deliveryLocations;
    [SerializeField] private int _activeDeliveryLocationIndex;
    
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
         _timeStart = 0f;
         _levelStarted = false;
         _levelEnded = false;
         _activeDeliveryLocationIndex = -1;
     }

     void Start() {
        _availableRecipes = CauldronBrewing.GET_RECIPE_LIST();
        _deliveryLocations = FindObjectsByType<DeliveryLocation>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        DisableAllDeliveryLocations();
        startText.SetActive(true);
    }

    void Update() {
        if (!_levelStarted || _levelEnded) 
            return;
        
        if (Time.time - _timeLastOrder >= _timeUntilNextOrder) {
            CreateNewOrder();
        }

        if (Time.time - _timeStart > timeLimit) {
            EndLevel();    
        }
    }

    void StartLevel() {
        if (_levelStarted)
            return;
        
        // Destroy all potions to prevent pre-making orders
        Potion[] potionList = FindObjectsByType<Potion>(FindObjectsInactive.Exclude, FindObjectsSortMode.None);
        foreach (Potion p in potionList) {
            Destroy(p.gameObject);
        }
        
        _timeStart = _timeLastOrder = Time.time;
        _timeUntilNextOrder = 5;
        ChangeDeliveryLocation();
        _levelStarted = true;
        startText.SetActive(false);
    }

    void EndLevel() {
        if (_levelEnded)
            return;
        
        _levelEnded = true;
        
        // Clear orders and Order UI
        OrderPanelUI.CLEAR_ORDERS();
        instance.DisableAllDeliveryLocations(); // Disable all delivery locations
        _currentOrders.Clear();

        bool isLevelSuccess = Score >= scoreGoal;
        if (isLevelSuccess) {
            PlayerPrefs.SetInt($"lvl{levelNumber + 1}_unlocked", 1);
        }
        
        // Display Game Over UI
        GameOverUI.DISPLAY_GAMEOVER(Score >= scoreGoal);
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

    void ChangeDeliveryLocation() {
        if (_activeDeliveryLocationIndex >= 0)
            _deliveryLocations[_activeDeliveryLocationIndex].gameObject.SetActive(false);

        if (_deliveryLocations.Length == 1) {
            _activeDeliveryLocationIndex = 0;
        }
        else if (_deliveryLocations.Length <= 0) {
            Debug.LogWarning("OrderSystem: No DeliveryLocations in Scene");
            return;
        }
        else {
            // Choose a random delivery location that is not the last location
            // (this means the same delivery location will not be chosen twice in a row)
            int oldIndex = _activeDeliveryLocationIndex;
            while (_activeDeliveryLocationIndex == oldIndex) {
                _activeDeliveryLocationIndex = UnityEngine.Random.Range(0, _deliveryLocations.Length);
            }
        }
        _deliveryLocations[_activeDeliveryLocationIndex].gameObject.SetActive(true);
    }

    void DisableAllDeliveryLocations() {
        foreach (DeliveryLocation location in _deliveryLocations) {
            location.gameObject.SetActive(false);
        }
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
        instance.ChangeDeliveryLocation();

        if (instance.Score >= instance.scoreGoal) { // end level early if goal is reached
            instance.EndLevel();
        }
        
        return true;
    }

    public static void START_LEVEL() {
        instance.StartLevel();
    }

    public static int GET_SCORE() {
        return instance.Score;
    }

    public static int GET_SCORE_GOAL() {
        return instance.scoreGoal;
    }

    public static float GET_TIME_REMAINING() {
        if (!instance._levelStarted)
            return instance.timeLimit;
        if (instance._levelEnded)
            return 0;
        return instance.timeLimit - (Time.time - instance._timeStart);
    }
}
