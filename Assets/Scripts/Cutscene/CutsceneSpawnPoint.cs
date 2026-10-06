using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CutsceneActionType
{
    Wait,
    Dialogue,
    MoveActors
}

[System.Serializable]
public class ActorMoveCommand
{
    public Transform actorTransform;
    public Animator actorAnimator;
    public Transform targetWaypoint;
    [HideInInspector] public bool hasArrived;
}

[System.Serializable]
public class CutsceneAction
{
    public CutsceneActionType actionType;
    public float waitDuration = 1f;
    public GameDialogueData dialogueData;
    public List<ActorMoveCommand> actorsToMove = new List<ActorMoveCommand>();
}

public class CutsceneSpawnPoint : MonoBehaviour
{
    public float walkSpeed = 3f;

    public List<CutsceneAction> cutsceneSequence = new List<CutsceneAction>();

    private void Start()
    {
        StartCoroutine(PlaySequence());
    }

    private IEnumerator PlaySequence()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (CutsceneAction action in cutsceneSequence)
        {
            if (action.actionType == CutsceneActionType.Wait)
            {
                yield return new WaitForSeconds(action.waitDuration);
            }
            else if (action.actionType == CutsceneActionType.Dialogue)
            {
                if (action.dialogueData != null && DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.StartDialogue(action.dialogueData);
                    while (DialogueManager.Instance.isDialogueActive) yield return null;
                }
            }
            else if (action.actionType == CutsceneActionType.MoveActors)
            {
                if (action.actorsToMove != null && action.actorsToMove.Count > 0)
                {
                    yield return StartCoroutine(MoveActorsRoutine(action.actorsToMove));
                }
            }
        }
    }

    private IEnumerator MoveActorsRoutine(List<ActorMoveCommand> commands)
    {
        foreach (ActorMoveCommand cmd in commands)
        {
            cmd.hasArrived = false;
            if (cmd.actorTransform != null && cmd.targetWaypoint != null)
            {
                if (cmd.actorAnimator != null)
                {
                    Vector2 dir = (cmd.targetWaypoint.position - cmd.actorTransform.position).normalized;
                    cmd.actorAnimator.SetFloat("wMoveX", dir.x);
                    cmd.actorAnimator.SetFloat("wMoveY", dir.y);
                    cmd.actorAnimator.SetBool("isWalking", true);
                }
            }
            else { cmd.hasArrived = true; }
        }

        bool allArrived = false;

        while (!allArrived)
        {
            allArrived = true;

            foreach (ActorMoveCommand cmd in commands)
            {
                if (cmd.hasArrived) continue;
                allArrived = false;

                if (Vector2.Distance(cmd.actorTransform.position, cmd.targetWaypoint.position) > 0.05f)
                {
                    cmd.actorTransform.position = Vector2.MoveTowards(cmd.actorTransform.position, cmd.targetWaypoint.position, walkSpeed * Time.deltaTime);
                }
                else
                {
                    cmd.hasArrived = true;
                    if (cmd.actorAnimator != null) cmd.actorAnimator.SetBool("isWalking", false);
                }
            }
            yield return null;
        }
    }
}