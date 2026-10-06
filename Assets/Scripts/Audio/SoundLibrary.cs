using UnityEngine;

[System.Serializable]
public struct SoundGroup
{
    public string groupName;    
    public AudioClip[] audioClips; 
}

[CreateAssetMenu(fileName = "SoundLibrary", menuName = "Audio/Sound Library")]
public class SoundLibrary : ScriptableObject
{
    public SoundGroup[] bgmGroups;
    public SoundGroup[] ambianceGroups;

    public SoundGroup[] sfxCombat;

    public SoundGroup[] sfxEvent;
    public SoundGroup[] sfxPlayer;
    public SoundGroup[] sfxUI;

    public AudioClip GetRandomClip(string nameToSearch, SoundGroup[] category)
    {
        foreach (SoundGroup group in category)
        {
            if (group.groupName == nameToSearch)
            {
                if (group.audioClips.Length == 0) return null;
                return group.audioClips[Random.Range(0, group.audioClips.Length)];
            }
        }
        return null;
    }
}