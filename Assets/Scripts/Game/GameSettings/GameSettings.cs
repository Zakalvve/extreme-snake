using ExtremeSnake.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    public int Volume { get { return _volume; } set { _volume = value; GameManager.Instance.GameEmitter.Emit("OnVolumeChanged",this); } }
    public bool PlayMusic { get { return _playMusic; } set { _playMusic = value; GameManager.Instance.GameEmitter.Emit("OnPlayMusicChanged",this); } }
    public bool PlaySoundFX { get { return _playSoundFX; } set { _playSoundFX = value; GameManager.Instance.GameEmitter.Emit("OnPlaySoundFXChanged",this); } }

    [Range(0,100)]
    [SerializeField]
    private int _volume = 100;
    [SerializeField]
    private bool _playMusic = true;
    [SerializeField]
    private bool _playSoundFX = true;

    //base game object setup to detect user input
    public GameObject BaseEntityControllerPrefab;
    //the prefab for a controllable snake
    public GameObject ExtremeSnakePrefab;

    public Difficulty DifficultySettings;

    public List<ControllingEntitySettings> SnakeControllingEntity { get; set; } = new List<ControllingEntitySettings>();
    public List<ControllerSettings> ControllerInfo { get; set; } = new List<ControllerSettings>();
    public int Duration { get; set; } = 5 * 60; //5 minutes
}
