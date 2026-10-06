using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    [SerializeField] private PlayerProgressData progressData;

    private int maxHp = 3;
    private int hp;

    public int MaxHP
    {
        get => maxHp;
        set
        {
            maxHp = Mathf.Max(0, value);
            hp = Mathf.Clamp(hp, 0, maxHp);
        }
    }
    public int HP
    {
        get => hp;
        set
        {
            hp = Mathf.Clamp(value, 0, maxHp);

            if (progressData != null)
            {
                if (progressData.activeCharacter == "Shuyi")
                    progressData.currentShuyiHP = hp;
                else
                    progressData.currentYoichiHP = hp;
            }
        }
    }

    public int BeadsEarned
    {
        get => progressData != null ? progressData.totalBeads : 0;
    }
    public int KeyEarned
    {
        get => progressData != null ? progressData.keyCount : 0;
    }
    public void AddKey(int amount)
    {
        if (progressData != null) progressData.keyCount += amount;
    }
    public void AddBead(int amount)
    {
        if (progressData != null) progressData.totalBeads += amount;
    }
}