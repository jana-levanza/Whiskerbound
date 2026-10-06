using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public SoundLibrary library;

    private AudioSource bgmSource;
    private AudioSource ambianceSource;
    private AudioSource typingSource;

    [Range(0f, 1f)] public float bgmVolume = 0.5f;
    [Range(0f, 1f)] public float ambianceVolume = 0.3f;
    [Range(0f, 1f)] public float sfxVolume = 1f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.volume = bgmVolume;

            ambianceSource = gameObject.AddComponent<AudioSource>();
            ambianceSource.loop = true;
            ambianceSource.volume = ambianceVolume;

            typingSource = gameObject.AddComponent<AudioSource>();
            typingSource.loop = true;
            typingSource.playOnAwake = false;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayBGM(string bgmName)
    {
        AudioClip clipToPlay = library.GetRandomClip(bgmName, library.bgmGroups);
        if (clipToPlay != null && bgmSource.clip != clipToPlay)
        {
            bgmSource.clip = clipToPlay;
            bgmSource.Play();
        }
    }

    public void PlayAmbiance(string ambianceName)
    {
        AudioClip clipToPlay = library.GetRandomClip(ambianceName, library.ambianceGroups);
        if (clipToPlay != null && ambianceSource.clip != clipToPlay)
        {
            ambianceSource.clip = clipToPlay;
            ambianceSource.Play();
        }
    }

    public void PlaySFX(string sfxName, string category)
    {
        AudioClip clipToPlay = null;

        switch (category.ToUpper())
        {
            case "COMBAT": clipToPlay = library.GetRandomClip(sfxName, library.sfxCombat); break;
            case "EVENT": clipToPlay = library.GetRandomClip(sfxName, library.sfxEvent); break;
            case "PLAYER": clipToPlay = library.GetRandomClip(sfxName, library.sfxPlayer); break;
            case "UI": clipToPlay = library.GetRandomClip(sfxName, library.sfxUI); break;
        }

        if (clipToPlay != null)
        {
            GameObject sfxObject = new GameObject("SFX_" + sfxName);
            AudioSource source = sfxObject.AddComponent<AudioSource>();

            source.clip = clipToPlay;
            source.volume = sfxVolume;
            if (category.ToUpper() != "UI")
                source.pitch = Random.Range(0.9f, 1.1f);
            source.Play();

            Destroy(sfxObject, clipToPlay.length / source.pitch);
        }
    }

    public void PlayTypingSFX(string sfxName)
    {
        AudioClip clipToPlay = library.GetRandomClip(sfxName, library.sfxUI);
        if (clipToPlay != null)
        {
            typingSource.clip = clipToPlay;
            typingSource.volume = sfxVolume;
            if (!typingSource.isPlaying)
                typingSource.Play();
        }
    }
    public void StopTypingSFX()
    {
        if (typingSource != null && typingSource.isPlaying)
            typingSource.Stop();
    }
}