using System.Collections;
using UnityEngine;

public class EntityKnockback : MonoBehaviour
{
    private float knockbackForce = 5f;
    private float knockbackTime = 0.2f;
    private float stunTime = 0.5f;

    private Rigidbody2D rb;
    private PlayerMovement playerMove;
    private EnemyMovement enemyMove;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerMove = GetComponent<PlayerMovement>();
        enemyMove = GetComponent<EnemyMovement>();
    }

    public void Apply(Vector2 attackerPosition, float multiplier)
    {
        StopAllCoroutines();
        StartCoroutine(KnockbackRoutine(attackerPosition, multiplier));
    }

    private IEnumerator KnockbackRoutine(Vector2 attackerPosition, float multiplier)
    {
        if (playerMove != null) playerMove.isStunned = true;
        if (enemyMove != null) enemyMove.isStunned = true;

        Vector2 direction = ((Vector2)transform.position - attackerPosition).normalized;
        rb.linearVelocity = direction * knockbackForce * multiplier;

        yield return new WaitForSeconds(knockbackTime);

        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(stunTime);

        if (playerMove != null) playerMove.isStunned = false;
        if (enemyMove != null) enemyMove.isStunned = false;
    }
}
