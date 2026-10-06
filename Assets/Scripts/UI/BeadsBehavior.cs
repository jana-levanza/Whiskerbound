using TMPro;
using UnityEngine;

public class BeadsBehavior : MonoBehaviour
{
    private PlayerAttributes player;
    [SerializeField] private TextMeshProUGUI beadText;

    private void Start()
    {
        FindPlayer();
        UpdateText();
    }

    private void Update()
    {
        UpdateText();
    }

    private void FindPlayer()
    {
        if (player == null)
        {
            player = Object.FindAnyObjectByType<PlayerAttributes>();
        }
    }

    private void UpdateText()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (player != null && beadText != null)
        {
            beadText.text = player.BeadsEarned.ToString();
        }
    }
}