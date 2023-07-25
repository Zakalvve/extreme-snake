using Assets.Scripts.Game.Score;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Levels;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.UI;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class SnakeScore : MonoBehaviour
{
    private EventEmitter Emitter;
    public int Score { get { return _score; } private set { _score = value < 0 ? 0 : value; } }
    public int FailureCost = 10;
    public int Multiplier { get { return MultiplierHelper.GetMultiplier(_streak); } }
    public ScoreMultiplier MultiplierHelper;
    public MultiplierGradients MultiplierColors;

    [SerializeField]
    private int _score;
    private int _streak;

    void Start() {
        MultiplierColors = GameManager.Instance.GlobalGradients;
        Emitter = GetComponent<Snake>().SnakeEmitter;
        Emitter.Subscribe<ScoreEventArgs>("ScoreOnEat",HandleEat);
        Emitter.Subscribe<StringEventArgs>("ScoreOnShrink",HandleShrink);
    }

    public void HandleEat(object sender,ScoreEventArgs args) {
        if (_streak < 0) _streak = 0;
        int oldMultiplier = Multiplier;
        _streak++;
        int multiplierCache = Multiplier;
        int scoreGained = args.PointsChange * multiplierCache;
        Score += scoreGained;
        ScoreEventArgs newArgs = new ScoreEventArgs(args.SnakeUUID,_score,scoreGained,oldMultiplier != multiplierCache, Color.white);
        newArgs.Score = this;
        newArgs.MultiplierColors = MultiplierColors;
        GameManager.Instance.GameEmitter.Emit($"{args.SnakeUUID}OnScoreChanged",this,newArgs);
    }

    public void HandleShrink(object sender,StringEventArgs args) {
        if (_streak > 0) _streak = 0;
        int oldScore = Score;
        Score -= FailureCost;
        int newScore = Score;

        ScoreEventArgs newArgs = new ScoreEventArgs(args.Text,_score,newScore - oldScore,true, Color.red);
        newArgs.Score = this;
        newArgs.MultiplierColors = MultiplierColors;
        GameManager.Instance.GameEmitter.Emit($"{args.Text}OnScoreChanged",this,newArgs);
    }
}