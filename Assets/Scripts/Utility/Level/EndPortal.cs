using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement; // Para sa pag-load ng next scene

public class EndPortal : MonoBehaviour
{
    [SerializeField] private PlayerProgressData progressData;
    [SerializeField] private GameObject fadeInCanvas;
    private int requiredKeys = 1;

    private bool isTransitioning = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isTransitioning)
        {
            if (progressData != null && progressData.keyCount >= requiredKeys)
            {
                isTransitioning = true;
                UIManager.Instance.ShowInspect("You have completed the Game!");
                EnterPortal();
            }
            else
                UIManager.Instance.ShowInspect("You need a key to open this path.");
        }
    }

    private IEnumerator EnterPortal()
    {
        AudioManager.Instance.PlaySFX("TaskComplete", "EVENT");

        Instantiate(fadeInCanvas);
        yield return new WaitForSecondsRealtime(1f);
        SceneLoader.Load("MainMenu");
    }
}