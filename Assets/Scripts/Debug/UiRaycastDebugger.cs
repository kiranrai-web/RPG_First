using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;

// Attach to the Canvas (or a parent) that has a GraphicRaycaster to see which UI elements are under the pointer when you click.
public class UiRaycastDebugger : MonoBehaviour
{
    public GraphicRaycaster raycaster;
    public EventSystem eventSystem;

    void Start()
    {
        if (raycaster == null)
            raycaster = GetComponent<GraphicRaycaster>();
        if (eventSystem == null)
            eventSystem = EventSystem.current;

        Debug.Log($"UiRaycastDebugger initialized. Raycaster={(raycaster!=null?raycaster.gameObject.name:"null")} EventSystem={(eventSystem!=null?eventSystem.gameObject.name:"null")}");
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (eventSystem == null)
            {
                Debug.LogWarning("UiRaycastDebugger: no EventSystem in scene.");
                return;
            }

            var pointer = new PointerEventData(eventSystem) { position = Input.mousePosition };
            var results = new List<RaycastResult>();

            // Try using provided raycaster or the first GraphicRaycaster in scene
            if (raycaster != null)
            {
                raycaster.Raycast(pointer, results);
            }
            else
            {
                var all = FindObjectsOfType<GraphicRaycaster>();
                foreach (var rc in all)
                    rc.Raycast(pointer, results);
            }

            Debug.Log($"UiRaycastDebugger: pointer at {pointer.position}, hits={results.Count}");
            for (int i = 0; i < results.Count; i++)
            {
                var r = results[i];
                Debug.Log($"  [{i}] {r.gameObject.name} (module={r.module.GetType().Name} depth={r.depth} index={r.index})");
            }

            if (results.Count == 0)
                Debug.Log("  No UI elements under pointer.");
        }
    }
}
