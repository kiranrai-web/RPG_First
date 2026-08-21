using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Loot : MonoBehaviour
{
    public ItemSO itemSO;
    public SpriteRenderer sr;
    public Animator anim;
    public bool canBePickedUp = true;
    public int quantity;
    public static Action<ItemSO, int> OnItemLooted;
    public float pickupRadius = 0.5f;

    private void OnValidate()
    {
        if (itemSO == null)
            return;
        UpdateAppearance();
    }

    public void Initialize(ItemSO item, int quantity)
    {
        this.itemSO = item;
        this.quantity = quantity;
        canBePickedUp=false;
        UpdateAppearance();
    }

    private void UpdateAppearance()
    {
        if (itemSO == null)
        {
            Debug.LogWarning("Loot.UpdateAppearance called with null itemSO on " + gameObject.name);
            if (sr != null)
                sr.sprite = null;
            return;
        }

        if (sr == null)
        {
            Debug.LogWarning("Loot prefab missing SpriteRenderer (sr) on " + gameObject.name + ". Assign in the Inspector.");
            this.name = itemSO.itemName;
            return;
        }

        sr.sprite = itemSO.icon;
        this.name = itemSO.itemName;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !canBePickedUp)
            return;

        // collect nearby loot of the same type so the player can pick multiples at once
        List<Loot> found = new List<Loot>();
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, pickupRadius);
        int totalQuantity = 0;
        foreach (var hit in hits)
        {
            if (hit == null) continue;
            Loot l = hit.GetComponent<Loot>();
            if (l == null) continue;
            if (l.itemSO == null) continue;
            if (l.itemSO != this.itemSO) continue;
            if (!l.canBePickedUp) continue;

            l.canBePickedUp = false; // mark so it won't be processed twice
            found.Add(l);
            totalQuantity += l.quantity;
        }

        if (found.Count == 0)
            return;

        // play pickup animation on this loot (others will simply be destroyed)
        anim.Play("LootPickUp");

        OnItemLooted?.Invoke(itemSO, totalQuantity);

        // destroy all collected loot objects after a short delay
        foreach (var l in found)
        {
            Destroy(l.gameObject, 0.5f);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canBePickedUp = true;
        }
    }
}
