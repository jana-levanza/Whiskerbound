using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(Light2D))]
public class FireFlicker : MonoBehaviour
{
    private Light2D fireLight;

    private float minIntensity = 0.3f;
    private float maxIntensity = 1.2f;
    private float flickerSpeed = 2.5f;

    private float randomOffset;

    void Start()
    {
        fireLight = GetComponent<Light2D>();
        randomOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, randomOffset);
        fireLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }
}