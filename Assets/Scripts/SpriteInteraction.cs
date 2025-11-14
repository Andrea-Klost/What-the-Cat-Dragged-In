using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [Header("Inscribed")]
    [SerializeField]
    public CauldronBrewing cauldron;
    public Sprite defaultSprite;
    public Sprite hoverSprite;
    public Sprite clickedSprite;
    public Button thisButton;

    [Header("Dynamic")]
    private Image buttonImage;

    // Start is called before the first frame update
    void Start()
    {
        buttonImage = thisButton.GetComponent<Image>();
        if(defaultSprite == null)
        {
            // Disable Script if no Sprite
            return;
        }
        buttonImage.sprite = defaultSprite; // Set initial Sprite
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //  Called when the mouse enters the collider of this GameObject
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Called when the mouse exits the collider of this GameObject
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        //Called when the mouse button is pressed while over the collider of this GameObject
        
        //If GameObject is Confirm, CheckAndInstantiate game object based on recipe.
        if(thisButton.name == "Confirm"){
            cauldron.CheckAndInstantiate();
        }
        // Then clear everything, will only clear if cancel is clicked.
        Array.Clear(cauldron.brewingSlots, 0, cauldron.brewingSlots.Length);
        cauldron.itemList.Clear();
        Array.Clear(cauldron.recipeResults, 0, cauldron.brewingSlots.Length);
        cauldron.resultSlot = null;
        cauldron.currIndex = -1;
        cauldron.currentRecipeString = "";
    }
}
