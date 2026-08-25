using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;
public class ShopSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    public ItemSO itemSO;
    public TMP_Text itemNameText;
    public TMP_Text priceText;
    public Image itemImage;
    public int price;

    [SerializeField] private ShopManager shopManager;
    [SerializeField] private ShopInfo shopInfo;
    public void Initialize(ItemSO newItemSO, int price)
    {
        itemSO = newItemSO;

        if (itemSO == null)
        {
            // clear slot
            if (itemImage != null) itemImage.gameObject.SetActive(false);
            if (itemNameText != null) itemNameText.text = "";
            if (priceText != null) priceText.text = "";
            this.price = 0;
            return;
        }

        if (itemImage != null)
        {
            itemImage.sprite = itemSO.icon;
            itemImage.gameObject.SetActive(true);
        }
        else
        {
            Debug.LogWarning($"ShopSlot.Initialize: itemImage not assigned on {gameObject.name}");
        }

        if (itemNameText != null)
            itemNameText.text = itemSO.itemName;
        else
            Debug.LogWarning($"ShopSlot.Initialize: itemNameText not assigned on {gameObject.name}");

        this.price = price;
        if (priceText != null)
            priceText.text = price.ToString();
        else
            Debug.LogWarning($"ShopSlot.Initialize: priceText not assigned on {gameObject.name}");
    }

    public void OnBuyButtonClicked()
    {
        shopManager.TryBuyItem(itemSO, price);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (itemSO != null && shopInfo != null)
            shopInfo.ShowItemInfo(itemSO);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (shopInfo != null)
            shopInfo.HideItemInfo();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (itemSO != null && shopInfo != null)
            shopInfo.FollowMouse();
    }
}
