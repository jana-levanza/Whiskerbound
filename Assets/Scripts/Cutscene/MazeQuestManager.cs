using UnityEngine;

public class MazeLevelManager : MonoBehaviour
{
    public static MazeLevelManager Instance;
    private int requiredBeads = 12;
    public CutsceneMazeQuest questCutscene;

    private bool isCutscenePlayed = false;

    private PlayerAttributes playerAttributes;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        playerAttributes = FindAnyObjectByType<PlayerAttributes>();
    }

    private void Update()
    {
        if (playerAttributes == null)
        {
            playerAttributes = FindAnyObjectByType<PlayerAttributes>();
            if (playerAttributes == null) return;
        }

        if (!isCutscenePlayed && playerAttributes.BeadsEarned >= requiredBeads)
        {
            isCutscenePlayed = true;
            MazeCompleted();
        }
    }

    private void MazeCompleted()
    {
        if (questCutscene != null)
            questCutscene.PlaySequence();
    }
}