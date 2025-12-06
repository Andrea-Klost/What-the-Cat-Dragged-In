using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public Sprite defaultSprite;
    public string itemName; // Cannot be item since item ref will be destroyed when the item gameobject is destroyed
    public int index; // Max of 4, 0-2 is Ingredients, 3 is Output.
    public Text slotLabel;
    public Image slotImage;
    
    private string _defaultText;

    void Awake() {
        if (slotImage == null)
            Debug.LogError("slotImage not set for Slot");
        if (slotLabel != null)
            _defaultText = slotLabel.text;
    }
    
    public void SetDefaultSprite() {
        slotImage.sprite = defaultSprite;
    }
    
    public void SetSprite(Sprite newSprite) {
        slotImage.GetComponent<Image>().sprite = newSprite;
    }

    public void SetDefaultLabel() {
        if (slotLabel != null)
            slotLabel.text = _defaultText;
    }
    
    public void SetLabel(string label) {
        if (slotLabel != null)
            slotLabel.text = label;
    }
}
