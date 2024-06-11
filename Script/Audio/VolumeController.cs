using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Slider masterSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private void Start()
    {
        AudioManager audioManager = AudioManager.Instance;

        InitializeSlider(masterSlider, "Master");
        InitializeSlider(bgmSlider, "BGM");
        InitializeSlider(sfxSlider, "SFX");
    }

    private void InitializeSlider(Slider slider, string volumeType)
    {
        if (slider != null)
        {
            slider.value = AudioManager.Instance.GetVolume(volumeType);
            slider.onValueChanged.AddListener(volume => AudioManager.Instance.SetVolume(volumeType, volume));
        }
        else
        {
            Debug.LogWarning($"{volumeType} slider is not assigned.");
        }
    }
}
