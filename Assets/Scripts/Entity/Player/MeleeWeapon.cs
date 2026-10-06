using UnityEngine;

public class MeleeWeapon : MonoBehaviour, IWeapon
{
    private int attackDamage = 1;
    private float attackRadius = 0.6f;
    private float attackOffset = 0.5f;
    private Vector2 bodyCenter = new Vector2(0f, 0.5f);
    [SerializeField] private LayerMask enemyLayer;

    public void PerformAttack(Vector2 facingDirection)
    {
        float currentBodyCenterX = bodyCenter.x;

        if (facingDirection.y < -0.5f)
            currentBodyCenterX = -0.3f;
        else if (facingDirection.y > 0.5f)
            currentBodyCenterX = 0.3f;

        float targetX = currentBodyCenterX + (facingDirection.x * attackOffset);
        float targetY = bodyCenter.y + (facingDirection.y * attackOffset);
        transform.localPosition = new Vector3(targetX, targetY, 0);

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(transform.position, attackRadius, enemyLayer);

        bool hasHit = false;

        foreach (Collider2D enemy in hitEnemies)
        {
            IDamageable damageableEntity = enemy.GetComponent<IDamageable>();
            if (damageableEntity != null)
            {
                damageableEntity.TakeDamage(attackDamage, transform.parent.position);
                hasHit = true;
            }
        }

        if (hasHit)
        {
            PlayerMovement pm = transform.parent.GetComponent<PlayerMovement>();
            pm.ApplyBounce(-facingDirection);
        }
    }
}