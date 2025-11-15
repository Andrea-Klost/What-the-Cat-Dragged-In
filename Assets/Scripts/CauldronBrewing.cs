using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] 
    private List<String> itemList;
    Text [] slotsNames; //Refers to the name textbox under slot.
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
        slotsNames = new Text[3];
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
            itemList.Add(collItem.itemName);
            itemList.Sort();
            //  Set Slot Texts.
            slotsNames[0] = brewingSlots[0].transform.Find("Name").GetComponent<Text>();
            slotsNames[1] = brewingSlots[1].transform.Find("Name").GetComponent<Text>();
            slotsNames[2] = brewingSlots[2].transform.Find("Name").GetComponent<Text>();
            
            for(int i = 0; i < 3; ++i)
            {
                if(slotsNames[i] != null)
                {
                    slotsNames[i].text = brewingSlots[i].itemName;
                }
            }

            // If Item has a sprite, set slot to sprite
            if (collItem.itemSprite != null)
                brewingSlots[currIndex].GetComponent<Image>().sprite = collItem.itemSprite;
            
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
                resultSlot.GetComponent<Image>().sprite = recipes[i].potionSprite;
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
            s.GetComponent<Image>().sprite = s.defaultSprite;
        }
        ClearResultSlot();
        
        //  Reset slotsNames.
        slotsNames[0].text = "Ingredient 1"; slotsNames[1].text = "Ingredient 2"; slotsNames[2].text = "Ingredient 3";
        itemList.Clear();
    }

    void ClearResultSlot() {
        resultSlot.gameObject.SetActive(false);
        resultSlot.itemName = "";
        resultSlot.GetComponent<Image>().sprite = resultSlot.defaultSprite;
    }
    
    public static void ON_BUTTON_CONFIRM() {
        instance.CheckAndInstantiate();
    }
    
    public static void ON_BUTTON_CANCEL() {
        instance.ClearSlots();
    }
}
