using ExtremeSnake.Core;
using ExtremeSnake.Game.Levels;
using ExtremeSnake.Game.UI;

namespace ExtremeSnake.Game
{
    //Make a State Machine
    public class GameManager : Singleton<GameManager>, IStateful<IMonobehaviourState>
    {
        //for development and starting from various non default entry points
        public StartingState startingState;
        public bool isDevelopment = true;
        private IMonobehaviourState _state;
        public EventEmitter GameEmitter { get; private set; }
        public GameSettings Settings;
        public AudioController AudioControls;
        public Level Level { get { return Settings.ActiveSession.SessionLevel; } }

        public static bool IsReleaseVersion = true;

        protected override void Awake() {
            base.Awake();
            GameEmitter = new EventEmitter();

            if (startingState == StartingState.MENU)
                _state = new MenuState(this);
            else
                _state = new LoadingLevelState(this);

            AudioControls = GetComponentInChildren<AudioController>();

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
            AudioControls.Initialize();
        }
    }

    //for developmenmt
    public enum StartingState {
        MENU,
        GAME
    }
}

