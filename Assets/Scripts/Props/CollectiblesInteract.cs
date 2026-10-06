using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectiblesInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Material whiteSilhouetteMaterial;
    private float outlineThickness = 0.05f;

    private int amountToGive = 1;
    private float collectSpeed = 5f;

    private List<GameObject> outlineContainers = new List<GameObject>();
    private bool isCollected = false;

    private void Awake()
    {
        SpriteRenderer[] childSprites = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer sr in childSprites)
        {
            GameObject container = new GameObject(sr.gameObject.name + "_Outline");
            container.transform.SetParent(sr.transform);
            container.transform.localPosition = Vector3.zero;

            Vector3[] offsets = {
                new Vector3(0, outlineThickness, 0),
                new Vector3(0, -outlineThickness, 0),
                new Vector3(outlineThickness, 0, 0),
                new Vector3(-outlineThickness, 0, 0)
            };

            foreach (Vector3 offset in offsets)
            {
                GameObject shadow = new GameObject("Shadow");
                shadow.transform.SetParent(container.transform);
                shadow.transform.localPosition = offset;
                shadow.transform.localScale = Vector3.one;
                shadow.transform.localRotation = Quaternion.identity;

                SpriteRenderer shadowSR = shadow.AddComponent<SpriteRenderer>();
                shadowSR.sprite = sr.sprite;
                shadowSR.flipX = sr.flipX;
                shadowSR.flipY = sr.flipY;
                shadowSR.sortingLayerID = sr.sortingLayerID;
                shadowSR.sortingOrder = sr.sortingOrder - 1;
                shadowSR.material = whiteSilhouetteMaterial;
            }

            container.SetActive(false);
            outlineContainers.Add(container);
        }
    }

    public void Interact(Vector3 playerPosition)
    {
        if (isCollected) return;
        isCollected = true;
        AudioManager.Instance.PlaySFX("BeadPickup", "EVENT");
        
        PlayerAttributes playerAttributes = FindAnyObjectByType<PlayerAttributes>();
        playerAttributes.AddBead(amountToGive);

        HideOutline();
        StartCoroutine(CollectRoutine());
    }

    public void ShowOutline()
    {
        if (this == null || isCollected) return;
        foreach (var container in outlineContainers)
            if (container != null) container.SetActive(true);
    }

    public void HideOutline()
    {
        if (this == null) return;

        foreach (var container in outlineContainers)
            if (container != null) container.SetActive(false);
    }

    private IEnumerator CollectRoutine()
    {
        Vector3 startScale = transform.localScale;

        for (float t = 0; t < 1; t += Time.deltaTime * collectSpeed)
        {
            transform.localScale = Vector3.Lerp(startScale, Vector3.zero, t);
            yield return null;
        }

        Destroy(gameObject);
    }
}