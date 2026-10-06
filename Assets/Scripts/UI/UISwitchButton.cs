using UnityEngine;
using UnityEngine.UI;

public class UISwitchButton : MonoBehaviour
{
    [SerializeField] private Material whiteSilhouetteMaterial;
    [SerializeField] private float outlineThickness = 3f;

    public string characterName;

    private GameObject outlineContainer;
    private Button btn;

    private void Awake()
    {
        btn = GetComponent<Button>();
        Image mainImage = GetComponent<Image>();

        Canvas btnCanvas = GetComponent<Canvas>();
        if (btnCanvas == null)
        {
            btnCanvas = gameObject.AddComponent<Canvas>();
            gameObject.AddComponent<GraphicRaycaster>();
        }

        Canvas parentCanvas = transform.parent.GetComponentInParent<Canvas>();
        int baseOrder = parentCanvas != null ? parentCanvas.sortingOrder : 0;

        btnCanvas.overrideSorting = true;
        btnCanvas.sortingOrder = baseOrder + 2; 

        outlineContainer = new GameObject(gameObject.name + "_OutlineContainer", typeof(RectTransform));
        outlineContainer.transform.SetParent(transform, false);

        Canvas outlineCanvas = outlineContainer.AddComponent<Canvas>();
        outlineCanvas.overrideSorting = true;
        outlineCanvas.sortingOrder = baseOrder + 1;

        RectTransform containerRect = outlineContainer.GetComponent<RectTransform>();
        if (mainImage != null && containerRect != null)
        {
            containerRect.anchorMin = Vector2.zero;
            containerRect.anchorMax = Vector2.one;
            containerRect.sizeDelta = Vector2.zero;
            containerRect.anchoredPosition = Vector2.zero;
        }

        Vector2[] offsets = {
            new Vector2(0, outlineThickness),
            new Vector2(0, -outlineThickness),
            new Vector2(outlineThickness, 0),
            new Vector2(-outlineThickness, 0)
        };

        foreach (Vector2 offset in offsets)
        {
            GameObject shadow = new GameObject("Shadow", typeof(RectTransform));
            shadow.transform.SetParent(outlineContainer.transform, false);

            RectTransform shadowRect = shadow.GetComponent<RectTransform>();
            if (mainImage != null)
            {
                shadowRect.anchorMin = Vector2.zero;
                shadowRect.anchorMax = Vector2.one;
                shadowRect.sizeDelta = Vector2.zero;
                shadowRect.anchoredPosition = offset;

                Image shadowImage = shadow.AddComponent<Image>();
                shadowImage.sprite = mainImage.sprite;
                shadowImage.material = whiteSilhouetteMaterial;
                shadowImage.raycastTarget = false;
            }
        }

        outlineContainer.SetActive(false);
    }

    public void SetActiveState(bool isActive)
    {
        if (outlineContainer != null)
            outlineContainer.SetActive(isActive);

        if (btn != null)
            btn.interactable = !isActive;
    }
}