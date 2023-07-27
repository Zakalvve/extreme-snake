using ExtremeSnake.Core;
using ExtremeSnake.Game;
using UnityEngine;

namespace Assets.Scripts.Game.Managers.Game_Manager.Concrete_States
{
    public class PausedState : BaseMonobehaviourState<GameManager>
    {
        public PausedState(GameManager context) : base(context) { }

        public override void TransitionTo() {
            Subscriptions.Add(_context.GameEmitter.Subscribe("EndSession", EndGame));
            Subscriptions.Add(_context.GameEmitter.Subscribe("TryResume", Resume));
            Subscriptions.Add(_context.GameEmitter.Subscribe("QuitGame",Quit));
            Subscriptions.Add(_context.GameEmitter.Subscribe("TryPause",Resume));
            Subscriptions.Add(_context.GameEmitter.Subscribe("OnEscape",Resume));
            _context.GameEmitter.Emit("GamePaused",this);
        }

        public void EndGame(object sender) {
            _context.Settings.ActiveSession.BypassFinalScoreboard = true;
            _context.ChangeState(new PostGameState(_context));
        }
        public void Resume(object sender) {
            _context.GameEmitter.Emit("ResumeGame",this); 
            UnsubscribeFromAll();
            _context.ChangeState(new GameState(_context));
        }
        public void Quit(object sender) {
            Application.Quit();
        }
        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        public override void Update() { }
    }
}
