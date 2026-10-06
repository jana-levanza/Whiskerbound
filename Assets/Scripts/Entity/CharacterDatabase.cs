using UnityEngine;

[System.Serializable]
public struct CharacterProfile
{
    public string characterName;
    public Sprite portrait;
    public AudioClip defaultVoice;  
}

[CreateAssetMenu(fileName = "CharacterDatabase", menuName = "Dialogue System/Character Database")]
public class CharacterDatabase : ScriptableObject
{
    public CharacterProfile[] characters;

    public CharacterProfile GetProfile(string nameToSearch)
    {
        foreach (CharacterProfile profile in characters)
            if (profile.characterName.ToLower() == nameToSearch.ToLower())
                return profile;

        return new CharacterProfile();
    }
}
