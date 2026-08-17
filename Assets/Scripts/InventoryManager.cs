using UnityEngine;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;
    public UseItem useItem;
    public int gold;
    public TMP_Text goldText;
    private void OnEnable()
    {
        Loot.OnItemLooted += AddItem;
    }

    private void OnDisable()
    {
        Loot.OnItemLooted -= AddItem;
    }

    public void AddItem(ItemSO item, int quantity)
    {
        if (item == null)
        {
            Debug.LogWarning("InventoryManager.AddItem called with null item");
            return;
        }

        if (item.isGold)
        {
            gold += quantity;
            if (goldText != null)
                goldText.text = gold.ToString();
            else
                Debug.LogWarning("InventoryManager: goldText is not assigned in the Inspector.");
            return;
        }

        if (itemSlots == null || itemSlots.Length == 0)
        {
            Debug.LogWarning("InventoryManager: itemSlots not configured or empty.");
            return;
        }

        foreach (var slot in itemSlots)
        {
            if (slot == null)
                continue;

            if (slot.itemSO == null)
            {
                slot.itemSO = item;
                slot.quantity = quantity;
                slot.UpdateUI();
                return;
            }
        }

        Debug.Log("Inventory is full - could not add item: " + item.itemName);
    }

    public void UseItem(InventorySlot slot)
    {
        if(slot.itemSO != null && slot.quantity >= 0)
        {
            useItem.ApplyItemEffects(slot.itemSO);
            slot.quantity--;
            if(slot.quantity <= 0)
            {
                slot.itemSO = null;
            }
            slot.UpdateUI();
        }
    }
}
