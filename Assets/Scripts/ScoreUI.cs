using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreUI : MonoBehaviour {
    private Text _scoreText;
    
    void Awake() {
        _scoreText = GetComponent<Text>();
    }

    void Update() {
        _scoreText.text = "Score: " + OrderSystem.GET_SCORE().ToString("N0");
    }
}
