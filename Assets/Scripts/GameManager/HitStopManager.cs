using UnityEngine;
using System.Collections;

public class HitStopManager : MonoBehaviour
{
    public static HitStopManager Instance;

    private bool isHitStopping = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void TriggerHitStop(float duration)
    {
        if (PauseController.isGamePaused) return;
        if (isHitStopping) return;

        StartCoroutine(HitStopRoutine(duration));
    }

    private IEnumerator HitStopRoutine(float duration)
    {
        isHitStopping = true;

        Time.timeScale = 0.05f;

        yield return new WaitForSecondsRealtime(duration);

        if (!PauseController.isGamePaused)
            Time.timeScale = 1f;

        isHitStopping = false;
    }
}