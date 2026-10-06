using UnityEngine;
using UnityEngine.SceneManagement;

public static class SceneLoader
{
    public static string TargetSceneName;

    public static void Load(string targetScene)
    {
        TargetSceneName = targetScene;
        SceneManager.LoadScene("LoadingScene");
    }
}