using UnityEngine;
using TMPro;

// Attach to your Shop Canvas GameObject to disable Raycast Target on text labels
// This prevents TMP text from intercepting pointer events intended for Buttons.
public class ShopUiFixer : MonoBehaviour
{
    public Canvas shopCanvas;

    void Start()
    {
        if (shopCanvas == null)
            shopCanvas = GetComponent<Canvas>();

        if (shopCanvas == null)
        {
            Debug.LogWarning("ShopUiFixer: no Canvas assigned or found on this GameObject.");
            return;
        }

        var texts = shopCanvas.GetComponentsInChildren<TMP_Text>(true);
        int changed = 0;
        foreach (var t in texts)
        {
            if (t == null) continue;
            if (t.raycastTarget)
            {
                t.raycastTarget = false;
                changed++;
            }
        }

        Debug.Log($"ShopUiFixer: disabled raycastTarget on {changed} TMP_Text components under {shopCanvas.gameObject.name}");
    }
}
