using Assets.Scripts.Game.Events;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Levels;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.SceneManagement;

namespace ExtremeSnake.Game
{
    public class GameState : BaseMonobehaviourState<GameManager>
    {
        public GameState(GameManager context) : base(context) { }

        private bool _isPaused = false;
        private float _elapsedTimeSinceLastSecond = 0f;
        private float _elapsedTimeSinceLastTick = 0f;
        private int _secondsRemaining;

        public override void TransitionTo() {
            _secondsRemaining = _context.Settings.Duration;
            _context.GameEmitter.Subscribe("OnGameOver",HandleGameOver);
            _context.GameEmitter.Subscribe("OnPause",HandlePause);
        }

        public override void Update() { }

        public override void FixedUpdate() {
            if (!_isPaused) {
                _elapsedTimeSinceLastTick += Time.deltaTime;
                _elapsedTimeSinceLastSecond += Time.deltaTime;

                //on each tick
                if (_elapsedTimeSinceLastTick > _context.Settings.DifficultySettings.SnakeDifficulty.GetTickTimeFromSnakeSpeed()) {
                    _context.GameEmitter.Emit("OnTick",this);
                    _elapsedTimeSinceLastTick = 0f;
                }

                //on each second
                if (_elapsedTimeSinceLastSecond > 1f) {
                    _context.GameEmitter.Emit("OnSecondTick",this);
                    _elapsedTimeSinceLastSecond = 0f;
                    _secondsRemaining--;
                    if (_secondsRemaining <= 0) {
                        _context.GameEmitter.Emit("OnGameOver",this);
                    }
                }
            }
        }

        public override void LateUpdate() { }

        public void HandleGameOver(object sender) {
            _context.GameEmitter.UnsubscribeFromAll();
            SceneManager.LoadScene(0);
            _context.ChangeState(new MenuState(_context));
        }

        public void HandlePause(object sender) {
            _isPaused = _isPaused ? false : true;
            _context.GameEmitter.Emit("TogglePause",this);
        }
    }
}