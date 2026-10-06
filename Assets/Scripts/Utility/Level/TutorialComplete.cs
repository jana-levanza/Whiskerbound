using UnityEngine;

public class TutorialComplete : MonoBehaviour
{
    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Actors") && !hasTriggered)
        {
            hasTriggered = true;
            if (GameManager.Instance != null)
                GameManager.Instance.CompleteTutorial();
            Destroy(gameObject);
        }
    }
}