using UnityEngine;
using UnityEngine.EventSystems;

// Ensures an EventSystem with a StandaloneInputModule exists at runtime.
// This helps projects using the legacy Input Manager where UI won't respond without a StandaloneInputModule.
public static class EnsureEventSystem
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void Ensure()
    {
        if (EventSystem.current == null)
        {
            var go = new GameObject("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            Debug.Log("EnsureEventSystem: Created EventSystem with StandaloneInputModule.");
            return;
        }

        var module = EventSystem.current.gameObject.GetComponent<BaseInputModule>();
        if (module == null)
        {
            EventSystem.current.gameObject.AddComponent<StandaloneInputModule>();
            Debug.Log("EnsureEventSystem: Added StandaloneInputModule to existing EventSystem.");
            return;
        }

        Debug.Log("EnsureEventSystem: EventSystem present with input module: " + module.GetType().Name);
    }
}
