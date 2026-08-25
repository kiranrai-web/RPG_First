using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private ShopSlot[] shopSlots;
    [SerializeField] private InventoryManager inventoryManager;

    public void PopulateShopItems(List<ShopItems> shopItems)
    {
        if (shopItems == null)
            shopItems = new List<ShopItems>();

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

    // flexible overload
    public void PopulateShopItems(IEnumerable shopItemsEnumerable)
    {
        if (shopItemsEnumerable == null)
            return;

        var list = new List<ShopItems>();
        foreach (var o in shopItemsEnumerable)
        {
            if (o is ShopItems si)
                list.Add(si);
        }

        PopulateShopItems(list);
    }

    public void TryBuyItem(ItemSO itemSO, int price)
    {
        if (itemSO != null && inventoryManager != null && inventoryManager.gold >= price)
        {
            if (HasSpaceForItem(itemSO))
            {
                inventoryManager.AddGold(-price);
                inventoryManager.AddItem(itemSO, 1);
            }
        }
    }

    private bool HasSpaceForItem(ItemSO itemSO)
    {
        if (inventoryManager == null || inventoryManager.itemSlots == null)
            return false;

        foreach (var slot in inventoryManager.itemSlots)
        {
            if (slot == null) continue;
            if (slot.itemSO == itemSO && slot.quantity < itemSO.StackSize)
                return true;
            else if (slot.itemSO == null)
                return true;
        }
        return false;
    }

    public void SellItem(ItemSO itemSO)
    {
        if (itemSO == null || shopSlots == null)
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
        if (inventoryManager != null)
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
