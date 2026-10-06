using UnityEngine;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    private Rigidbody2D rb;
    private PlayerAttributes attributes;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;
    private Animator anim;

    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;

    private float inVulnerability = 1.5f;

    private bool isInvincible = false;
    private bool isDead = false;

    private int playerLayer;
    private int enemyLayer;
    private void Awake()
    {
        attributes = GetComponent<PlayerAttributes>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();

        originalMaterial = spriteRenderer.material;

        playerLayer = LayerMask.NameToLayer("Player");
        enemyLayer = LayerMask.NameToLayer("Enemy");
    }

    public void TakeDamage(int amount, Vector2 attackerPosition)
    {
        if (isInvincible || isDead) return;

        attributes.HP -= amount;

        GetComponent<EntityKnockback>().Apply(attackerPosition, 1.0f);

        StartCoroutine(DamageSequence());

        if (attributes.HP <= 0)
        {
            isDead = true;
            StartCoroutine(DeathSequence());
        }
        else
        {
            AudioManager.Instance.PlaySFX("PlayerDamage", "COMBAT");
            CameraShake.Instance.Shake(1.3f);
            HitStopManager.Instance.TriggerHitStop(0.3f);
        }
    }

    IEnumerator DamageSequence()
    {
        isInvincible = true;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, true);

        spriteRenderer.material = flashMaterial;
        yield return new WaitForSeconds(0.15f);
        spriteRenderer.material = originalMaterial;

        Physics2D.IgnoreLayerCollision(playerLayer, enemyLayer, false);

        float timer = 0f;
        while (timer < inVulnerability)
        {
            Color c = spriteRenderer.color;
            c.a = (c.a == 1f) ? 0.3f : 1f;
            spriteRenderer.color = c;

            yield return new WaitForSeconds(0.1f);
            timer += 0.1f;
        }

        Color finalColor = spriteRenderer.color;
        finalColor.a = 1f;
        spriteRenderer.color = finalColor;

        isInvincible = false;
    }
    IEnumerator DeathSequence()
    {
        anim.SetBool("isDead", true);

        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<PlayerCombat>().enabled = false;

        rb.linearVelocity = Vector2.zero;
        col.enabled = false;

        gameObject.tag = "Untagged";
        gameObject.layer = LayerMask.NameToLayer("Default");

        CameraShake.Instance.Shake(1.5f);
        Time.timeScale = 0.5f;

        yield return new WaitForSecondsRealtime(1.5f);

        DeathLight.Instance.TriggerPitchBlackSequence();
        UIManager.Instance.ShowGameOver();
    }
}