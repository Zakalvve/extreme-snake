using System.Collections.Generic;
using UnityEngine;
using ExtremeSnake.Core;
using ExtremeSnake.Game.Levels;
using ExtremeSnake.Game.Food;
using ExtremeSnake.Game.Snakes;

namespace ExtremeSnake.Game
{
    //Make a State Machine
    public class GameManager : Singleton<GameManager>, IStateful<IMonobehaviourState>
    {
        //for development and starting from various non default entry points
        public StartingState startingState;

        private IMonobehaviourState _state;

        public EventEmitter GameEmitter { get; private set; }

        public GameSettings Settings;

        public Level Level { get; set; }

        public List<IController> _controllers;

        protected override void Awake() {
            base.Awake();
            GameEmitter = new EventEmitter();

            if (startingState == StartingState.MENU)
                _state = new MenuState(this);
            else
                _state = new GameState(this);

            _state.TransitionTo();
        }

        private void Update() {
            _state.Update();
        }
        void FixedUpdate() {
            _state.FixedUpdate();
        }

        private void LateUpdate() {
            _state.LateUpdate();
        }

        public void ChangeState(IMonobehaviourState state) {
            _state = state;
            _state.TransitionTo();
        }
    }

    public enum StartingState {
        MENU,
        GAME
    }
}

