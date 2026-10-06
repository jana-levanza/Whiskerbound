using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public Vector2 facingDirection = new Vector2(0, -1);
    private float moveSpeed = 3f;

    private PlayerCombat playerCombat;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 moveInput;

    [SerializeField] private TrailRenderer dashTrail;
    private float dashSpeed = 12f;
    private float dashDuration = 0.2f;
    private float dashCooldown = 5f;
    private float dashCooldownTimer = 0f;

    private float bounceForce = 6f;
    private float bounceDuration = 0.4f;
    public bool isBouncing = false;
    public bool isStunned = false;
    public bool isDashing = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerCombat = GetComponent<PlayerCombat>();
        dashTrail.emitting = false;
    }

    void Update()
    {
        if (isBouncing) return;

        if (dashCooldownTimer > 0)
            dashCooldownTimer -= Time.deltaTime;

        if (isStunned) return;
        if (isDashing) return;

        if (PauseController.isGamePaused)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        if (playerCombat != null && playerCombat.isAttacking)
        {
            rb.linearVelocity = Vector2.zero;
            animator.SetBool("isWalking", false);
            return;
        }

        rb.linearVelocity = moveInput * moveSpeed;
        bool isWalking = rb.linearVelocity.magnitude > 0;
        animator.SetBool("isWalking", isWalking);

        if (isWalking)
        {
            facingDirection = moveInput.normalized;

            animator.SetFloat("moveX", facingDirection.x);
            animator.SetFloat("moveY", facingDirection.y);
            animator.SetFloat("wMoveX", moveInput.x);
            animator.SetFloat("wMoveY", moveInput.y);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (PauseController.isGamePaused || isDashing)
        {
            moveInput = Vector2.zero;
            return;
        }

        moveInput = context.ReadValue<Vector2>();

        if (context.canceled)
        {
            animator.SetBool("isWalking", false);
        }
    }
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && !isDashing && dashCooldownTimer <= 0 && !isStunned && !PauseController.isGamePaused && !playerCombat.isAttacking)
        {
            AudioManager.Instance.PlaySFX("Dash", "PLAYER");
            StartCoroutine(DashRoutine());
        }
    }

    IEnumerator DashRoutine()
    {
        isDashing = true;
        dashCooldownTimer = dashCooldown;
        animator.SetBool("isWalking", false);

        dashTrail.emitting = true;

        Vector2 dashDirection = moveInput.magnitude > 0 ? moveInput.normalized : facingDirection.normalized;
        rb.linearVelocity = dashDirection * dashSpeed;

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;
        rb.linearVelocity = Vector2.zero;

        dashTrail.emitting = false;
    }
    IEnumerator BounceRoutine(Vector2 bounceDirection)
    {
        isBouncing = true;
        float timer = 0f;

        while (timer < bounceDuration)
        {
            float t = timer / bounceDuration;

            float easeOutT = 1f - (1f - t) * (1f - t);
            float currentForce = Mathf.Lerp(bounceForce, 0f, easeOutT);

            rb.linearVelocity = bounceDirection * currentForce;

            timer += Time.deltaTime;

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;
        isBouncing = false;
    }
    public void SetDashColor(string hexColor)
    {
        Color newColor;
        if (ColorUtility.TryParseHtmlString(hexColor, out newColor))
        {
            Gradient gradient = new Gradient();

            GradientColorKey[] colorKeys = new GradientColorKey[2];
            colorKeys[0] = new GradientColorKey(newColor, 0.0f);
            colorKeys[1] = new GradientColorKey(newColor, 1.0f);

            GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
            alphaKeys[0] = new GradientAlphaKey(1.0f, 0.0f);
            alphaKeys[1] = new GradientAlphaKey(0.0f, 1.0f);

            gradient.SetKeys(colorKeys, alphaKeys);
            dashTrail.colorGradient = gradient;
        }
    }
    public void ApplyBounce(Vector2 bounceDirection)
    {
        if (!isBouncing && !isStunned)
            StartCoroutine(BounceRoutine(bounceDirection));
    }

    private void OnDisable()
    {
        rb.linearVelocity = Vector2.zero;
        animator.SetBool("isWalking", false);
        moveInput = Vector2.zero;
        GetComponent<PlayerInput>().enabled = false;
    }
    private void OnEnable()
    {
        GetComponent<PlayerInput>().enabled = true;
    }
}