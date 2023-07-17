using ExtremeSnake.Core;
using System;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using Assets.Scripts.Game.Events;

namespace ExtremeSnake.Game
{
    public class MenuState : BaseMonobehaviourState<GameManager>
    {
        public MenuState(GameManager context) : base(context) { }
        private List<Action> _eventSubscriptions = new List<Action>();
        public override void TransitionTo() {
            _eventSubscriptions.Add(_context.GameEmitter.Subscribe<LoadLevelArgs>("OnLoadGame", HandleLoadLevel));
        }

        public override void Update() { }

        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        private void HandleLoadLevel(object sender, LoadLevelArgs args) {
            _context.GameEmitter.UnsubscribeFromAll();
            SceneManager.LoadScene(args.LevelToLoad.SceneName);
            _context.ChangeState(new GameState(_context));
        }
    }
}
