using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private RectTransform gameOverTextRect;
    [SerializeField] private CanvasGroup panelCanvasGroup;
    [SerializeField] private TextMeshProUGUI continueText;

    [SerializeField] private PlayerProgressData progressData;
    [SerializeField] private CharacterData shuyiData;
    [SerializeField] private CharacterData yoichiData;

    private Vector2 startPosition = new Vector2(0, 0);
    private Vector2 targetPosition = new Vector2(0, 200);
    private float animationDuration = 1.5f;
    private float flickerSpeed = 4f;

    private bool canRestart = false;
    private bool isFlickering = false;
    private bool isRestarting = false;

    public static string levelToLoad = "";

    private void OnEnable()
    {
        isRestarting = false;

        gameOverTextRect.anchoredPosition = startPosition;
        panelCanvasGroup.alpha = 0f;

        Color c = continueText.color;
        c.a = 0f;
        continueText.color = c;

        canRestart = false;
        isFlickering = false;

        StartCoroutine(GameOverSequence());
    }
    private void Update()
    {
        if (isFlickering)
        {
            float alpha = (Mathf.Sin(Time.unscaledTime * flickerSpeed) + 1f) / 2f;
            Color c = continueText.color;
            c.a = Mathf.Clamp(alpha, 0.1f, 1f);
            continueText.color = c;
        }

        if (canRestart && !isRestarting && IsAnyInputPressed())
            StartCoroutine(RestartSequence());
    }

    IEnumerator GameOverSequence()
    {
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / animationDuration;
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);

            gameOverTextRect.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, smoothStep);
            panelCanvasGroup.alpha = Mathf.Lerp(0f, 1f, smoothStep);

            yield return null;
        }

        gameOverTextRect.anchoredPosition = targetPosition;
        panelCanvasGroup.alpha = 1f;

        isFlickering = true;
        canRestart = true;
    }
    IEnumerator RestartSequence()
    {
        isRestarting = true;
        canRestart = false;

        panelCanvasGroup.alpha = 0f;

        if (DeathLight.Instance != null)
            yield return StartCoroutine(DeathLight.Instance.ShrinkDeathLightRoutine());

        Time.timeScale = 1f;

        if (progressData != null && shuyiData != null && yoichiData != null)
        {
            progressData.ResetHPToMax(shuyiData.maxHP, yoichiData.maxHP);
            levelToLoad = progressData.respawnScene;
        }
        else
            levelToLoad = SceneManager.GetActiveScene().name;

        if (UIManager.Instance != null)
            UIManager.Instance.HideGameOver();
        SceneLoader.Load(levelToLoad);
        UIManager.Instance.ShowGameplayUI();
    }

    private bool IsAnyInputPressed()
    {
        bool mouseClick = Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame;
        bool touch = Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame;
        bool keyboard = Keyboard.current != null && Keyboard.current.anyKey.wasPressedThisFrame;

        return mouseClick || touch || keyboard;
    }
}