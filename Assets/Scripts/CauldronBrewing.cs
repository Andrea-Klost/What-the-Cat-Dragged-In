using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CauldronBrewing : MonoBehaviour {
    private static CauldronBrewing instance; // Singleton class, will only be one cauldron
    
    [Header("Inscribed")]
    [SerializeField]
    public Potion[] recipes; // Prefabs of the potions, must have Potion component
    public GameObject popupUI;
    public Transform potionSpawnPoint;
    public Slot[] brewingSlots; // 0-2 ; Ingredients 1-3
    public Slot resultSlot;
    
    [Header("Dynamic")] 
    [SerializeField] private List<String> itemList;
    
    void Awake() {
        if (potionSpawnPoint == null)
            potionSpawnPoint = this.transform;
        
        instance = this;
        itemList = new List<String>();
        resultSlot.gameObject.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide popup
        if(popupUI != null)
        {
            popupUI.SetActive(false); // Toggle Visibility
        }
    }
    
    void OnTriggerEnter(Collider coll) {
        // Display the UI if collide with player
        if(coll.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(true);
        }
        
        // If item add to list and check for a recipe match
        Item collItem = coll.GetComponent<Item>();
        if (collItem != null && itemList.Count < 3)
        {
            // currIndex equal to the current amount of items in cauldron
            int currIndex = itemList.Count;
            
            // Setup next slot
            brewingSlots[currIndex].itemName = collItem.itemName;
            brewingSlots[currIndex].index = currIndex;
            brewingSlots[currIndex].SetLabel(collItem.itemName.Replace('_', ' '));
            
            // Add to list
            itemList.Add(collItem.itemName);
            itemList.Sort();
            
            
            // If Item has a sprite, set slot to sprite
            if (collItem.itemSprite != null)
                brewingSlots[currIndex].SetSprite(collItem.itemSprite);
            
            //Destroy currItem gameObj.
            Destroy(coll.gameObject);
            //Check if a recipe can be made
            CheckForCreatedRecipes();
        }
    }

    void OnTriggerExit(Collider coll) {
        //If Cat exits, deactivate popup hoverover ui
        if(coll.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(false);
        }
    }

    string CurrentRecipeString() {
        string recipeString = "";
        foreach (string item in itemList) {
            recipeString += item;
        }
        return recipeString;
    }
    
    void CheckForCreatedRecipes() {
        string currRecipeString = CurrentRecipeString();
        for(int i = 0; i < recipes.Length; i++){
            if(recipes[i].RecipeString() == currRecipeString) {
                resultSlot.gameObject.SetActive(true);
                resultSlot.SetSprite(recipes[i].potionSprite);
                resultSlot.itemName = recipes[i].name;
                return;
            }
        }
        // No Recipe found clear result slot
        ClearResultSlot();
    }

    void CheckAndInstantiate() {
        // Return if Cauldron is empty
        if (itemList.Count == 0) {
            return;
        }
        
        string currRecipeString = CurrentRecipeString();
        Potion matchedPotion = null;
        foreach (Potion p in recipes) {
            if (p.RecipeString() == currRecipeString) {
                matchedPotion = p;
                break;
            } 
        }
        
        // No recipes found, return 
        if (matchedPotion == null)
            return;
        
        // Spawn potion and clear all slots
        ClearSlots();
        GameObject potionGo = Instantiate(matchedPotion.gameObject);
        potionGo.transform.position = potionSpawnPoint.position;
    }

    void ClearSlots() {
        foreach (Slot s in brewingSlots) {
            s.itemName = "";
            s.SetDefaultSprite();
            s.SetDefaultLabel();
        }
        ClearResultSlot();
        
        itemList.Clear();
    }

    void ClearResultSlot() {
        resultSlot.gameObject.SetActive(false);
        resultSlot.itemName = "";
        resultSlot.SetDefaultSprite();
        resultSlot.SetDefaultLabel();
    }
    
    public static void ON_BUTTON_CONFIRM() {
        instance.CheckAndInstantiate();
    }
    
    public static void ON_BUTTON_CANCEL() {
        instance.ClearSlots();
    }
}
