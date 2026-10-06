using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            IDamageable damageableTarget = collision.GetComponent<IDamageable>();

            if (damageableTarget != null)
            {
                damageableTarget.TakeDamage(1, transform.position);
            }
        }
    }
}