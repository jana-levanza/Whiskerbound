using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public bool isTutorialComplete = false;
    public string[] objectsToDestroyNames;
    public string cutsceneManagerName = "CutsceneManager";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Hut" && !isTutorialComplete)
        {
            CompleteTutorial();
        }

        if (isTutorialComplete)
        {
            DestroyTutorialObjects();
            if (scene.name == "SpawnPoint2.0" || scene.name == "SpawnPoint" || scene.name == "SecludedWoods")
            {
                DestroyCutsceneManager();
            }
        }
    }

    public void CompleteTutorial()
    {
        isTutorialComplete = true;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.ShowGameplayUI();
        }

        DestroyTutorialObjects();
    }

    private void DestroyTutorialObjects()
    {
        foreach (string objName in objectsToDestroyNames)
        {
            if (!string.IsNullOrEmpty(objName))
            {
                GameObject objToDestroy = GameObject.Find(objName);

                if (objToDestroy != null)
                {
                    Destroy(objToDestroy);
                    Debug.Log("Tutorial object destroyed: " + objName);
                }
            }
        }
    }

    private void DestroyCutsceneManager()
    {
        if (!string.IsNullOrEmpty(cutsceneManagerName))
        {
            GameObject cutsceneManager = GameObject.Find(cutsceneManagerName);

            if (cutsceneManager != null)
            {
                Destroy(cutsceneManager);
                Debug.Log("Cutscene Manager destroyed dahil tapos na ang tutorial at nasa " + SceneManager.GetActiveScene().name + " tayo.");
            }
        }
    }
}