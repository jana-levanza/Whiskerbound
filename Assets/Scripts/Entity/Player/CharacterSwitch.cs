using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.U2D.Animation;

public class CharacterSwitch : MonoBehaviour
{
    private PlayerAttributes attributes;
    private PlayerCombat playerCombat;
    private PlayerMovement playerMovement;
    private SpriteLibrary spriteLibrary;
    private Animator anim;

    private MeleeWeapon yoichiWeapon;
    private ProjectileWeapon shuyiWeapon;

    [SerializeField] private CharacterData shuyi;
    [SerializeField] private CharacterData yoichi;
    [SerializeField] private PlayerProgressData progressData;

    private CharacterData current;

    private void Awake()
    {
        attributes = GetComponent<PlayerAttributes>();
        playerCombat = GetComponent<PlayerCombat>();
        playerMovement = GetComponent<PlayerMovement>();
        spriteLibrary = GetComponent<SpriteLibrary>();
        anim = GetComponent<Animator>();

        yoichiWeapon = GetComponentInChildren<MeleeWeapon>();
        shuyiWeapon = GetComponentInChildren<ProjectileWeapon>();

        if (progressData != null && progressData.activeCharacter == yoichi.characterName)
            current = yoichi;
        else
            current = shuyi;
    }

    private void Start()
    {
        Apply(current);
        EquipWeapon();
    }

    public void OnSwitch(InputAction.CallbackContext context)
    {
        if (context.performed && !PauseController.isGamePaused)
        {
            string targetCharacter = (current == shuyi) ? yoichi.characterName : shuyi.characterName;
            ForceSwitchCharacter(targetCharacter);
        }
    }

    public void ForceSwitchCharacter(string charName)
    {
        if (current != null && current.characterName == charName)
            return;

        AudioManager.Instance.PlaySFX("Switch", "PLAYER");

        if (playerCombat != null)
            playerCombat.isAttacking = false;

        if (playerMovement != null)
        {
            playerMovement.isDashing = false;
            playerMovement.isBouncing = false;
            playerMovement.isStunned = false;
        }

        current = (charName == "Shuyi") ? shuyi : yoichi;

        if (progressData != null)
            progressData.activeCharacter = current.characterName;

        Apply(current);
        EquipWeapon();
    }

    private void EquipWeapon()
    {
        if (current == shuyi)
            playerCombat.currentWeapon = shuyiWeapon;
        else
            playerCombat.currentWeapon = yoichiWeapon;
    }

    private void Apply(CharacterData data)
    {
        attributes.MaxHP = data.maxHP;

        if (progressData != null)
            attributes.HP = (data == shuyi) ? progressData.currentShuyiHP : progressData.currentYoichiHP;

        spriteLibrary.spriteLibraryAsset = data.spriteLibraryAsset;
            
        if (anim != null && data.animatorController != null)
            anim.runtimeAnimatorController = data.animatorController;

        HealthBarBehaviour currentHealthUI = Object.FindFirstObjectByType<HealthBarBehaviour>();
        if (currentHealthUI != null)
            currentHealthUI.ApplyUI(data.healthIconSprite, data.heartPrefab);

        if (playerMovement != null)
        {
            if (data == shuyi)
                playerMovement.SetDashColor("#8B3662");
            else if (data == yoichi)
                playerMovement.SetDashColor("#4C468B");
        }
    }
    public void RefreshUI() => Apply(current);
}