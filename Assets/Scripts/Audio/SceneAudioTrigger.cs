using UnityEngine;

public class SceneAudioTrigger : MonoBehaviour
{
    public string bgmName;     
    public string ambianceName; 

    private void Start()
    {
        if (AudioManager.Instance != null)
        {
            if (!string.IsNullOrEmpty(bgmName))
                AudioManager.Instance.PlayBGM(bgmName);

            if (!string.IsNullOrEmpty(ambianceName))
                AudioManager.Instance.PlayAmbiance(ambianceName);
        }
    }
}