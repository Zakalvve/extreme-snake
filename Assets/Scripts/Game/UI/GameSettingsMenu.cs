using ExtremeSnake.Game;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettingsMenu : MonoBehaviour
{
    //volume levels
    public Slider _masterVolumeSlider;
    public Slider _musicVolumeSlider;
    public Slider _sfxVolumeSlider;

    //mutes
    public GameObject MasterToggleOn;
    public GameObject MasterToggleOff;
    public GameObject MusicToggleOn;
    public GameObject MusicToggleOff;
    public GameObject SoundFXToggleOn;
    public GameObject SoundFXToggleOff;

    public bool ShouldDebug;

    private void Awake() {
        OnActive();
        if (ShouldDebug) {
            GameManager.Instance.GameEmitter.Subscribe("OnVolumeChanged",DebugSettingsValues);
            GameManager.Instance.GameEmitter.Subscribe("OnPlayMusicChanged",DebugSettingsValues);
            GameManager.Instance.GameEmitter.Subscribe("OnPlaySoundFXChanged",DebugSettingsValues);
        }
    }

    public void OnActive() {
        //volume levels
        _masterVolumeSlider.value = GameManager.Instance.Settings.AudioSettings.MasterVolume;
        _musicVolumeSlider.value = GameManager.Instance.Settings.AudioSettings.MusicVolume;
        _sfxVolumeSlider.value = GameManager.Instance.Settings.AudioSettings.SFXVolume;


        //mutes
        bool masterToggle = GameManager.Instance.Settings.AudioSettings.MasterMute;
        bool musicToggle = GameManager.Instance.Settings.AudioSettings.MusicMute;
        bool sfxToggle = GameManager.Instance.Settings.AudioSettings.SFXMute;

        MasterToggleOn.SetActive(!masterToggle);
        MasterToggleOff.SetActive(masterToggle);

        MusicToggleOn.SetActive(!musicToggle);
        MusicToggleOff.SetActive(musicToggle);

        SoundFXToggleOn.SetActive(!sfxToggle);
        SoundFXToggleOff.SetActive(sfxToggle);
    }

    //volume levels
    public void OnMasterVolumeSliderChanged() {
        GameManager.Instance.Settings.AudioSettings.MasterVolume = (int)_masterVolumeSlider.value;
    }

    public void OnMusicVolumeSliderChanged() {
        GameManager.Instance.Settings.AudioSettings.MusicVolume = (int)_musicVolumeSlider.value;
    }

    public void OnSfxVolumeSliderChanged() {
        GameManager.Instance.Settings.AudioSettings.SFXVolume = (int)_sfxVolumeSlider.value;
    }

    //mutes
    public void OnToggleMasterMute() {
        GameManager.Instance.Settings.AudioSettings.MasterMute = !GameManager.Instance.Settings.AudioSettings.MasterMute;
    }
    public void OnToggleMusicMute() {
        GameManager.Instance.Settings.AudioSettings.MusicMute = !GameManager.Instance.Settings.AudioSettings.MusicMute;
    }

    public void OnToggleSfxMute() {
        GameManager.Instance.Settings.AudioSettings.SFXMute = !GameManager.Instance.Settings.AudioSettings.SFXMute;
    }


    public void DebugSettingsValues(object sender) {
        Debug.Log($"Volume Level: {GameManager.Instance.Settings.AudioSettings.MasterVolume}");
        Debug.Log($"Play Music: {(GameManager.Instance.Settings.AudioSettings.MusicMute ? "Yes" : "No")}");
        Debug.Log($"Play Sound FX: {(GameManager.Instance.Settings.AudioSettings.SFXMute ? "Yes" : "No")}");
    }
}
