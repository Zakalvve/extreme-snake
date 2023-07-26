using ExtremeSnake.Game;
using ExtremeSnake.Utils;
using System;
using ExtremeSnake.Core;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField]
    private AudioSource _musicSource, _sfxSource;
    private List<Action> Subscriptions = new List<Action>();
    public List<AudioClip> RandomClips;
    [SerializeField]
    public SDictionaryStringAudioClip Clips = new SDictionaryStringAudioClip();

    private void Awake() {
        _musicSource = GetComponent<AudioSource>();
    }

    private void Start() {
        Initialize();
    }

    public void PlaySFX(AudioClip clip) {
        _sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(string clipKey) {
        clipKey = clipKey.ToLower();
        _sfxSource.PlayOneShot(Clips[clipKey]);
    }

    public void PlayRandomSfx() {
        PlaySFX("eat");
    }

    public void Initialize() {
        if (Subscriptions.Count > 0) {
            Subscriptions.ForEach(unsub => unsub());
        }

        //volume levels
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnMasterVolumeChanged",SetMasterVolume));
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnMusicVolumeChanged",SetMusicVolume));
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnSfxVolumeChanged",SetSfxVolume));

        SetMasterVolume(this);
        SetMusicVolume(this);
        SetSfxVolume(this);

        //mutes
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnToggleMasterMute",ToggleMasterMute));
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnToggleMusicMute",ToggleMusicMute));
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnToggleSfxMute",ToggleSfxMute));

        ToggleMasterMute(this);
        ToggleMusicMute(this);
        ToggleSfxMute(this);
    }

    //volume levels
    private void SetMasterVolume(object sender) {
        SetMusicVolume(this);
        SetSfxVolume(this);
    }
    private void SetMusicVolume(object sender) {
        _musicSource.volume = (GameManager.Instance.Settings.AudioSettings.MusicVolume / 100f) * (GameManager.Instance.Settings.AudioSettings.MasterVolume / 100f);
    }
    private void SetSfxVolume(object sender) {
        _sfxSource.volume = (GameManager.Instance.Settings.AudioSettings.SFXVolume / 100f) * (GameManager.Instance.Settings.AudioSettings.MasterVolume / 100f);
    }

    //mutes
    private void ToggleMasterMute(object sender) {
        ToggleMusicMute(this);
        ToggleSfxMute(this);
    }

    private void ToggleMusicMute(object sender) {
        _musicSource.mute = GameManager.Instance.Settings.AudioSettings.MusicMute || GameManager.Instance.Settings.AudioSettings.MasterMute;
    }

    private void ToggleSfxMute(object sender) {
        _sfxSource.mute = GameManager.Instance.Settings.AudioSettings.SFXMute || GameManager.Instance.Settings.AudioSettings.MasterMute;
    }
}
