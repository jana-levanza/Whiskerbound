using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class UIButtonFlash : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private Material whiteSilhouetteMaterial;
    [SerializeField] private float outlineThickness = 1.5f;

    [Header("What to do after 1 second?")]
    public UnityEvent onDelayedClick;

    private GameObject outlineContainer;
    private Image mainImage;
    private Button btn;
    private bool isClicked = false;

    private void Awake()
    {
        mainImage = GetComponent<Image>();
        btn = GetComponent<Button>();

        outlineContainer = new GameObject(gameObject.name + "_OutlineContainer");
        outlineContainer.transform.SetParent(transform.parent, false);
        outlineContainer.transform.SetSiblingIndex(transform.GetSiblingIndex());

        RectTransform containerRect = outlineContainer.AddComponent<RectTransform>();
        containerRect.anchorMin = mainImage.rectTransform.anchorMin;
        containerRect.anchorMax = mainImage.rectTransform.anchorMax;
        containerRect.pivot = mainImage.rectTransform.pivot;
        containerRect.sizeDelta = mainImage.rectTransform.sizeDelta;
        containerRect.anchoredPosition = mainImage.rectTransform.anchoredPosition;
        containerRect.localRotation = mainImage.rectTransform.localRotation;
        containerRect.localScale = mainImage.rectTransform.localScale;

        Canvas btnCanvas = GetComponent<Canvas>();
        if (btnCanvas != null && btnCanvas.overrideSorting)
        {
            Canvas outlineCanvas = outlineContainer.AddComponent<Canvas>();
            outlineCanvas.overrideSorting = true;
            outlineCanvas.sortingLayerID = btnCanvas.sortingLayerID;
            outlineCanvas.sortingOrder = btnCanvas.sortingOrder - 1;
        }

        Vector2[] offsets = {
            new Vector2(0, outlineThickness),
            new Vector2(0, -outlineThickness),
            new Vector2(outlineThickness, 0),
            new Vector2(-outlineThickness, 0)
        };

        foreach (Vector2 offset in offsets)
        {
            GameObject shadow = new GameObject("Shadow");
            shadow.transform.SetParent(outlineContainer.transform, false);

            RectTransform shadowRect = shadow.AddComponent<RectTransform>();
            shadowRect.sizeDelta = mainImage.rectTransform.sizeDelta;
            shadowRect.anchoredPosition = offset;

            Image shadowImage = shadow.AddComponent<Image>();
            shadowImage.sprite = mainImage.sprite;
            shadowImage.material = whiteSilhouetteMaterial;
            shadowImage.raycastTarget = false;
        }

        outlineContainer.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!isClicked && outlineContainer != null) { outlineContainer.SetActive(true); }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isClicked && outlineContainer != null) outlineContainer.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isClicked) return;
        StartCoroutine(ClickRoutine());
    }

    private IEnumerator ClickRoutine()
    {
        AudioManager.Instance.PlaySFX("Menu", "EVENT");
        isClicked = true;

        if (btn != null) btn.interactable = false;
        if (outlineContainer != null) outlineContainer.SetActive(true);
        yield return new WaitForSecondsRealtime(1f);
        onDelayedClick?.Invoke();
    }
}