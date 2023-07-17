//using ExtremeSnake.Core;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using UnityEngine.UIElements.Experimental;

//namespace ExtremeSnake.Game.Snakes
//{
//    public class SnakeOld : MonoBehaviour
//    {
//        [Range(2,10)]
//        public int StartingLength;
//        [SerializeField]
//        private GameObject _headPrefab;
//        [SerializeField]
//        private GameObject _segmentPrefab;
//        [SerializeField]
//        private SnakeSpites _sprites;

//        private LinkedList<SnakeSegment> _snake = new LinkedList<SnakeSegment>();
//        private EventEmitter<Vector2> _controller;

//        private Vector2Int _spawnPosition;
//        private Vector2 _moveDirection = Vector2.right;
//        private Vector2 _newDirection = Vector2.zero;

//        private void Start() {
//            _spawnPosition = (Vector2Int)GameManager.Instance.Level.Grid.WorldToCell(gameObject.transform.position);
//            _controller = GameManager.Instance.RequestController();
//            _controller.Subscribe("OnChangeDirection",HandleChangeDirection);
//            GameManager.Instance.GameEmitter.Subscribe("OnTick",HandleTick);
//            //create segments
//            _snake.AddFirst(CreateSegment(_headPrefab, _spawnPosition));
//            _snake.AddLast(CreateSegment(_segmentPrefab,_spawnPosition + Vector2Int.RoundToInt(_moveDirection * -1)));
//            ChangeLength(StartingLength - 2);
//        }

//        public void HandleTick() {
//            if (_newDirection != Vector2.zero) {
//                _moveDirection = _newDirection;
//                _newDirection = Vector2.zero;
//            }
//            //update the model
//            MoveSnake();
//            DrawSnake();
//        }
//        public void HandleChangeDirection(Vector2 direction) {
//            if (_moveDirection * -1 != direction) {
//                _newDirection = direction;
//            }
//        }

//        public void ChangeLength(int amount) {
//            if (amount > 0) {
//                Eat(amount);
//            }
//            else if (amount < 0) {
//                Shrink(Mathf.Abs(amount));
//            }
//        }
//        public void Eat(int amount) {
//            Vector2Int directionOfGrowth = _snake.Last.Value.ModelPosition - _snake.Last.Previous.Value.ModelPosition;
//            for (int i = 0; i < amount; i++) {
//                _snake.AddLast(CreateSegment(_segmentPrefab, _snake.Last.Value.ModelPosition + directionOfGrowth));
//            }
//        }
//        public void Shrink(int amount) {
//            for (int i = 0; i < amount; i++) {
//                if (_snake.Count == 2) {
//                    GameManager.Instance.GameEmitter.Emit("OnGameOver");
//                    return;
//                }
//                RemoveSegement();
//            }
//        }

//        private void MoveSnake() {
//            if (!IsMoveValid(GameManager.Instance.Level.CenterInCell(_snake.First.Value.ModelPosition),Vector2Int.RoundToInt(_moveDirection),GameManager.Instance.Level.Grid.cellSize.x)) {
//                Shrink(1);
//                return;
//            }
//            for (var segment = _snake.Last; segment != null; segment = segment.Previous) {
//                if (segment.Previous == null) {
//                    segment.Value.ModelPosition += Vector2Int.RoundToInt(_moveDirection);
//                    continue;
//                }
//                segment.Value.ModelPosition = segment.Previous.Value.ModelPosition;
//            }
//        }
//        private void DrawSnake() {
//            //draw the snake
//            for (var segment = _snake.First; segment != null; segment = segment.Next) {
//                segment.Value.Segment.transform.position = ModelPositionToView(segment.Value.ModelPosition);

//                if (segment == _snake.First) segment.Value.Renderer.sprite = _sprites.GetHeadSprite(segment);
//                else if (segment == _snake.Last) segment.Value.Renderer.sprite = _sprites.GetTailSprite(segment);
//                else segment.Value.Renderer.sprite = _sprites.GetBodySprite(segment);
//            }
//        }
//        private void RemoveSegement() {
//            GameObject segmentGO = _snake.Last.Value.Segment;
//            _snake.RemoveLast();
//            GameObject.Destroy(segmentGO);
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
//        private SnakeSegment CreateSegment(GameObject prefab, Vector2Int position) {
//            GameObject segment = GameObject.Instantiate(prefab);
//            segment.transform.localScale = GameManager.Instance.Level.Grid.cellSize;
//            segment.transform.parent = gameObject.transform;
//            segment.transform.position = ModelPositionToView(position);
//            return new SnakeSegment(segment, position);
//        }
//        private Vector2 ModelPositionToView(Vector2Int modelPosition) {
//            return GameManager.Instance.Level.CenterInCell(modelPosition);
//        }
//    }
//}
