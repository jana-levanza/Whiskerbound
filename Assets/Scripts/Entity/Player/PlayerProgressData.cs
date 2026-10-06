using UnityEngine;

[CreateAssetMenu(menuName = "Whiskerbound/Player Progress Data")]
public class PlayerProgressData : ScriptableObject
{
    [Header("Inventory & Currency")]
    public int totalBeads;
    public int keyCount;

    [Header("Character Data")]
    public int currentShuyiHP = 3;
    public int currentYoichiHP = 3;
    public string activeCharacter = "Shuyi";

    [Header("Respawn Checkpoint")]
    public string respawnScene;
    public string respawnDoorID;
    public string currentDoorID;

    public void ResetHPToMax(int shuyiMax, int yoichiMax)
    {
        currentShuyiHP = shuyiMax;
        currentYoichiHP = yoichiMax;
    }
}