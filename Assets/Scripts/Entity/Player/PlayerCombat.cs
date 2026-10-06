using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private PlayerMovement movement;

    public bool isAttacking = false;
    private bool hasExecutedAttack = false;

    public IWeapon currentWeapon;

    void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<PlayerMovement>();
    }

    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.performed && !isAttacking && !movement.isStunned && !movement.isDashing && !PauseController.isGamePaused)
        {
            isAttacking = true;
            hasExecutedAttack = false;
            rb.linearVelocity = Vector2.zero;
            anim.SetTrigger("attack");
        }
    }

    public void ExecuteAttack()
    {
        if (hasExecutedAttack) return;

        hasExecutedAttack = true;

        currentWeapon.PerformAttack(movement.facingDirection);

            
        if (currentWeapon is MeleeWeapon)
            AudioManager.Instance.PlaySFX("AttackYoichi", "PLAYER");
        else
        {
            CameraShake.Instance.Shake(0.3f);
            AudioManager.Instance.PlaySFX("AttackShuyi", "PLAYER");
        }
    }

    public void FinishAttack()
    {
        isAttacking = false;
    }
}
