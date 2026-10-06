using UnityEngine;

[CreateAssetMenu(fileName = "NewDialogue", menuName = "Dialogue System/Game Dialogue Data")]
public class GameDialogueData : ScriptableObject
{
    public float typingSpeed = 0.05f;
    public float autoProgressDelay = 1.5f;

    public bool[] autoProgressLines;
    public string[] dialogueLines;
}