using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public CanvasGroup canvasGroup;
    public Image portrait;
    public TMP_Text actorName;
    public TMP_Text dialogueText;

    private DialogueSO currentDialogue;
    private int dialogueIndex;
    public bool isDialogueActive;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        canvasGroup.alpha = 0;
       canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

    }

    public void StartDialogue(DialogueSO dialogueSO)
    {
        //if (dialogueSO == null)
        //{
        //    Debug.LogWarning("StartDialogue called with null DialogueSO");
        //    return;
        //}

        //if (dialogueSO.lines == null || dialogueSO.lines.Length == 0)
        //{
        //    Debug.LogWarning("StartDialogue called with DialogueSO that has no lines.");
        //    return;
        //}

        currentDialogue = dialogueSO;
        dialogueIndex = 0;
        isDialogueActive = true;

        canvasGroup.alpha = 1;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        ShowDialogue();
    }

    public void AdvanceDialogue()
    {
        //if (currentDialogue == null || currentDialogue.lines == null)
        //{
        //    EndDialogue();
        //    return;
        //}

        if (dialogueIndex < currentDialogue.lines.Length)
            ShowDialogue();
        else
            EndDialogue();
    }

    private void ShowDialogue() 
    {
        //if (currentDialogue == null || currentDialogue.lines == null || dialogueIndex >= currentDialogue.lines.Length)
        //{
        //    EndDialogue();
        //    return;
        //}

        //DialogueLine line = currentDialogue.lines[dialogueIndex];
        //if (line == null)
        //{
        //    Debug.LogWarning($"DialogueManager: DialogueLine is null at index {dialogueIndex}");
        //    EndDialogue();
        //    return;
        //}

        //// Assign speaker info if available
        //if (line.speaker != null)
        //{
        //    if (portrait != null)
        //        portrait.sprite = line.speaker.portrait;
        //    if (actorName != null)
        //        actorName.text = line.speaker.actorName;
        //}
        //else
        //{
        //    if (actorName != null)
        //        actorName.text = string.Empty;
        //    if (portrait != null)
        //        portrait.sprite = null;
        //}

        //if (dialogueText != null)
        //    dialogueText.text = line.text ?? string.Empty;

        //if (canvasGroup != null)
        //{
        //    canvasGroup.alpha = 1; // Show the dialogue UI
        //    canvasGroup.interactable = true;
        //    canvasGroup.blocksRaycasts = true;
        //}

        DialogueLine line = currentDialogue.lines[dialogueIndex];

        portrait.sprite = line.speaker.portrait;
        actorName.text = line.speaker.actorName;
        dialogueText.text = line.text;

        dialogueIndex++;
    }

    private void EndDialogue()
    {
        dialogueIndex = 0;
        isDialogueActive = false;
        // Optionally, you can clear the UI elements here
        canvasGroup.alpha = 0; // Hide the dialogue UI initially
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
}
