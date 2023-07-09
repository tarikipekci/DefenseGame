using UnityEngine;
using UnityEngine.UI;

public class SettingsBehaviour : MonoBehaviour
{
   [SerializeField] private Slider volumeSlider;
   [SerializeField] private Text volumeLevel;
   [SerializeField] private GameObject settings;

   public void VolumeSlider(float volume)
   {
      volumeLevel.text = volume.ToString("0.0");
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
