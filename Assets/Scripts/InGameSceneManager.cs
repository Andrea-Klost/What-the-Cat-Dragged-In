using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameSceneManager : MonoBehaviour {
    public static void RELOAD_LEVEL() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
    public static void LOAD_MAIN_MENU() {
        SceneManager.LoadScene(0);
    }
}
