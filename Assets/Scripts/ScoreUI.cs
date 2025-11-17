using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreUI : MonoBehaviour {
    private Text _scoreText;
    private int _scoreGoal;
    
    void Awake() {
        _scoreText = GetComponent<Text>();
    }

    void Start() {
        _scoreGoal = OrderSystem.GET_SCORE_GOAL();
    }
    
    void Update() {
        _scoreText.text = "Score: " + OrderSystem.GET_SCORE().ToString("N0") + "/"
                          + _scoreGoal.ToString("N0");
    }
}
