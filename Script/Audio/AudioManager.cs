using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    private Dictionary<string, AudioClip> audioClips = new Dictionary<string, AudioClip>();
    private AudioSource bgmSource;
    private List<AudioSource> sfxSources = new List<AudioSource>();

    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    public float sfxVolume = 1f;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject audioManagerGO = new GameObject("AudioManager");
                instance = audioManagerGO.AddComponent<AudioManager>();
                DontDestroyOnLoad(audioManagerGO);
            }
            return instance;
        }
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneUnloaded += OnSceneUnloaded;

        // 볼륨 값 불러오기
        masterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        bgmVolume = PlayerPrefs.GetFloat("BGMVolume", 1f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);

        bgmSource = gameObject.AddComponent<AudioSource>();
        UpdateAllVolumes();
    }

    private void OnDestroy()
    {
        SceneManager.sceneUnloaded -= OnSceneUnloaded;

        // 볼륨 값 저장하기
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    private void OnSceneUnloaded(Scene scene)
    {
        // 볼륨 값 저장하기
        PlayerPrefs.SetFloat("MasterVolume", masterVolume);
        PlayerPrefs.SetFloat("BGMVolume", bgmVolume);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
        PlayerPrefs.Save();
    }

    public void RegisterClip(string clipName, AudioClip clip)
    {
        if (!audioClips.ContainsKey(clipName))
        {
            audioClips.Add(clipName, clip);
        }
        else
        {
            Debug.LogWarning($"Audio clip already registered: {clipName}");
        }
    }

    public void PlayClip(string clipName, bool isBGM = false)
    {
        if (audioClips.TryGetValue(clipName, out AudioClip clip))
        {
            if (isBGM)
            {
                PlayBGM(clip);
            }
            else
            {
                PlaySFX(clip);
            }
        }
        else
        {
            Debug.LogWarning($"Audio clip not found: {clipName}");
        }
    }

    private void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.loop = true;
        bgmSource.volume = bgmVolume * masterVolume;
        bgmSource.Play();
    }

    private void PlaySFX(AudioClip clip)
    {
        AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.clip = clip;
        sfxSource.volume = sfxVolume * masterVolume;
        sfxSource.Play();
        sfxSources.Add(sfxSource);
    }

    private void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Stop();
        }
    }

    private void StopAllSFX()
    {
        foreach (var sfxSource in sfxSources)
        {
            if (sfxSource.isPlaying)
            {
                sfxSource.Stop();
            }
            Destroy(sfxSource);
        }
        sfxSources.Clear();
    }

    public void SetVolume(string type, float volume)
    {
        volume = Mathf.Clamp01(volume);
        switch (type)
        {
            case "Master":
                masterVolume = volume;
                UpdateAllVolumes();
                break;
            case "BGM":
                bgmVolume = volume;
                if (bgmSource != null)
                    bgmSource.volume = bgmVolume * masterVolume;
                break;
            case "SFX":
                sfxVolume = volume;
                UpdateSFXVolumes();
                break;
            default:
                Debug.LogWarning($"Unknown volume type: {type}");
                break;
        }
    }

    private void UpdateAllVolumes()
    {
        if (bgmSource != null)
            bgmSource.volume = bgmVolume * masterVolume;
        UpdateSFXVolumes();
    }

    public void UpdateSFXVolumes()
    {
        foreach (var sfxSource in sfxSources)
        {
            if (sfxSource != null)
            {
                sfxSource.volume = sfxVolume * masterVolume;
            }
        }
    }

    public float GetVolume(string type)
    {
        return type switch
        {
            "Master" => masterVolume,
            "BGM" => bgmVolume,
            "SFX" => sfxVolume,
            _ => 0f,
        };
    }
}
