using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeMenu : MonoBehaviour
{
 public GameObject HideThis;

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
}
