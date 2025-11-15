using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //public GameObject thisItem;
    public string itemName;
    public Sprite itemSprite;
    
    // Start is called before the first frame update
    void Start()
    {
        itemName = gameObject.tag; // Will use the objectTag
    }
}
