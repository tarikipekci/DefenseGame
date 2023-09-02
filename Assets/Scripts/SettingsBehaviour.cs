using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
    [SerializeField] private Slider generalSoundSlider, ambientSoundSlider, musicSoundSlider;
    [SerializeField] private Text generalSoundLevel, ambientSoundLevel, musicSoundLevel;
    [SerializeField] private GameObject settings;
    [SerializeField] private AudioManager audioManager;
    private float hundred = 100f;
    
    private void Awake()
    {
        generalSoundSlider.value = AudioListener.volume;
        musicSoundSlider.value = audioManager.GetMusicSoundLevel();
        ambientSoundSlider.value = audioManager.GetAmbientSoundLevel();
    }

    public void GeneralSoundSlider(float volume)
    {
        AudioListener.volume = generalSoundSlider.value;
        generalSoundSlider.value = AudioListener.volume;
        generalSoundLevel.text = Mathf.FloorToInt(AudioListener.volume * hundred).ToString();
    }

    public void AmbientSoundSlider(float volume)
    {
        volume = ambientSoundSlider.value;
        ambientSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetAmbientSoundLevel(volume);
    }

    public void MusicSoundSlider(float volume)
    {
        volume = musicSoundSlider.value;
        musicSoundLevel.text = Mathf.FloorToInt(volume * hundred).ToString();
        audioManager.SetMusicSoundLevel(volume);
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