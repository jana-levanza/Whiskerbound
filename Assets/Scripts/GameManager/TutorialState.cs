using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTutorialState : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private PlayerMovement playerMovement;
    private PlayerInput playerInput;
    private Collider2D playerCollider;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
        playerInput = GetComponent<PlayerInput>();
        playerCollider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        CheckTutorialState();
    }

    public void CheckTutorialState()
    {
        if (GameManager.Instance != null)
        {
            bool isDone = GameManager.Instance.isTutorialComplete;

            spriteRenderer.enabled = isDone;
            playerMovement.enabled = isDone;
            playerInput.enabled = isDone;
            playerCollider.enabled = isDone;
        }
    }

    public void UnlockPlayer()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.isTutorialComplete = true;

        spriteRenderer.enabled = true;
        playerMovement.enabled = true;
        playerInput.enabled = true;
        playerCollider.enabled = true;

    }
}