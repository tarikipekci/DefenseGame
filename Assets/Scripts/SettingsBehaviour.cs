using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    [SerializeField] private Slider generalSoundSlider, ambientSoundSlider, musicSoundSlider;
    [SerializeField] private Text generalSoundLevel, ambientSoundLevel, musicSoundLevel;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioManager audioManager;
    private float hundred = 100f;
    private float defaultValue = 0.5f;

    private void Awake()
    {
        generalSoundSlider.value = defaultValue;
        ambientSoundSlider.value = defaultValue;
        musicSoundSlider.value = defaultValue;
    }

    public void GeneralSoundSlider(float volume)
    {
        volume = generalSoundSlider.value;
        generalSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
    }

    public void AmbientSoundSlider(float volume)
    {
        volume = ambientSoundSlider.value;
        ambientSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetAmbientSoundLevel(volume);
        generalSoundSlider.value = (audioManager.GetAmbientSoundLevel() + audioManager.GetMusicSoundLevel()) / 2f;
    }

    public void MusicSoundSlider(float volume)
    {
        volume = musicSoundSlider.value;
        musicSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetMusicSoundLevel(volume);
        generalSoundSlider.value = (audioManager.GetAmbientSoundLevel() + audioManager.GetMusicSoundLevel()) / 2f;
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