using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Core;
using ExtremeSnake.Game.Data;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class Snake : MonoBehaviour, ISnakeData, IPausable {
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

        //set could be private
        public Vector2 MoveDirection { get; set; }

        public Transform SnakeTransform { get { return gameObject.transform; } }
        public EventEmitter SnakeEmitter { get; private set; }
        public bool Invulnerable { get; set; }

        //could be private
        public bool IsAlive { get; set; } = true;

        //could be moved as private variables in snake model


        //could be moved as private variables in snake logic
        public bool LockMovement { get; set; } = false;
        public Vector2 LockDirection { get; set; }
        public int Fullness { get; set; }

        //private properties
        private ISnakeLogic _snakeLogic { get; set; }

        public bool IsPaused { get; private set; } = false;

        private ISnakeModel _snakeModel;

        private List<Action> Subscriptions = new List<Action>();

        private void Start() {
            Vector2Int spawnPoint = (Vector2Int)GameManager.Instance.Level.Grid.WorldToCell(gameObject.transform.position);
            ViewData = new SnakeViewData(_headPrefab,_bodyPrefab,_sprites);

            MoveDirection = ReduceSnakeStartingDirections(InitialDirection);
            _snakeModel = new SnakeModel(this,spawnPoint,gameObject.layer);
            _snakeLogic = new SnakeLogic(this,_snakeModel);
            SnakeEmitter.Emit("OnSnakeStartComplete",this);
            GameManager.Instance.GameEmitter.Emit($"{UUID}OnSnakeStartComplete",this);
        }

        public void EnableSnake(object sender) {
            Subscriptions.Add(GameManager.Instance.GameEmitter.Subscribe("OnTick",HandleTick));
            GameManager.Instance.GameEmitter.Subscribe("GamePaused",HandlePause);
            GameManager.Instance.GameEmitter.Subscribe("ResumeGame",HandleResume);
            GameManager.Instance.GameEmitter.Subscribe<StringEventArgs>("OnSnakeDeath",HandleDeath);
        }

        public void AssignEmitter(EventEmitter emitter) {
            SnakeEmitter = emitter;
            SnakeEmitter.Subscribe<ControllerEventArgs>("OnChangeDirection",HandleChangeDirection);
            SnakeEmitter.Subscribe("OnControllerAttached",EnableSnake);
        }

        public void HandleTick(object sender) {
            if (SnakeEmitter != null && IsAlive && !IsPaused) {
                if (!_snakeLogic.OnMove()) return;
                if (!_snakeLogic.HandleHunger()) return;
            }
        }

        public void HandleDeath(object sender, StringEventArgs args) {
            if (UUID == args.Text) {
                //we are dead
                IsAlive = false;
                Subscriptions.ForEach(unsub => unsub());
                SnakeEmitter.UnsubscribeFromAll();
                SnakeEmitter = null;
                gameObject.SetActive(false);
                GameManager.Instance.GameEmitter.Emit("CameraDropFocus",this,new GameObjectEventArgs(Segments.First.Value.Segment));
            }
        }
        public void HandleChangeDirection(object sender, ControllerEventArgs args) {
            if (IsAlive && !IsPaused) _snakeLogic.OnChangeDirection(args.Direction);
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

        public void HandlePause(object sender) {
            Pause();
        }

        public void Pause() {
            IsPaused = true;
        }

        public void HandleResume(object sender) {
            Resume();
        }

        public void Resume() {
            IsPaused = false;
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