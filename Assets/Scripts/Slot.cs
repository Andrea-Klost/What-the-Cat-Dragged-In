using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public Sprite defaultSprite;
    public string itemName; // Cannot be item since item ref will be destroyed when the item gameobject is destroyed
    public int index; // Max of 4, 0-2 is Ingredients, 3 is Output.

    public void SetDefaultSprite()
    {
        this.GetComponent<Image>().sprite = defaultSprite;
    }
}
