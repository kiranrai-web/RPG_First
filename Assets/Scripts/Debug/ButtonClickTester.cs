using UnityEngine;
using UnityEngine.EventSystems;

// Attach this to any UI Button GameObject to log pointer clicks
public class ButtonClickTester : MonoBehaviour, IPointerClickHandler
{
    private void Start()
    {
        Debug.Log($"ButtonClickTester attached to {gameObject.name}");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log($"ButtonClickTester: {gameObject.name} clicked by {eventData.button}");
    }
}
