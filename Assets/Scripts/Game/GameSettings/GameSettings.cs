using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameSettings
{
    //defaults
    public GameObject DefaultController;
    public GameObject DefaultSnake;
    public SnakeSprites DefaultSkin;
    public Difficulty DefaultDifficulty;

    //audio settings
    [SerializeField]
    public ExtremeSnake.Game.AudioSettings AudioSettings;
    //session settings
    public SessionData ActiveSession { get; set; }
}
