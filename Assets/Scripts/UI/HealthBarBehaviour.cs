using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarBehaviour : MonoBehaviour
{
    private PlayerAttributes player;
    [SerializeField] private Image healthIconImage;
    [SerializeField] private Transform heartRoot;
    [SerializeField] private GameObject heartPrefab;

    private readonly List<HeartUI> hearts = new();
    private int lastHP;

    private void Start()
    {
        FindPlayer();
        if (player != null)
        {
            RebuildHearts();
            SyncInstant();
        }
    }

    private void FindPlayer()
    {
        player = Object.FindAnyObjectByType<PlayerAttributes>();
    }

    private void Update()
    {
        FindPlayer();
        if (player == null) return;
        if (player.HP == lastHP) return;

        if (hearts.Count != player.MaxHP)
        {
            RebuildHearts();
            SyncInstant();
        }

        if (player.HP < lastHP)
            for (int i = lastHP - 1; i >= player.HP; i--)
            {
                if (player.HP <= 0)
                    hearts[i].SetEmptyInstant();
                else
                    hearts[i].Break();
            }
        else
            for (int i = lastHP; i < player.HP; i++)
                hearts[i].Restore();

        lastHP = player.HP;
    }

    public void ApplyUI(Sprite iconSprite, GameObject newHeartPrefab)
    {
        healthIconImage.sprite = iconSprite;
        heartPrefab = newHeartPrefab;

        if (player == null)
            FindPlayer();

        if (player != null)
        {
            RebuildHearts();
            SyncInstant();
            lastHP = player.HP;
        }
    }

    private void RebuildHearts()
    {
        hearts.Clear();

        for (int i = heartRoot.childCount - 1; i >= 0; i--)
            Destroy(heartRoot.GetChild(i).gameObject);

        for (int i = 0; i < player.MaxHP; i++)
        {
            GameObject h = Instantiate(heartPrefab, heartRoot);
            hearts.Add(h.GetComponent<HeartUI>());
        }
    }
    private void SyncInstant()
    {
        for (int i = 0; i < hearts.Count; i++)
        {
            if (i < player.HP) hearts[i].SetFullInstant();
            else hearts[i].SetEmptyInstant();
        }

        lastHP = player.HP;
    }
}