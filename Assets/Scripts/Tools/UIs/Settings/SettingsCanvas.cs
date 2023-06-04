using UnityEngine;
using UnityEngine.UI;

public class SettingsCanvas : MonoBehaviour
{
    [SerializeField] private Slider masterVolSlider;
    [SerializeField] private Slider musicVolSlider;
    [SerializeField] private Slider fxVolSlider;

    private void Start()
    {
        // Initialize sliders with current volume settings
        masterVolSlider.value = GameSettings.Instance.MasterVol;
        musicVolSlider.value = GameSettings.Instance.MusicVol;
        fxVolSlider.value = GameSettings.Instance.FxVol;
    }

    public void OnMasterVolumeChanged()
    {
        // Update the master volume setting in GameSettings
        GameSettings.Instance.UpdateVol(masterVolSlider.value, GameSettings.Instance.MusicVol, GameSettings.Instance.FxVol);
    }

    public void OnMusicVolumeChanged()
    {
        // Update the music volume setting in GameSettings
        GameSettings.Instance.UpdateVol(GameSettings.Instance.MasterVol, musicVolSlider.value, GameSettings.Instance.FxVol);
    }

    public void OnFxVolumeChanged()
    {
        // Update the sound effects volume setting in GameSettings
        GameSettings.Instance.UpdateVol(GameSettings.Instance.MasterVol, GameSettings.Instance.MusicVol, fxVolSlider.value);
    }
}
