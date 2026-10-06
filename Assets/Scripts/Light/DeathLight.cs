using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using System.Collections.Generic;

public class DeathLight : MonoBehaviour
{
    public static DeathLight Instance { get; private set; }

    [Header("Lights Setup")]
    [SerializeField] private Light2D globalLight;
    [SerializeField] private Light2D deathPointLight;
    [SerializeField] private GameObject auraLightObj;

    [Header("Transition Settings")]
    [SerializeField] private float fadeToBlackDuration = 1.5f; // Gano katagal bago maging total pitch black
    [SerializeField] private float shrinkLightDuration = 1.0f; // Gano katagal paliitin yung ilaw bago mag-loading

    private List<Light2D> otherLights = new List<Light2D>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        if (deathPointLight != null) deathPointLight.enabled = false;
    }

    public void TriggerPitchBlackSequence()
    {
        StartCoroutine(FadeToBlackRoutine());
    }

    private IEnumerator FadeToBlackRoutine()
    {
        // 1. Patayin agad ang particles at aura light
        if (auraLightObj != null) auraLightObj.SetActive(false);

        ParticleSystem[] allParticles = FindObjectsByType<ParticleSystem>(FindObjectsSortMode.None);
        foreach (ParticleSystem ps in allParticles)
        {
            ps.gameObject.SetActive(false);
        }

        // 2. I-setup ang Death Light
        if (deathPointLight != null)
        {
            deathPointLight.enabled = true;
            deathPointLight.intensity = 0.3f;
            deathPointLight.pointLightInnerRadius = 1f;
            deathPointLight.pointLightOuterRadius = 1.1f;
            // Kung sakaling naka Spot light type ka na 360, ise-set din natin ito
            deathPointLight.pointLightInnerAngle = 360f;
            deathPointLight.pointLightOuterAngle = 360f;
        }

        // 3. Hanapin lahat ng iba pang ilaw para i-fade
        Light2D[] allLights = FindObjectsByType<Light2D>(FindObjectsSortMode.None);
        List<float> otherLightsStartIntensity = new List<float>();

        foreach (Light2D light in allLights)
        {
            if (light != deathPointLight && light != globalLight)
            {
                otherLights.Add(light);
                otherLightsStartIntensity.Add(light.intensity);
            }
        }

        float elapsedTime = 0f;
        float startGlobalIntensity = globalLight != null ? globalLight.intensity : 0f;

        // 4. Dahan-dahang mag-fade to black
        while (elapsedTime < fadeToBlackDuration)
        {
            elapsedTime += Time.unscaledDeltaTime; // Unscaled para kahit naka-slowmo gagana
            float t = elapsedTime / fadeToBlackDuration;

            if (globalLight != null)
                globalLight.intensity = Mathf.Lerp(startGlobalIntensity, 0f, t);

            for (int i = 0; i < otherLights.Count; i++)
            {
                if (otherLights[i] != null)
                    otherLights[i].intensity = Mathf.Lerp(otherLightsStartIntensity[i], 0f, t);
            }

            yield return null;
        }

        // 5. Siguraduhin na 0 na talaga sila sa dulo
        if (globalLight != null) globalLight.intensity = 0f;
        foreach (var l in otherLights) if (l != null) { l.intensity = 0f; l.enabled = false; }
    }

    // TATAWAGIN ITO PAGKA-TAP NG SCREEN PARA LUMIIT YUNG ILAW
    public IEnumerator ShrinkDeathLightRoutine()
    {
        if (deathPointLight == null) yield break;

        float elapsedTime = 0f;
        float startInnerRad = deathPointLight.pointLightInnerRadius;
        float startOuterRad = deathPointLight.pointLightOuterRadius;

        float startInnerAng = deathPointLight.pointLightInnerAngle;
        float startOuterAng = deathPointLight.pointLightOuterAngle;

        while (elapsedTime < shrinkLightDuration)
        {
            elapsedTime += Time.unscaledDeltaTime;
            float t = elapsedTime / shrinkLightDuration;
            float smoothStep = Mathf.SmoothStep(0f, 1f, t);

            // Paliitin ang Radius (Para sa Point Light)
            deathPointLight.pointLightInnerRadius = Mathf.Lerp(startInnerRad, 0f, smoothStep);
            deathPointLight.pointLightOuterRadius = Mathf.Lerp(startOuterRad, 0f, smoothStep);

            // Paliitin ang Angle (Para sa Spot Light na naka 360)
            deathPointLight.pointLightInnerAngle = Mathf.Lerp(startInnerAng, 0f, smoothStep);
            deathPointLight.pointLightOuterAngle = Mathf.Lerp(startOuterAng, 0f, smoothStep);

            yield return null;
        }

        deathPointLight.intensity = 0f;
    }
}