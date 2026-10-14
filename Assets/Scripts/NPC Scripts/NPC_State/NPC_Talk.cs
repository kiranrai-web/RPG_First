using UnityEngine;

public class NPC_Talk : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    public  Animator interactAnim;
    public DialogueSO dialogueSO;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        if (rb != null)
            rb.linearVelocity = Vector2.zero;
        if (anim != null)
            anim.Play("Idle");
        if (interactAnim != null && interactAnim.gameObject.activeInHierarchy)
            interactAnim.Play("Open");
    }

    private void OnDisable()
    {
        if (interactAnim != null && interactAnim.gameObject.activeInHierarchy)
            interactAnim.Play("Close");
    }

    private void Update() 
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (DialogueManager.Instance.isDialogueActive)
                DialogueManager.Instance.AdvanceDialogue();
            else
                DialogueManager.Instance.StartDialogue(dialogueSO);
        }
    }
}
