using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    [SerializeField] private Slider generalSoundSlider, ambientSoundSlider, musicSoundSlider;
    [SerializeField] private TextMeshProUGUI generalSoundLevel, ambientSoundLevel, musicSoundLevel;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioManager audioManager;
    private float hundred = 100f;

    private void Awake()
    {
        generalSoundSlider.value = PlayerPrefsBehaviour.GetGeneralSound();
        ambientSoundSlider.value = PlayerPrefsBehaviour.GetAmbientSound();
        musicSoundSlider.value = PlayerPrefsBehaviour.GetMusicSound();
        GeneralSoundSlider(PlayerPrefsBehaviour.GetGeneralSound());
        AmbientSoundSlider(PlayerPrefsBehaviour.GetAmbientSound());
        MusicSoundSlider(PlayerPrefsBehaviour.GetMusicSound());
    }

    public void GeneralSoundSlider(float volume)
    {
        AudioListener.volume = generalSoundSlider.value;
        generalSoundSlider.value = AudioListener.volume;
        generalSoundLevel.text = Mathf.FloorToInt(AudioListener.volume * hundred).ToString();
        PlayerPrefsBehaviour.SetGeneralSound(AudioListener.volume);
    }

    public void AmbientSoundSlider(float volume)
    {
        volume = ambientSoundSlider.value;
        ambientSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetAmbientSoundLevel(volume);
        PlayerPrefsBehaviour.SetAmbientSound(volume);
    }

    public void MusicSoundSlider(float volume)
    {
        volume = musicSoundSlider.value;
        musicSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetMusicSoundLevel(volume);
        PlayerPrefsBehaviour.SetMusicSound(volume);
    }

    public void OpenSettings()
    {
        settings.SetActive(true);
    }

    public void CloseSettings()
    {
        settings.SetActive(false);
    }
}