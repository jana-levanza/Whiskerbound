using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1.5f;
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable currentTarget;
    void Update()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, interactRadius, interactableLayer);

        IInteractable closestInteractable = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider2D col in colliders)
        {
            IInteractable interactable = col.GetComponent<IInteractable>();
            if (interactable != null)
            {
                float distance = Vector2.Distance(transform.position, col.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closestInteractable = interactable;
                }
            }
        }

        if (closestInteractable != currentTarget)
        {
            if (currentTarget != null) currentTarget.HideOutline();

            currentTarget = closestInteractable;

            if (currentTarget != null) currentTarget.ShowOutline();
        }
    }
    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.started && currentTarget != null)
        {
            AudioManager.Instance.PlaySFX("Interact", "PLAYER");
            currentTarget.Interact(transform.position);
        }
    }
}