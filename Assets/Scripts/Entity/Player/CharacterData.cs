using UnityEngine;
using UnityEngine.U2D.Animation;

[CreateAssetMenu(menuName = "Whiskerbound/Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public SpriteLibraryAsset spriteLibraryAsset;
    public Sprite healthIconSprite;
    public GameObject heartPrefab;

    public RuntimeAnimatorController animatorController;

    public int maxHP = 3;

    [HideInInspector] public int currentHP;
}