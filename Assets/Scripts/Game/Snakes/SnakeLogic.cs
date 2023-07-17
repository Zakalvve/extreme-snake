using ExtremeSnake.Game;
using ExtremeSnake.Game.Food;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ExtremeSnake.Game.Snakes
{
    //controls the logic of a snake
    public class SnakeLogic : ISnakeLogic
    {
        private ISnakeData _data;
        private ISnakeModel _model;
        public Vector2 NewDirection { get; set; } = Vector2.zero; //used to check against actual direction and track input changes for logic only

        public SnakeLogic(ISnakeData data, ISnakeModel model) {
            _data = data;
            _model = model;
            _model.Draw();
            _data.SnakeEmitter.Subscribe<EatEventArgs>("OnEat",HandleEat);
        }

        public void OnMove() {
            if (NewDirection != Vector2.zero) {
                NewDirection = _model.UpdateDirection(NewDirection);
            }
            //if (GameManager.Instance.Level.IsMoveValid(Vector2Int.RoundToInt(_model.HeadPosition + _model.MoveDirection), _data.Segments.First.Value.Segment.layer)) {
            if (GameManager.Instance.Level.IsMoveValid(GameManager.Instance.Level.CenterInCell(Vector2Int.RoundToInt(_model.HeadPosition)),_model.MoveDirection,GameManager.Instance.Level.Grid.cellSize.x, _data.Segments.First.Value.Segment.layer)) {
                _model.Move();
            }
            else {
                _model.ChangeLength(-1);
                _data.SnakeEmitter.Emit("OnShrink",this);
            }
            _model.Draw();
        }

        public void OnChangeDirection(Vector2 inputDirection) {
            if (_model.MoveDirection * -1 != inputDirection) {
                NewDirection = inputDirection;
            }
        }

        public void HandleEat(object sender, EatEventArgs args) {
            _model.ChangeLength(args.FoodEaten.GrowthValue);
            _model.Draw();
        }
    }
}