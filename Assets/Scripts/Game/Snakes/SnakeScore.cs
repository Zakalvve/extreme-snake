using Assets.Scripts.Game.Score;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Food;
using ExtremeSnake.Game.Snakes;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class SnakeScore : MonoBehaviour
{
    public int Score { get { return _score; } private set { _score = value < 0 ? 0 : value; } }
    public int FailureCost = 10;
    public ScoreMultiplier MultiplierHelper;

    [SerializeField]
    private int _score;
    private int _streak;

    void Start() {
        EventEmitter emitter = GetComponent<Snake>().SnakeEmitter;
        emitter.Subscribe<EatEventArgs>("OnEat",HandleEat);
        emitter.Subscribe("OnShrink",HandleShrink);
    }

    public void HandleEat(object sender, EatEventArgs args) {
        if (_streak < 0) _streak = 0;
        _streak++;
        Score += args.FoodEaten.PointsValue * MultiplierHelper.GetMultiplier(_streak);
    }

    public void HandleShrink(object sender) {
        if (_streak > 0) _streak = 0;
        Score -= FailureCost;
    }
}