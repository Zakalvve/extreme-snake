using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Levels;
using UnityEngine;


namespace ExtremeSnake.Game.Snakes
{
    //controls the logic of a snake
    public class SnakeLogic : ISnakeLogic
    {
        private ISnakeData _data;
        private ISnakeModel _model;
        private SnakeDifficulty difficulty;
        public Vector2 NewDirection { get; set; } = Vector2.zero; //used to check against actual direction and track input changes for logic only

        public SnakeLogic(ISnakeData data, ISnakeModel model) {
            _data = data;
            _model = model;
            _model.Draw();
            _data.SnakeEmitter.Subscribe<EatEventArgs>("OnEat",HandleEat);

            difficulty = GameManager.Instance.Settings.ActiveSession.DifficultySettings.SnakeDifficulty;
            _data.Fullness = difficulty.InitialGraceLength + difficulty.ShrinkTimerLength;
        }

        public bool OnMove() {
            if (NewDirection != Vector2.zero) {
                NewDirection = _model.UpdateDirection(NewDirection);
            }
            //if (GameManager.Instance.Level.IsMoveValid(Vector2Int.RoundToInt(_model.HeadPosition + _model.MoveDirection), _data.Segments.First.Value.Segment.layer)) {
            if (GameManager.Instance.Level.IsMoveValid(GameManager.Instance.Level.CenterInCell(Vector2Int.RoundToInt(_model.HeadPosition)),_model.MoveDirection,GameManager.Instance.Level.Grid.cellSize.x, _data.Segments.First.Value.Segment.layer)) {
                _model.Move();
                _data.LockMovement = false;
            }
            else {
                GameManager.Instance.AudioControls.PlaySFX("fail");
                if (!_model.ChangeLength(-1)) {
                    return false;
                }
                if (!_data.LockMovement) {
                    _data.LockMovement = true;
                    _data.LockDirection = _data.Segments.First.Value.ModelPosition - _data.Segments.First.Next.Value.ModelPosition;
                }
                _data.SnakeEmitter.Emit("ScoreOnShrink",this, new StringEventArgs(_data.UUID));
            }
            _model.Draw();
            return true;
        }

        public bool HandleHunger() {
            _data.Fullness--;
            if (_data.Fullness == 0) {
                if (!_model.ChangeLength(-1,true)) {
                    return false;
                }
                _data.Fullness = difficulty.ShrinkTimerLength;
            }
            return true;
        }

        public void OnChangeDirection(Vector2 inputDirection) {
            Vector2 comparissonDir = _data.LockMovement ? _data.LockDirection : _model.MoveDirection;
            if (comparissonDir * -1 != inputDirection) {
                NewDirection = inputDirection;
                _data.SnakeEmitter.Emit("FinalSnakeDirection",this,new ControllerEventArgs(NewDirection));
            }
        }

        public void HandleEat(object sender,EatEventArgs args) {
            GameManager.Instance.AudioControls.PlaySFX("eat");
            _model.AddGrowth(args.FoodEaten.GrowthValue);
            _model.Draw();
            _data.Fullness += difficulty.ShrinkTimerLength * args.FoodEaten.GrowthValue;
            _data.SnakeEmitter.Emit("ScoreOnEat",this,new ScoreEventArgs(_data.UUID,args.FoodEaten.PointsValue));
        }
    }
}