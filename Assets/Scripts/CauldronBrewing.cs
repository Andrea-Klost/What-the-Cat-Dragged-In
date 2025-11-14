using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CauldronBrewing : MonoBehaviour
{
    [Header("Inscribed")]
    [SerializeField]
    public string[] recipes;
    public GameObject popupUI;

    [Header("Dynamic")]
    [SerializeField]
    public Slot[] brewingSlots; // 0-2 ; Ingredients 1-3
    private Item currItem;

    public List<Item> itemList;
    public Item[] recipeResults;
    public Slot resultSlot;
    public int currIndex = -1;
    public GameObject collidedWith;

    // Start is called before the first frame update
    void Start()
    {
        // Initially hide popup
        if(popupUI != null)
        {
            popupUI.SetActive(false); // Toggle Visibility
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Code is based on this Tutorial : https://youtu.be/1fbd-yTcMgY 
    //Need an OnColl Script here for an Item, place into slots up to index 2
    //If over index 2, make a popup saying "The Cauldron is Full!!"
    void OnCollisionEnter(Collision coll){
        collidedWith = coll.gameObject;
        // Display the UI if collide with player
        if(collidedWith.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(true);
        }

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
            //Check if a recipe can be made
            CheckForCreatedRecipes();
        }
    }
    //Need an OnColl Check for the player, make the UI popup if the player is colliding
    //with Cauldron radius

    void OnCollisionExit(Collision coll)
    {
        //If Cat exits, deactivate popup hoverover ui
        if(coll.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(false);
        }
    }

    // Refer to the video at 8:55
    // Will Pick up back here
    void CheckForCreatedRecipes(){
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        string currentRecipeString = "";
        foreach(Item item in itemList){
            if(item != null){
                currentRecipeString += item.itemName;
            }
            else{
                currentRecipeString += "null";
            }
            for(int i = 0; i < recipes.Length; i++){
                if(recipes[i] == currentRecipeString){
                    resultSlot.gameObject.SetActive(true);
                    resultSlot.GetComponent<Image>().sprite = recipeResults[i].GetComponent<Image>().sprite;
                    resultSlot.item = recipeResults[i];
                }
            }
        }
        //Run string through if statements that cover edge cases
        //Example Case
        //case: BananaBread || BreadBanana || BreadBananaNull || etc.
        if(currentRecipeString.Contains("Bread") && currentRecipeString.Contains("Banana"))
        {
            //Instantiate BananaBread or in this case a Potion!
        }
    }

    //void OnSlotFill(Slot slot){
    //    slot.item = null;
    //    itemList[slot.index] = null;
    //    slot.gameObject.SetActive(false);
    //    CheckForCreatedRecipes();
    //}

    //  This part may need put into its own script as this will have a 
    //  MouseHoverOver and MouseOnClick updating the sprites
    //  See Cancel1-Cancel3 and Confirm1

    //  BOTH should set currIndex to 0 after being clicked
    //  Confirm should only do it if there's a valid recipe

    //  Very Arbitrary Code here, will need to check for cancel
    //  removing all Items, tutorial/instruction manual will explain that removing all items will destroy them.
    //  On Cancel

    //  Will also need to check if Confirm sign is clicked:
    //  instantiate new object based on matching Tag with Itemname
    // OnConfirm(){
    //  
    //  
    //  
    //  
    //}
}
