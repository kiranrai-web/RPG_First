using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;

public class ShopManager : MonoBehaviour
{
    public static event Action<ShopManager, bool> onShopStateChanged;
    [SerializeField] private List<ShopItems> shopItems;
    [SerializeField] private ShopSlot[] shopSlots;

    [SerializeField] private InventoryManager inventoryManager;
    private void Start()
    {
        // ensure inventoryManager reference is populated
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            Debug.LogWarning("ShopManager: inventoryManager was not assigned in the Inspector. Found: " + (inventoryManager != null ? inventoryManager.gameObject.name : "null"));
        }

        PopulateShopItems();
        onShopStateChanged?.Invoke(this, true);
    }
    public void PopulateShopItems()
    {
        for (int i = 0; i < shopItems.Count && i < shopSlots.Length; i++)
        {
            ShopItems shopItem = shopItems[i];
            shopSlots[i].Initialize(shopItem.itemSO, shopItem.price);
            shopSlots[i].gameObject.SetActive(true);
        }
        for (int i = shopItems.Count; i < shopSlots.Length; i++)
        {
            shopSlots[i].gameObject.SetActive(false);
        }
    }

    public void TryBuyItem(ItemSO itemSO, int price)
    {
        if (itemSO != null && inventoryManager.gold >= price)
        {
            if (HasSpaceForItem(itemSO))
            {
                // charge through InventoryManager so UI updates correctly
                if (inventoryManager != null)
                    inventoryManager.AddGold(-price);
                else
                    Debug.LogWarning("ShopManager.TryBuyItem: inventoryManager is null, cannot deduct gold.");
                inventoryManager.AddItem(itemSO,1);
            }
        }
    }

    private bool HasSpaceForItem(ItemSO itemSO)
    {
        foreach(var slot in inventoryManager.itemSlots)
        {
            if (slot.itemSO == itemSO && slot.quantity < itemSO.stackSize)
                return true;
            else if(slot.itemSO == null)
                return true;
        }
        return false;
    }

    public void SellItem(ItemSO itemSO)
    {
        if (itemSO == null)
            return;

        // find matching shop entry to determine sell price
        foreach (var slot in shopSlots)
        {
            if (slot == null)
                continue;

            if (slot.itemSO == itemSO)
            {
                if (inventoryManager == null)
                {
                    Debug.LogWarning("ShopManager.SellItem: inventoryManager is null. Cannot add gold.");
                    return;
                }

                Debug.Log($"ShopManager.SellItem: selling {itemSO.itemName} for {slot.price} gold. InventoryManager before={inventoryManager.gold}");
                inventoryManager.AddGold(slot.price);
                Debug.Log($"ShopManager.SellItem: InventoryManager after={inventoryManager.gold}");
                return;
            }
        }

        // if item not found in shop catalog, give a minimal fallback amount
        inventoryManager.AddGold(1);
        Debug.LogWarning("Sold item that is not in shop catalog: " + (itemSO != null ? itemSO.itemName : "null") + ". Gave fallback gold=1.");
    }
}

[System.Serializable]
public class ShopItems
{
    public ItemSO itemSO;
    public int price;
}
