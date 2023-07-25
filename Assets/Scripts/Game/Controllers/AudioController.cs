using ExtremeSnake.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public AudioSource Source;
    private List<Action> Subscriptions = new List<Action>();
    private void Awake() {
        Source = GetComponent<AudioSource>();
    }

    private void Start() {
        Initialize();
    }

    public void Initialize() {
        if (Subscriptions.Count > 0) {
            Subscriptions.ForEach(unsub => unsub());
        }
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnVolumeChanged",SetVolume));
        Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnPlayMusicChanged",SetMute));
        SetVolume(this);
        SetMute(this);
    }

    private void SetVolume(object sender) {
        Source.volume = GameManager.Instance.Settings.AudioSettings.Volume/100f;
    }

    private void SetMute(object sender) {
        Source.mute = !GameManager.Instance.Settings.AudioSettings.PlayMusic;
    }
}
