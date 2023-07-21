using Assets.Scripts.Game.Score;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Food;
using ExtremeSnake.Game.Snakes;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class SnakeScore : MonoBehaviour
{
    public int Score { get { return _score; } private set { _score = value < 0 ? 0 : value; } }
    public int FailureCost = 10;
    public int Multiplier { get { return MultiplierHelper.GetMultiplier(_streak); } }
    public ScoreMultiplier MultiplierHelper;

    [SerializeField]
    private int _score;
    private int _streak;

    void Start() {
        EventEmitter emitter = GetComponent<Snake>().SnakeEmitter;
        emitter.Subscribe<ScoreEventArgs>("ScoreOnEat",HandleEat);
        emitter.Subscribe<ScoreEventArgs>("ScoreOnShrink",HandleShrink);
    }

    public void HandleEat(object sender,ScoreEventArgs args) {
        if (_streak < 0) _streak = 0;
        _streak++;
        Score += args.Points * MultiplierHelper.GetMultiplier(_streak);
        args.Score = this;
        GameManager.Instance.GameEmitter.Emit($"{args.SnakeUUID}OnScoreChanged",this, args);
    }

    public void HandleShrink(object sender, ScoreEventArgs args) {
        if (_streak > 0) _streak = 0;
        Score -= FailureCost;
        args.Score = this;
        GameManager.Instance.GameEmitter.Emit($"{args.SnakeUUID}OnScoreChanged",this,args);
    }
}