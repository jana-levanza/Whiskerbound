using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    private EnemyAttributes attributes;
    private SpriteRenderer spriteRenderer;
    private Animator anim;
    private Collider2D coll;
    private EnemyMovement movement;


    [SerializeField] private Material flashMaterial;
    private Material originalMaterial;

    [SerializeField] private ParticleSystem hitParticlePrefab;
    [SerializeField] private ParticleSystem deathParticlePrefab;

    private bool isDead = false;

    private void Awake()
    {
        attributes = GetComponent<EnemyAttributes>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        coll = GetComponent<Collider2D>();
        movement = GetComponent<EnemyMovement>();

        originalMaterial = spriteRenderer.material;
    }

    public void TakeDamage(int amount, Vector2 attackerPosition)
    {
        if (isDead) return;

        attributes.HP -= amount;

        spriteRenderer.material = flashMaterial;
        Invoke(nameof(ResetMaterial), 0.2f);

        Vector2 knockbackDir = ((Vector2)transform.position - attackerPosition).normalized;
        float angle = Mathf.Atan2(knockbackDir.y, knockbackDir.x) * Mathf.Rad2Deg;
        Vector3 spawnPosition = transform.position + (Vector3)(knockbackDir * 0.5f);
        spawnPosition.z = 0f;


        if (attributes.HP <= 0)
        {
            GetComponent<EntityKnockback>().Apply(attackerPosition, 2.0f);
            AudioManager.Instance.PlaySFX("EnemyDeath", "COMBAT");
            CameraShake.Instance.Shake(1.3f);
            HitStopManager.Instance.TriggerHitStop(0.3f);
            isDead = true;
            StartCoroutine(DeathSequence());
        }
        else
        {
            GetComponent<EntityKnockback>().Apply(attackerPosition, 1.0f);
            AudioManager.Instance.PlaySFX("EnemyHit", "COMBAT");
            CameraShake.Instance.Shake(0.8f);
            HitStopManager.Instance.TriggerHitStop(0.12f);
            Instantiate(hitParticlePrefab, spawnPosition, Quaternion.Euler(0, 0, angle));
        }
    }

    private void ResetMaterial()
    {
        if (spriteRenderer != null && originalMaterial != null)
            spriteRenderer.material = originalMaterial;
    }

    IEnumerator DeathSequence()
    {
        anim.SetBool("isDead", true);

        movement.enabled = false;
        coll.enabled = false;

        Transform hitBox = transform.Find("DamageHitBox");
        hitBox.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.2f);
        
        Instantiate(deathParticlePrefab, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.6f);

        Destroy(gameObject);
    }
}