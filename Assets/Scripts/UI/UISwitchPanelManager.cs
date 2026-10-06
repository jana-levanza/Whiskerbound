using UnityEngine;

public class UISwitchPanelManager : MonoBehaviour
{
    [SerializeField] private UISwitchButton shuyiButton;
    [SerializeField] private UISwitchButton yoichiButton;
    [SerializeField] private PlayerProgressData progressData;

    private void Start()
    {
        string activeChar = progressData != null ? progressData.activeCharacter : "Shuyi";
        UpdateUI(activeChar);
    }

    public void OnCharacterSelected(string charName)
    {
        UpdateUI(charName);

        // Hanapin ang player at ipa-switch!
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterSwitch charSwitch = player.GetComponent<CharacterSwitch>();
            if (charSwitch != null) 
                charSwitch.ForceSwitchCharacter(charName); 
        }
    }

    public void UpdateUI(string activeCharacter)
    {
        shuyiButton.SetActiveState(activeCharacter == "Shuyi");
        yoichiButton.SetActiveState(activeCharacter == "Yoichi");
    }
}