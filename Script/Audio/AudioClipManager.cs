using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioClipManager : MonoBehaviour
{
    public AudioClip[] audioClips; // 인덱스 0: 메인 메뉴, 1: 아침, 2: 저녁
    public AudioClip soundEffectClip;

    private AudioManager audioManager;
    private DayNightCycle dayNightCycle;
    private string currentSceneName;
    private AudioClip currentClip;

    private void Start()
    {
        audioManager = AudioManager.Instance;
        dayNightCycle = FindObjectOfType<DayNightCycle>();

        // 현재 씬 이름 저장
        currentSceneName = SceneManager.GetActiveScene().name;

        // 오디오 클립 등록
        RegisterClips(); 

        // 처음 시작 시 적절한 클립 재생
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
            // 씬이 변경된 경우
            currentSceneName = SceneManager.GetActiveScene().name;
            PlayAppropriateClip();
        }
        else if (dayNightCycle != null)
        {
            // 아침과 저녁을 판별
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
            PlayClip(audioClips[0]); // 메인 메뉴 클립 재생
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

            // 아침: 0.25 ~ 0.75, 저녁: 나머지
            if (currentTime >= 0.25f && currentTime < 0.75f)
            {
                PlayClip(audioClips[1]); // 아침 클립 재생
            }
            else
            {
                PlayClip(audioClips[2]); // 저녁 클립 재생
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
