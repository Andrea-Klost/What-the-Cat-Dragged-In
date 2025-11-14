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
    public string currentRecipeString = "";

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
        //collidedWith = coll.gameObject;
        // Display the UI if collide with player
        if(coll.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(true);
        }

        //Continue Here
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

    void OnCollisionExit(Collision coll)
    {
        //If Cat exits, deactivate popup hoverover ui
        if(coll.gameObject.layer == LayerMask.NameToLayer("Cat"))
        {
            popupUI.SetActive(false);
        }
    }

    // Refer to the video https://youtu.be/1fbd-yTcMgY at 8:55
    // Continue Here
    void CheckForCreatedRecipes(){
        resultSlot.gameObject.SetActive(false);
        resultSlot.item = null;

        currentRecipeString = "";
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
                    break;
                }
            }
        }

    }

    public void CheckAndInstantiate()
    {
        if((currentRecipeString == "") || (currentRecipeString == "nullnullnull"))
        {
            return; // Cancel this function if the string is empty or full of null cases.
        }

        //Should be a way here to check for single cases, but for now...

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
}
