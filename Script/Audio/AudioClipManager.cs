using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioClipManager : MonoBehaviour
{
    public AudioClip[] audioClips; // �ε��� 0: ���� �޴�, 1: ��ħ, 2: ����
    public AudioClip soundEffectClip;

    private AudioManager audioManager;
    private DayNightCycle dayNightCycle;
    private string currentSceneName;
    private AudioClip currentClip;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        // ���� �� �̸� ����
        currentSceneName = SceneManager.GetActiveScene().name;

        // ����� Ŭ�� ���
        RegisterClips(); 

        // ó�� ���� �� ������ Ŭ�� ���
        PlayAppropriateClip();

        if (soundEffectClip != null)
        {
            audioManager.RegisterClip(soundEffectClip.name, soundEffectClip);
        }
    }

    private void Update()
    {
        if (currentSceneName != SceneManager.GetActiveScene().name)
        {
            // ���� ����� ���
            currentSceneName = SceneManager.GetActiveScene().name;
            PlayAppropriateClip();
        }
        else if (dayNightCycle != null)
        {
            // ��ħ�� ������ �Ǻ�
            PlayClipBasedOnTime();
        }
        Click();
    }

    private void RegisterClips()
    {
        for (int i = 0; i < audioClips.Length; i++)
        {
            if (audioClips[i] != null)
            {
                audioManager.RegisterClip(audioClips[i].name, audioClips[i]);
            }
        }
    }

    private void PlayAppropriateClip()
    {
        if (currentSceneName == "StartScenes" && audioClips.Length > 0)
        {
            PlayClip(audioClips[0]); // ���� �޴� Ŭ�� ���
        }
        else
        {
            PlayClipBasedOnTime();
        }
    }

    private void PlayClipBasedOnTime()
    {
        if (dayNightCycle != null && audioClips.Length > 2)
        {
            float currentTime = dayNightCycle.time;

            // ��ħ: 0.25 ~ 0.75, ����: ������
            if (currentTime >= 0.25f && currentTime < 0.75f)
            {
                PlayClip(audioClips[1]); // ��ħ Ŭ�� ���
            }
            else
            {
                PlayClip(audioClips[2]); // ���� Ŭ�� ���
            }
        }
    }

    private void PlayClip(AudioClip clip)
    {
        if (clip != null && clip != currentClip)
        {
            currentClip = clip;
            audioManager.PlayClip(clip.name, true);
        }
    }

    private void Click()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (soundEffectClip != null)
            {
                AudioManager.Instance.PlayClip(soundEffectClip.name);
            }
        }
    }
}
