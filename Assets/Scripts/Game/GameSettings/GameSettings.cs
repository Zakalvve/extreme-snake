using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.UI;
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
    public MultiplierGradients DefaultMultiplierGradients;

    //audio settings
    [SerializeField]
    public ExtremeSnake.Game.AudioSettings AudioSettings;
    //session settings
    public SessionData ActiveSession { get; set; }
    public Actor Player1 { get; set; } = null;
    public Actor Player2 { get; set; } = null;
}
