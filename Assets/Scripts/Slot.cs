using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {
    public Sprite defaultSprite;
    public string itemName; // Cannot be item since item ref will be destroyed when the item gameobject is destroyed
    public int index; // Max of 4, 0-2 is Ingredients, 3 is Output.
    public Text slotLabel;
    
    private Image _slotImage;
    private string _defaultText;

    void Awake() {
        _slotImage = GetComponent<Image>();
        if (slotLabel != null)
            _defaultText = slotLabel.text;
    }

    public void SetDefaultSprite() {
        _slotImage.sprite = defaultSprite;
    }
    
    public void SetSprite(Sprite newSprite) {
        _slotImage.GetComponent<Image>().sprite = newSprite;
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
