using UnityEngine;

public interface IInteractable
{
    void Interact(Vector3 playerPosition);
    void ShowOutline();
    void HideOutline();
}