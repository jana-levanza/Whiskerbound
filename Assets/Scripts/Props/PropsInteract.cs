using System.Collections.Generic;
using UnityEngine;

public class PropsInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Material whiteSilhouetteMaterial;
    [SerializeField] private string textMessage;
    private float outlineThickness = 0.05f;

    private List<GameObject> outlineContainers = new List<GameObject>();

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
        if (textMessage != "")
            UIManager.Instance.ShowInspect(textMessage);
        else UIManager.Instance.ShowInspect("There's nothing interesting here.");
    }

    public void ShowOutline()
    {
        foreach (GameObject container in outlineContainers)
            container.SetActive(true);
    }

    public void HideOutline()
    {
        foreach (GameObject container in outlineContainers)
            container.SetActive(false);
    }
}