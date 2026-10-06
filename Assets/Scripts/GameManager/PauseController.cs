using UnityEngine;
using UnityEngine.InputSystem;

public class PauseController : MonoBehaviour
{
    public static bool isGamePaused { get; private set; } = false;

    public static void SetPause(bool pause)
    {
        isGamePaused = pause;

        if (isGamePaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            SetPause(!isGamePaused);
    }
    private void OnDestroy()
    {
        Time.timeScale = 1f;
    }
}