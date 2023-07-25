using System;
using UnityEngine;

namespace ExtremeSnake.Game
{
    [System.Serializable]
    public class AudioSettings
    {
        //audio settings
        [Range(0,100)]
        [SerializeField]
        private int _volume = 100;
        [SerializeField]
        private bool _playMusic = true;
        [SerializeField]
        private bool _playSoundFX = true;
        public int Volume { get { return _volume; } set { _volume = value; GameManager.Instance.GameEmitter.Emit("OnVolumeChanged",this); } }
        public bool PlayMusic { get { return _playMusic; } set { _playMusic = value; GameManager.Instance.GameEmitter.Emit("OnPlayMusicChanged",this); } }
        public bool PlaySoundFX { get { return _playSoundFX; } set { _playSoundFX = value; GameManager.Instance.GameEmitter.Emit("OnPlaySoundFXChanged",this); } }
    }
}
