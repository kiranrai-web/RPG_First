using UnityEngine;
using TMPro;
public class InventoryManager : MonoBehaviour
{
    public InventorySlot[] itemSlots;
    public UseItem useItem;
    public int gold;
    public TMP_Text goldText;
    public GameObject lootPrefab;
    public Transform player;
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
            AddGold(quantity);
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
                int maxStack = Mathf.Max(1, item.StackSize);
                int quantityToAdd = Mathf.Min(maxStack, quantity);
                slot.itemSO = item;
                slot.quantity = quantityToAdd;
                quantity -= quantityToAdd;
                slot.UpdateUI();
                if (quantity <= 0)
                    return;
            }

            if (slot.itemSO == item)
            {
                int maxStack = Mathf.Max(1, item.StackSize);
                if (slot.quantity < maxStack)
                {
                    int availableSpace = maxStack - slot.quantity;
                    int quantityToAdd = Mathf.Min(availableSpace, quantity);
                    slot.quantity += quantityToAdd;
                    quantity -= quantityToAdd;
                    slot.UpdateUI();
                    if (quantity <= 0)
                        return;
                }
            }

        }

        if (quantity > 0)
        {
            // drop any remaining quantity once after attempting to fill all slots
            DropLoot(item, quantity);
            Debug.Log("Inventory is full - dropped remaining " + quantity + " of: " + item.itemName);
            return;
        }

        Debug.Log("Inventory is full - could not add item: " + item.itemName);
    }

    public void DropItem(InventorySlot slot)
    {
        DropLoot(slot.itemSO, 1);
        slot.quantity--;
        if(slot.quantity < 0)
        {
            slot.itemSO = null;
        }
        slot.UpdateUI();
    }

    public void AddGold(int amount)
    {
        int before = gold;
        gold += amount;
        if (goldText != null)
            goldText.text = gold.ToString();
        else
            Debug.LogWarning("InventoryManager: goldText is not assigned in the Inspector.");

        Debug.Log($"InventoryManager.AddGold called on '{gameObject.name}': before={before} amount={amount} after={gold}");
    }

    private void DropLoot(ItemSO item, int quantity)
    {
        // spawn with a small offset so it doesn't immediately overlap the player and retrigger pickup
        Vector3 spawnPos = player.position + new Vector3(Random.Range(-0.5f, 0.5f), 1f, 0f);
        Loot loot = Instantiate(lootPrefab, spawnPos, Quaternion.identity).GetComponent<Loot>();
        if (loot != null)
        {
            loot.Initialize(item, quantity);
        }
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

    // Note: stack size is now read from ItemSO.StackSize property
}
