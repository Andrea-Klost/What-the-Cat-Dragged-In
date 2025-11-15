using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour {
    public List<string> recipe;
    public Sprite potionSprite;
    
    public string RecipeString() {
        recipe.Sort();
        string recipeString = "";
        foreach (string item in recipe) {
            recipeString += item;
        }
        return recipeString;
    }
}
