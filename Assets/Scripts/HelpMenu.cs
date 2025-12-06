using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpMenu : MonoBehaviour {
    private static HelpMenu instance;
 public GameObject HideThis;

     void Awake() {
         instance = this;
     }
 
    public void ToggleMenu()
    {
        bool isActive = HideThis.activeSelf;

        if (HideThis == null)
        {
            Debug.LogWarning("MenuToggle: No HideThis assigned!");
            return;
        }

        if (isActive)
        {
            HideThis.SetActive(false);
        }
        else
        {
            HideThis.SetActive(true);
        } 
    }

    public static void TOGGLE_MENU() {
        instance.ToggleMenu();
    }
}
