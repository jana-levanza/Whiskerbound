using UnityEngine;

public class EnemyAttributes : MonoBehaviour
{
    [SerializeField] private int maxHp = 3;
    [SerializeField] private int hp;

    public int HP
    {
        get => hp;
        set => hp = Mathf.Clamp(value, 0, maxHp);
    }

    public int MaxHP => maxHp;

    private void Awake()
    {
        hp = maxHp;
    }
}