//using System.Collections;
//using UnityEngine;
//using UnityEngine.Rendering.Universal;

//public class GameOverManager : MonoBehaviour
//{
//    public static GameOverManager Instance { get; private set; }

//    [Header("Lighting Setup")]
//    [SerializeField] private Light2D globalLight;
//    [SerializeField] private Light2D deathSpotlight;
//    [SerializeField] private float lightTransitionDuration = 1.5f;

//    [Header("UI Setup")]
//    [SerializeField] private CanvasGroup gameOverCanvasGroup;

//    private void Awake()
//    {
//        if (Instance == null) Instance = this;
//        else Destroy(gameObject);

//        gameOverCanvasGroup.alpha = 0f;
//        gameOverCanvasGroup.interactable = false;
//        gameOverCanvasGroup.blocksRaycasts = false;
//    }

//    public void TriggerGameOverSequence()
//    {
//        StartCoroutine(GameOverRoutine());
//    }

//    private IEnumerator GameOverRoutine()
//    {
//        float elapsed = 0f;
//        float startGlobalIntensity = globalLight.intensity;
//        float targetSpotlightIntensity = 1.5f; 

//        while (elapsed < lightTransitionDuration)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            float t = elapsed / lightTransitionDuration;

//            globalLight.intensity = Mathf.Lerp(startGlobalIntensity, 0f, t);
//            deathSpotlight.intensity = Mathf.Lerp(0f, targetSpotlightIntensity, t);

//            yield return null;
//        }

//        globalLight.intensity = 0f;
//        deathSpotlight.intensity = targetSpotlightIntensity;

//        yield return new WaitForSecondsRealtime(1f); 

//        elapsed = 0f;
//        float uiFadeDuration = 1f;
//        while (elapsed < uiFadeDuration)
//        {
//            elapsed += Time.unscaledDeltaTime;
//            gameOverCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / uiFadeDuration);
//            yield return null;
//        }

//        gameOverCanvasGroup.interactable = true;
//        gameOverCanvasGroup.blocksRaycasts = true;
//    }
//}