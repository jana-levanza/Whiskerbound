using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private float speed = 2f;

    private Transform player;

    private Rigidbody2D rb;
    private Animator anim;

    private bool isChasing;
    public bool isStunned = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (isStunned) return;

        if (isChasing == true)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.linearVelocity = direction * speed;

            if (direction.x > 0)
                transform.localScale = new Vector3(1, 1, 1);
            else if (direction.x < 0)
                transform.localScale = new Vector3(-1, 1, 1);

            anim.SetBool("isWalking", true);
        }
    }

    public void StartAggro(Transform target)
    {
        player = target;
        isChasing = true;
    }

    public void StopAggro()
    {
        player = null;
        isChasing = false;
        rb.linearVelocity = Vector2.zero;
        anim.SetBool("isWalking", false);
    }
}