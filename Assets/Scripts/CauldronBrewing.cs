using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronBrewing : MonoBehaviour
{
    private Item currItem;

    public List<Item> itemList;
    public string[] recipes;
    public Item[] recipeResults;
    
    public Slot[] brewingSlots; // 0-2 ; Ingredients 1-3
    public Slot resultSlot;
    public int currIndex = -1;

    // Code is based on this Tutorial : https://youtu.be/1fbd-yTcMgY 
    //Need an OnColl Script here for an Item, place into slots up to index 2
    //If over index 2, make a popup saying "The Cauldron is Full!!"
    void OnCollisionEnter(Collision coll){
        // Find out what collided with the Cauldron

        //If an Ingredient, handle as follows.
        //Insert gameobj into slot as a sprite.
        if ((coll.gameObject.layer == LayerMask.NameToLayer("Ingredient")) && (currIndex < 3))
        {
            //Increase currIndex
            currIndex += 1;
            //Setup nextSlot
            Slot nextSlot = null;
            nextSlot.index = currIndex;

            //Setup currItem
            currItem.itemName = coll.gameObject.tag;
            //Might need a sprite component?
            nextSlot.GetComponent<Image>().sprite = currItem.GetComponent<Image>().sprite;
            nextSlot.item = currItem;
            itemList[nextSlot.index] = currItem;
            currItem = null;
            //Destroy currItem gameObj.
            Destroy(coll.gameObject);
        }
    }
    //Need an OnColl Check for the player, make the UI popup if the player is colliding
    //with Cauldron radius

    //Need a script to cancel, removing all Items, tutorial/instruction manual
    //will explain that removing all items will destroy them.

    //void CheckForCreatedRecipes(){
    // resultSlot.gameObject.SetActive(false);
    // resultSlot.item = null;
    // string currentRecipeString = "";
    // foreach(Item item in itemList){
    //  if(item != null){
    //      currentRecipeString += item.itemName;
    //  }
    //  else{
    //      currentRecipeString += "null";
    //  }
    //  for(int i = 0; i < recipes.Length; i++){
    //      if(recipes[i] == currentRecipeString){
    //          resultSlot.gameObject.SetActive(true);
    //          resultSlot.GetComponent<Image>().sprite = recipeResults[i].GetComponent<Image>().sprite;
    //          resultSlot.item = recipeResults[i];
    //      }
    //  }
    //
    // }
    //}
    //  Very Arbitrary Code here, will need to check for cancel
    //  removing all Items, tutorial/instruction manual will explain that removing all items will destroy them.
    
    // OnSlotFill(Slot slot){
    // slot.item = null;
    // itemList[slot.index] = null;
    // slot.gameObject.SetActive(false);
    // CheckForCreatedRecipes();
    //}

    // OnConfirm(){
    //  
    //  
    //  
    //  
    //}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
