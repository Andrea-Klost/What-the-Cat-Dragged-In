using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
    public List<string> recipe;
    public Sprite potionSprite;
    
    void Awake() {
        recipe.Sort();    
    }

    public string RecipeString() {
        string recipeString = "";
        foreach (string item in recipe) {
            recipeString += item;
        }
        return recipeString;
    }
}
