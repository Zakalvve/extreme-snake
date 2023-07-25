using ExtremeSnake.Game;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    public GameObject MusicToggleOn;
    public GameObject MusicToggleOff;
    public GameObject SoundFXToggleOn;
    public GameObject SoundFXToggleOff;
    private Slider _volumeSlider;
    public bool ShouldDebug;

    private void Awake() {
        _volumeSlider = GetComponentInChildren<Slider>();
        if (ShouldDebug) {
            GameManager.Instance.GameEmitter.Subscribe("OnVolumeChanged",DebugSettingsValues);
            GameManager.Instance.GameEmitter.Subscribe("OnPlayMusicChanged",DebugSettingsValues);
            GameManager.Instance.GameEmitter.Subscribe("OnPlaySoundFXChanged",DebugSettingsValues);
        }
    }

    public void OnActive() {
        _volumeSlider.value = GameManager.Instance.Settings.AudioSettings.Volume;
        
        bool musicToggle = GameManager.Instance.Settings.AudioSettings.PlayMusic;
        bool sfxToggle = GameManager.Instance.Settings.AudioSettings.PlaySoundFX;

        MusicToggleOn.SetActive(musicToggle);
        MusicToggleOff.SetActive(!musicToggle);

        SoundFXToggleOn.SetActive(sfxToggle);
        SoundFXToggleOff.SetActive(!sfxToggle);
    }

    public void OnVolumeSliderChanged() {
        GameManager.Instance.Settings.AudioSettings.Volume = (int)_volumeSlider.value;
    }

    public void TogglePlaySFX() {
        GameManager.Instance.Settings.AudioSettings.PlaySoundFX = !GameManager.Instance.Settings.AudioSettings.PlaySoundFX;
    }

    public void TogglePlayMusic() {
        GameManager.Instance.Settings.AudioSettings.PlayMusic = !GameManager.Instance.Settings.AudioSettings.PlayMusic;
    }

    public void DebugSettingsValues(object sender) {
        Debug.Log($"Volume Level: {GameManager.Instance.Settings.AudioSettings.Volume}");
        Debug.Log($"Play Music: {(GameManager.Instance.Settings.AudioSettings.PlayMusic ? "Yes" : "No")}");
        Debug.Log($"Play Sound FX: {(GameManager.Instance.Settings.AudioSettings.PlaySoundFX ? "Yes" : "No")}");
    }
}
