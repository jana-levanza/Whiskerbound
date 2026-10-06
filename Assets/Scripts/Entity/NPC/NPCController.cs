using UnityEngine;

public class NPCController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void Interact(Vector3 playerPosition)
    {
        Vector2 direction = (playerPosition - transform.position).normalized;

        anim.SetFloat("moveX", direction.x);
        anim.SetFloat("moveY", direction.y);

        if (direction.x > 0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
        else if (direction.x < -0.1f)
            transform.localScale = new Vector3(1, 1, 1);
    }
}