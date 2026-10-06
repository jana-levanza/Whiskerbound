using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct QuestDialogueMap
{
    public StoryStateType state;
    public GameDialogueData dialogue;
}

public class NPCInteract : MonoBehaviour, IInteractable
{
    private SpriteRenderer spriteRenderer;
    private Material originalMaterial;
    private NPCController npcController;

    private GameDialogueData dialogueToPlay;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private List<QuestDialogueMap> dialogueList;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        npcController = GetComponent<NPCController>();

        originalMaterial = spriteRenderer.material;
    }

    public void Interact(Vector3 playerPosition)
    {
        npcController.Interact(playerPosition);

        StoryStateType currentState = StoryState.Instance.currentStoryState;

        foreach (QuestDialogueMap map in dialogueList)
            if (map.state == currentState)
            {
                dialogueToPlay = map.dialogue;
                DialogueManager.Instance.StartDialogue(dialogueToPlay);
            }
    }

    public void ShowOutline()
    {
        spriteRenderer.material = outlineMaterial;
    }

    public void HideOutline()
    {
        spriteRenderer.material = originalMaterial;
    }
}