using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AltarInteract : MonoBehaviour, IInteractable
{
    [SerializeField] private Material whiteSilhouetteMaterial;
    private float outlineThickness = 0.05f;
    private List<GameObject> outlineContainers = new List<GameObject>();

    [SerializeField] private ParticleSystem gatheringParticles; 
    [SerializeField] private ParticleSystem burstParticles;

    [SerializeField] private CharacterData shuyiData;
    [SerializeField] private CharacterData yoichiData;
    [SerializeField] private PlayerProgressData progressData;

    private float chargeTime = 3f;
    private float shakeIntensity = 3f; 

    private bool hasInteracted = false;

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
        if (!hasInteracted)
        {
            hasInteracted = true;
            UIManager.Instance.ShowInspect("You have offered 12 beads.");
            StartCoroutine(AltarSequence());
        }
    }

    private IEnumerator AltarSequence()
    {
        gatheringParticles.Play();
        AudioManager.Instance.PlaySFX("Event", "EVENT");

        yield return new WaitForSeconds(chargeTime);

        gatheringParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        CameraShake.Instance.Shake(shakeIntensity);
        burstParticles.Play();
        AudioManager.Instance.PlaySFX("TaskComplete", "EVENT");

        shuyiData.maxHP += 1;
        yoichiData.maxHP += 1;

        if (progressData != null)
        {
            progressData.currentShuyiHP = shuyiData.maxHP;
            progressData.currentYoichiHP = yoichiData.maxHP;
        }

        yield return new WaitForSeconds(2f);
        UIManager.Instance.ShowInspect("The Altar has been awakened!\nMaximum HP increased!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            CharacterSwitch charSwitch = player.GetComponent<CharacterSwitch>();
            if (charSwitch != null)
                charSwitch.RefreshUI();
        }
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