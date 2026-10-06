using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingController : MonoBehaviour
{
    [SerializeField] private GameObject fadeInCanvas;
    private float minimumLoadingTime = 2.5f;

    private void Start()
    {
        StartCoroutine(LoadTargetScene());
    }

    private IEnumerator LoadTargetScene()
    {
        string target = string.IsNullOrEmpty(SceneLoader.TargetSceneName)
            ? "MainMenu"
            : SceneLoader.TargetSceneName;

        float timer = 0f;

        AsyncOperation operation = SceneManager.LoadSceneAsync(target, LoadSceneMode.Single);

        operation.allowSceneActivation = false;


        while (true)
        {
            timer += Time.deltaTime;

            bool sceneLoaded = operation.progress >= 0.9f;
            bool timePassed = timer >= minimumLoadingTime;

            if (sceneLoaded && timePassed)
                break;

            yield return null;
        }

        Instantiate(fadeInCanvas);  
        yield return new WaitForSeconds(1f);

        operation.allowSceneActivation = true;
    }
}