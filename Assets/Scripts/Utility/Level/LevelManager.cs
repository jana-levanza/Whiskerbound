using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private string sceneToLoad;
    [SerializeField] private string destinationSpawnID;
    [SerializeField] private GameObject fadeInCanvas;

    [SerializeField] private PlayerProgressData progressData;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Actors"))
        {
            progressData.respawnScene = SceneManager.GetActiveScene().name;
            progressData.respawnDoorID = SpawnPoint.DestinationSpawnID;
            progressData.currentDoorID = destinationSpawnID;
                
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;

            Animator anim = collision.GetComponent<Animator>();
            anim.SetBool("isWalking", false);

            StartCoroutine(fadeIn());
        }
    }
    IEnumerator fadeIn()
    {
        Instantiate(fadeInCanvas);
        yield return new WaitForSecondsRealtime(1f); 

        SceneLoader.Load(sceneToLoad);
    }

}
