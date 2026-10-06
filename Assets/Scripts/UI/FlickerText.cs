using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class FlickerText : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    [SerializeField] private float flickerSpeed = 3f;
    [SerializeField] private float minAlpha = 0.2f;
    [SerializeField] private float maxAlpha = 1f;

    private void Awake()
    {
        textMesh = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        float alpha = Mathf.Lerp(minAlpha, maxAlpha, (Mathf.Sin(Time.unscaledTime * flickerSpeed) + 1f) / 2f);
        Color currentColor = textMesh.color;
        currentColor.a = alpha;
        textMesh.color = currentColor;
    }
}