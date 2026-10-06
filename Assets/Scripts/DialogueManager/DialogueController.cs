using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueController : MonoBehaviour
{
    public void TriggerNextLine()
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.NextLine();
        }
    }

    public void OnBackgroundClicked()
    {
        if (DialogueManager.Instance != null && DialogueManager.Instance.isDialogueActive)
        {
            TriggerNextLine();
        }
    }
}