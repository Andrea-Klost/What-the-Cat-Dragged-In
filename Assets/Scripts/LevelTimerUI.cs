using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class LevelTimerUI : MonoBehaviour {

    private Text _timerText;

    void Awake() {
        _timerText = GetComponent<Text>();
    }

    void Update() {
        _timerText.text = OrderSystem.GET_TIME_REMAINING().ToString("N0");
    }
}
