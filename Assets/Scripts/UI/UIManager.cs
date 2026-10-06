using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private GameObject healthPanel;
    [SerializeField] private GameObject beadPanel;
    [SerializeField] private GameObject switchPanel;
    [SerializeField] private GameObject mobileControl;
    [SerializeField] private GameObject inspectPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI inspectText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            inspectPanel.SetActive(false);
            gameOverPanel.SetActive(false);
            HideGameplayUI();
        }
        else if (Instance != this)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }
    }

    public void ShowInspect(string message)
    {
        inspectText.text = message;
        HideGameplayUI();
        inspectPanel.SetActive(true);
        PauseController.SetPause(true);
    }

    public void HideInspect()
    {
        inspectPanel.SetActive(false);
        ShowGameplayUI();
        PauseController.SetPause(false);
    }

    public void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        HideGameplayUI();
    }

    public void HideGameOver()
    {
        gameOverPanel.SetActive(false);
    }

    public void HideGameplayUI() { Debug.Log("HIDE"); mobileControl.SetActive(false); healthPanel.SetActive(false); beadPanel.SetActive(false); switchPanel.SetActive(false); }
    public void ShowGameplayUI() { Debug.Log("SHOW"); mobileControl.SetActive(true); healthPanel.SetActive(true); beadPanel.SetActive(true); switchPanel.SetActive(true); }
}