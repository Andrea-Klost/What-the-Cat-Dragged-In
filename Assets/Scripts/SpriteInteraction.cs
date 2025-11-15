using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SpriteInteraction : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, 
IPointerDownHandler, IPointerUpHandler
{
    [Header("Inscribed")]
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
        if(hoverSprite != null)
        {
            buttonImage.sprite = hoverSprite;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Called when the mouse exits the collider of this GameObject
        if(defaultSprite != null)
        {
            buttonImage.sprite = defaultSprite;
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(clickedSprite != null)
        {
            buttonImage.sprite = clickedSprite;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (defaultSprite != null)
        {
            buttonImage.sprite = defaultSprite; // Revert to the original sprite
        }
    }
}
