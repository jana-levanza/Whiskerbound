using UnityEngine;

public enum StoryStateType
{
    BeforeFarmQuest,
    AfterFarmQuest,
    BeforeMazeQuest,
    AfterMazeQuest
}

public class StoryState : MonoBehaviour
{
    public static StoryState Instance;
    public StoryStateType currentStoryState;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
}