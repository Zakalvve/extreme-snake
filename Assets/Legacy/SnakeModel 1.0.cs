//using ExtremeSnake.Core;
//using System.Collections.Generic;
//using UnityEngine;

//namespace ExtremeSnake.Game.Snakes
//{
//    public class SnakeModelv1
//    {
//        private LinkedList<Vector2Int> _snakeModel;
//        private EventEmitter<SnakeEventArgs> _emitter;


//        public SnakeModelv1(Vector2Int headPos,Vector2 facing,EventEmitter<SnakeEventArgs> emitter) {
            
//            _emitter = emitter;
//            _emitter.Subscribe("OnMoveSnake",HandleMove);
//            _emitter.Subscribe("OnEat",HandleEat);
//            _emitter.Subscribe("OnShrink",HandleShrink);

//            _snakeModel = new LinkedList<Vector2Int>();
//            _snakeModel.AddFirst(headPos);
//            _snakeModel.AddLast(headPos + Vector2Int.RoundToInt(facing * -1));

//            ChangeLength(8);
//        }
//        public void HandleMove(SnakeEventArgs args) {
//            if (!IsMoveValid(GameManager.Instance.Level.CenterInCell(_snakeModel.First.Value),Vector2Int.RoundToInt(args.MoveDirection),GameManager.Instance.Level.Grid.cellSize.x)) {
//                Shrink(1);
//                return;
//            }
//            for (var segment = _snakeModel.Last; segment != null; segment = segment.Previous) {
//                if (segment.Previous == null) {
//                    segment.Value += Vector2Int.RoundToInt(args.MoveDirection);
//                    continue;
//                }
//                segment.Value = segment.Previous.Value;
//            }
//            _emitter.Emit("OnModelChanged",new SnakeEventArgs(_snakeModel));
//        }
//        public void ChangeLength(int amount) {
//            if (amount > 0) {
//                Eat(amount);
//            }
//            else if (amount < 0) {
//                Shrink(Mathf.Abs(amount));
//            }
//        }
//        public void HandleEat(SnakeEventArgs args) {
//            Eat(args.Value);
//        }
//        public void HandleShrink(SnakeEventArgs args) {
//            Shrink(args.Value);
//        }
//        private void Eat(int amount) {
//            Vector2Int direction = _snakeModel.Last.Value - _snakeModel.Last.Previous.Value;
//            for (int i = 0; i < amount; i++) {
//                _snakeModel.AddLast(_snakeModel.Last.Value + direction);
//            }
//            _emitter.Emit("OnModelChanged",new SnakeEventArgs(_snakeModel));
//        }
//        private void Shrink(int amount) {
//            for (int i = 0; i < amount; i++) {
//                if (_snakeModel.Count == 2) {
//                    _emitter.Emit("OnModelChanged",new SnakeEventArgs(_snakeModel));
//                    GameManager.Instance.GameEmitter.Emit("OnGameOver");
//                    return;
//                }
//                _snakeModel.RemoveLast();
//            }
//            _emitter.Emit("OnModelChanged",new SnakeEventArgs(_snakeModel));
//        }
//        private bool IsMoveValid(Vector2 fromPos,Vector2 dir,float distance) {
//            //raycast in the move direction to check if the new cell is valid to move into
//            int layerMask = 1 << 3;
//            var hit = Physics2D.Raycast(fromPos,dir,distance,layerMask);
//            if (hit.collider != null) {
//                return false;
//            }
//            return true;
//        }
//    }
//}