using System;
using UnityEngine;

namespace ExtremeSnake.Game
{
    [System.Serializable]
    public class AudioSettings
    {
        //volume levels
        [Range(0,100)]
        [SerializeField]
        private int _masterVolume = 100;
        [Range(0,100)]
        [SerializeField]
        private int _musicVolume = 100;
        [Range(0,100)]
        [SerializeField]
        private int _sfxVolume = 100;

        public int MasterVolume { get { return _masterVolume; } set { _masterVolume = value; GameManager.Instance.GameEmitter.Emit("OnMasterVolumeChanged",this); } }
        public int MusicVolume { get { return _musicVolume; } set { _musicVolume = value; GameManager.Instance.GameEmitter.Emit("OnMusicVolumeChanged",this); } }
        public int SFXVolume { get { return _sfxVolume; } set { _sfxVolume = value; GameManager.Instance.GameEmitter.Emit("OnSfxVolumeChanged",this); } }


        //mutes
        [SerializeField]
        private bool _masterMute;
        [SerializeField]
        private bool _musicMute = true;
        [SerializeField]
        private bool _sfxMute = true;
        
        public bool MasterMute { get { return _masterMute; } set { _masterMute = value; GameManager.Instance.GameEmitter.Emit("OnToggleMasterMute",this); } }
        public bool MusicMute { get { return _musicMute; } set { _musicMute = value; GameManager.Instance.GameEmitter.Emit("OnToggleMusicMute",this); } }
        public bool SFXMute { get { return _sfxMute; } set { _sfxMute = value; GameManager.Instance.GameEmitter.Emit("OnToggleSfxMute",this); } }
    }
}
