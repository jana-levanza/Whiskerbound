using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float speed = 5f;
    private int damage = 1;
    private float lifetime = 0.5f;

    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        Destroy(gameObject, lifetime);
    }

    public void Setup(Vector2 direction)
    {
        rb.linearVelocity = direction.normalized * speed;

        anim.Play("ProjectileFly");
        transform.localScale = new Vector3(1.5f, 1.5f, 1f);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle += 90f;

        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            IDamageable damageable = collision.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(damage, transform.position);
            }
            Destroy(gameObject);
        }
    }
}