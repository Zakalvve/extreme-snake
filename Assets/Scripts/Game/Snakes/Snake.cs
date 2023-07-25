using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class Snake : MonoBehaviour, ISnakeData {
        //properties exposed in editor
        [SerializeField]
        private GameObject _headPrefab;
        [SerializeField]
        private GameObject _bodyPrefab;
        [SerializeField]
        private SnakeSprites _sprites;
        public SnakeStartingDirections InitialDirection { get; set; }

        //implement ISnakeData
        public string UUID { get; } = Guid.NewGuid().ToString();
        public string Name { get; set; }
        public SnakeViewData ViewData { get; set; }
        public int StartingLength { get { return GameManager.Instance.Settings.ActiveSession.DifficultySettings.SnakeDifficulty.SnakeStartingLength; } }
        public LinkedList<SnakeSegment> Segments { get; set; } = new LinkedList<SnakeSegment>();
        public Vector2 MoveDirection { get; set; }
        public Transform SnakeTransform { get { return gameObject.transform; } }
        public EventEmitter SnakeEmitter { get; private set; }
        public int Fullness { get; set; }
        public bool IsAlive { get; set; }
        public bool Invulnerable { get; set; }
        public bool LockMovement { get; set; } = false;
        public Vector2 LockDirection { get; set; }
        //private properties
        private ISnakeLogic _snakeLogic { get; set; }
        private ISnakeModel _snakeModel;

        private void Start() {
            Vector2Int spawnPoint = (Vector2Int)GameManager.Instance.Level.Grid.WorldToCell(gameObject.transform.position);
            ViewData = new SnakeViewData(_headPrefab,_bodyPrefab,_sprites);

            MoveDirection = ReduceSnakeStartingDirections(InitialDirection);
            _snakeModel = new SnakeModel(this,spawnPoint,gameObject.layer);
            _snakeLogic = new SnakeLogic(this,_snakeModel);
            SnakeEmitter.Emit("OnSnakeStartComplete",this);
        }

        public void EnableSnake(object sender) {
            GameManager.Instance.GameEmitter.Subscribe("OnTick",HandleTick);
        }

        public void AssignEmitter(EventEmitter emitter) {
            SnakeEmitter = emitter;
            SnakeEmitter.Subscribe<ControllerEventArgs>("OnChangeDirection",HandleChangeDirection);
            SnakeEmitter.Subscribe("OnControllerAttached",EnableSnake);
        }

        public void HandleTick(object sender) {
            if (SnakeEmitter != null) {
                if (!_snakeLogic.OnMove()) return;
                if (!_snakeLogic.HandleHunger()) return;
            }

        }
        public void HandleChangeDirection(object sender, ControllerEventArgs args) {
            _snakeLogic.OnChangeDirection(args.Direction);
        }
        public Vector2 ReduceSnakeStartingDirections(SnakeStartingDirections direction) {
            switch (direction) {
                case SnakeStartingDirections.UP:
                    return Vector2.up;
                case SnakeStartingDirections.DOWN:
                    return Vector2.down;
                case SnakeStartingDirections.LEFT:
                    return Vector2.left;
                case SnakeStartingDirections.RIGHT:
                    return Vector2.right;
                default: return Vector2.right;
            }
        }

        public List<LevelPosition> ExtractSegmentPositions() {
            return Segments.Select(segment => {
                return new LevelPosition(segment.ModelPosition, segment.Segment.layer);
            }).ToList();
        }

        public void RemoveAll() {
            _snakeModel.Clear();
        }
    }

    public enum SnakeStartingDirections {
        UP,
        DOWN,
        LEFT,
        RIGHT
    }

    public class LevelPosition {
        public Vector2Int Position;
        public int Layer;
        public string LayerName;
        public LevelPosition(Vector2Int pos, int layer) {
            Position = pos;
            Layer = layer;
            LayerName = LayerMask.LayerToName(layer);
        }
    }
}