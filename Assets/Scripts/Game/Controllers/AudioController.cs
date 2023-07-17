using ExtremeSnake.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    public AudioSource Source;
    private void Awake() {
        Source = GetComponent<AudioSource>();
    }

    private void Start() {
        GameManager.Instance.GameEmitter.Subscribe("OnVolumeChanged",SetVolume);
        GameManager.Instance.GameEmitter.Subscribe("OnPlayMusicChanged",SetMute);
        SetVolume(this);
        SetMute(this);
    }

    private void SetVolume(object sender) {
        Source.volume = GameManager.Instance.Settings.Volume/100f;
    }

    private void SetMute(object sender) {
        Source.mute = !GameManager.Instance.Settings.PlayMusic;
    }
}
