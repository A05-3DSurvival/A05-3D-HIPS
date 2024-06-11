using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeUI : MonoBehaviour
{
    public Slider masterSlider;
    public TextMeshProUGUI masterText;
    public Slider bgmSlider;
    public TextMeshProUGUI bgmText;
    public Slider sfxSlider;
    public TextMeshProUGUI sfxText;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.Instance;

        InitializeVolumeSlider(masterSlider, "Master", masterText);
        InitializeVolumeSlider(bgmSlider, "BGM", bgmText);
        InitializeVolumeSlider(sfxSlider, "SFX", sfxText);
    }

    private void InitializeVolumeSlider(Slider slider, string volumeType, TextMeshProUGUI volumeText)
    {
        if (slider != null)
        {
            slider.minValue = 0f;
            slider.maxValue = 1f;
            slider.value = audioManager.GetVolume(volumeType);
            UpdateVolumeText(volumeText, slider.value);

            slider.onValueChanged.AddListener(value =>
            {
                audioManager.SetVolume(volumeType, value);
                UpdateVolumeText(volumeText, value);
            });
        }
        else
        {
            Debug.LogWarning($"{volumeType} slider is not assigned.");
        }
    }

    private void UpdateVolumeText(TextMeshProUGUI volumeText, float volume)
    {
        if (volumeText != null)
        {
            volumeText.text = $"{Mathf.RoundToInt(volume * 100)}%";
        }
    }
}
