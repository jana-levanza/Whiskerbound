using System.Collections;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private GameObject fadeInCanvas;
    public void StartGame()
    {
        StartCoroutine(fadeIn());
        
    }

    public void LoadGame()
    {
        StartCoroutine(fadeIn());
    }

    public void QuitGame()
    {
        Application.Quit();

        //UnityEditor.EditorApplication.isPlaying = false;

    }

    IEnumerator fadeIn()
    {
        Instantiate(fadeInCanvas);
        yield return new WaitForSeconds(1f);

        SceneLoader.Load(sceneToLoad);
    }
}