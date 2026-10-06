using UnityEngine;
using Unity.Cinemachine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private string mySpawnID;
    [SerializeField] private GameObject playerPrefab;
    [SerializeField] private PlayerProgressData progressData;

    public static string DestinationSpawnID = "";
    private void Start()
    {
        DestinationSpawnID = mySpawnID;

        if (progressData.currentDoorID == mySpawnID || progressData.respawnDoorID == mySpawnID)
            SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        GameObject newPlayer = Instantiate(playerPrefab, transform.position, Quaternion.identity);

        if (newPlayer.TryGetComponent(out CharacterSwitch charSwitch))
            charSwitch.RefreshUI();
    }
}