using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverUI : MonoBehaviour {
    private static GameOverUI instance; // Singleton class
    
    public string levelSuccessText = "Level Passed!";
    public string levelFailedText = "Level Failed...";
    public Text gameOverText;
    
    void Awake() {
        instance = this;
        this.gameObject.SetActive(false);
    }

    public static void DISPLAY_GAMEOVER(bool success) {
        if (success) {
            instance.gameOverText.text = instance.levelSuccessText;
        }
        else {
            instance.gameOverText.text = instance.levelFailedText;
        }
        
        instance.gameObject.SetActive(true);
    }
    
}
