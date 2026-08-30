using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Runtime helper to make shop UI clickable and auto-wire top-level shop buttons.
// Attach to the Shop Canvas GameObject.
public class ShopUiAutoWire : MonoBehaviour
{
    public Canvas shopCanvas;

    void Start()
    {
        if (shopCanvas == null)
            shopCanvas = GetComponent<Canvas>();
        if (shopCanvas == null)
        {
            Debug.LogWarning("ShopUiAutoWire: no Canvas assigned or found on this GameObject.");
            return;
        }

        // Aggressive: disable raycast on all Graphics except button target graphics
        var buttons = shopCanvas.GetComponentsInChildren<Button>(true);
        var allowedGraphics = new System.Collections.Generic.HashSet<Graphic>();
        foreach (var b in buttons)
        {
            if (b == null) continue;
            if (b.targetGraphic != null)
                allowedGraphics.Add(b.targetGraphic);
        }

        int disabledCount = 0;
        var allGraphics = shopCanvas.GetComponentsInChildren<Graphic>(true);
        foreach (var g in allGraphics)
        {
            if (g == null) continue;
            if (allowedGraphics.Contains(g))
                continue;
            if (g.raycastTarget)
            {
                g.raycastTarget = false;
                disabledCount++;
            }
        }
        if (disabledCount > 0)
            Debug.Log($"ShopUiAutoWire: disabled raycastTarget on {disabledCount} non-button Graphics under {shopCanvas.gameObject.name}.");

        // Auto-wire top-level shop navigation buttons to ShopButtonToggles if needed
        var toggles = FindObjectOfType<ShopButtonToggles>();
        // buttons already collected above
        foreach (var b in buttons)
        {
            if (b == null) continue;
            // ensure the button's target graphic can receive raycasts
            var img = b.GetComponent<Image>();
            if (img != null && !img.raycastTarget)
                img.raycastTarget = true;

            // add a debug listener so we can see clicks and optionally call ShopButtonToggles methods
            Button localB = b;
            localB.onClick.AddListener(() => OnAutoButtonClick(localB, toggles));
        }

        Debug.Log($"ShopUiAutoWire: wired {buttons.Length} buttons under {shopCanvas.gameObject.name}.");
    }

    private void OnAutoButtonClick(Button b, ShopButtonToggles toggles)
    {
        Debug.Log($"ShopUiAutoWire: Button clicked {b.name}");

        if (toggles == null)
            toggles = FindObjectOfType<ShopButtonToggles>();
        if (toggles == null)
        {
            Debug.LogWarning("ShopUiAutoWire: no ShopButtonToggles found in scene to handle navigation buttons.");
            return;
        }

        string lower = b.name.ToLower();
        if (lower.Contains("item"))
        {
            toggles.OpenItemShop();
            return;
        }
        if (lower.Contains("weapon") || lower.Contains("weapons"))
        {
            toggles.OpenWeaponsShop();
            return;
        }
        if (lower.Contains("armour") || lower.Contains("armor"))
        {
            toggles.OpenArmourShop();
            return;
        }

        // Not a navigation button: do nothing else. ShopSlot buy buttons still work as before.
    }
}
