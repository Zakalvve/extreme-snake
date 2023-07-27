using ExtremeSnake.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ExtremeSnake.Game
{
    public class PostGameState : BaseMonobehaviourState<GameManager>
    {
        public PostGameState(GameManager context) : base(context) { }
        public override void TransitionTo() {
            if (_context.Settings.ActiveSession.BypassFinalScoreboard) {
                GameOver(this);
            }
            else {
                Subscriptions.Add(_context.GameEmitter.Subscribe("OnEnter",GameOver));
                Subscriptions.Add(_context.GameEmitter.Subscribe("OnFinish",GameOver));
                _context.GameEmitter.Emit("OnPostGame",this);
            }
        }

        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        public override void Update() { }

        public void GameOver(object sender) {
            _context.GameEmitter.Emit("OnGameOver",this);
            _context.GameEmitter.UnsubscribeFromAll();
            _context.Settings.ActiveSession.EndSession();
            SceneManager.LoadScene(0);
            _context.ChangeState(new MenuState(_context));
        }
    }
}