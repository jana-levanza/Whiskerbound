using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    //private int beadsRequired = 12;

    private void Update()
    {
        //if (PlayerAttributes.instance.BeadsEarned >= beadsRequired && enemyPrefab != null)
        //{
        //    Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        //    Destroy(gameObject);
        //}
    }
}