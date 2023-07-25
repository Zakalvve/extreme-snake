using Assets.Scripts.Game.Events;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
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
            _secondsRemaining = _context.Settings.ActiveSession.Duration;
            Subscriptions.Add(_context.GameEmitter.Subscribe<StringEventArgs>("OnSnakeDeath",HandleSnakeDeath));
            Subscriptions.Add(_context.GameEmitter.Subscribe("OnPause",HandlePause));
        }

        public override void Update() { }

        public override void FixedUpdate() {
            if (!_isPaused) {
                _elapsedTimeSinceLastTick += Time.deltaTime;
                _elapsedTimeSinceLastSecond += Time.deltaTime;

                //on each tick
                if (_elapsedTimeSinceLastTick > _context.Settings.ActiveSession.DifficultySettings.SnakeDifficulty.GetTickTimeFromSnakeSpeed()) {
                    _context.GameEmitter.Emit("OnTick",this);
                    _elapsedTimeSinceLastTick = 0f;
                }

                //on each second
                if (_elapsedTimeSinceLastSecond > 1f) {
                    _context.GameEmitter.Emit("OnSecondTick",this);
                    _elapsedTimeSinceLastSecond = 0f;
                    _secondsRemaining--;
                    if (_secondsRemaining <= 0) {
                        GameOver();
                    }
                }
            }
        }

        public override void LateUpdate() { }

        public void HandleSnakeDeath(object sender, StringEventArgs args) {
            _context.Settings.ActiveSession.ActiveSnakes.Remove(args.Text);
            if (_context.Settings.ActiveSession.ActiveSnakes.Count <= 1) {
                GameOver();
            }
        }

        public void GameOver() {
            UnsubscribeFromAll();
            _context.ChangeState(new PostGameState(_context));
        }

        public void HandlePause(object sender) {
            _isPaused = _isPaused ? false : true;
            _context.GameEmitter.Emit("TogglePause",this);
        }
    }
}