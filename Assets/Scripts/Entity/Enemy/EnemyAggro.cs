using UnityEngine;

public class EnemyAggro : MonoBehaviour
{
    private EnemyMovement enemy;

    private void Start()
    {
        enemy = GetComponentInParent<EnemyMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            enemy.StartAggro(collision.transform);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            enemy.StopAggro();
    }
}