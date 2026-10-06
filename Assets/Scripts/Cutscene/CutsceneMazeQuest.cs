using UnityEngine;
using System.Collections;
using UnityEngine.Tilemaps;
using UnityEngine.Rendering.Universal;

public class CutsceneMazeQuest : MonoBehaviour
{
    [SerializeField] private GameObject doorCamera;
    [SerializeField] private GameObject secretDoor;
    [SerializeField] private GameObject doorLight;

    private PlayerMovement playerMovement;

    public void PlaySequence()
    {
        playerMovement = FindAnyObjectByType<PlayerMovement>();
        if (playerMovement != null)
            playerMovement.enabled = false;
        UIManager.Instance.ShowInspect("May lagusang nagbukas...");
        StartCoroutine(SequenceRoutine());
    }

    private IEnumerator SequenceRoutine()
    {
        float panTime = 2.0f;
        float lightFadeTime = 1.5f;
        float breakTime = 2.5f;

        doorCamera.SetActive(true);
        yield return new WaitForSeconds(2f);

        Light2D light2D = null;
        float targetIntensity = 0f;

        doorLight.SetActive(true);
        light2D = doorLight.GetComponent<Light2D>();

        targetIntensity = light2D.intensity;
        light2D.intensity = 0f;

        float lightTimer = 0;
        while (lightTimer < lightFadeTime)
        {
            lightTimer += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(0f, targetIntensity, lightTimer / lightFadeTime);
            yield return null;
        }
        light2D.intensity = targetIntensity;

        yield return new WaitForSeconds(0.5f);

        CameraShake.Instance.Shake(5f);

        SpriteRenderer sr = secretDoor.GetComponent<SpriteRenderer>();
        Tilemap tm = secretDoor.GetComponent<Tilemap>();

        float t = 0;
        while (t < breakTime)
        {
            t += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, t / breakTime);

            if (sr != null) { Color c = sr.color; c.a = alpha; sr.color = c; }
            if (tm != null) { Color c = tm.color; c.a = alpha; tm.color = c; }

            yield return null;
        }

        Destroy(secretDoor);

        yield return new WaitForSeconds(1.5f);

        float lightTimerOut = 0;
        while (lightTimerOut < lightFadeTime)
        {
            lightTimerOut += Time.deltaTime;
            light2D.intensity = Mathf.Lerp(targetIntensity, 0f, lightTimerOut / lightFadeTime);
            yield return null;
        }
        light2D.intensity = 0f;

        doorCamera.SetActive(false);

        yield return new WaitForSeconds(panTime);

        playerMovement.enabled = true;

        Destroy(doorLight);
        Destroy(doorCamera);
        Destroy(gameObject);
    }
}