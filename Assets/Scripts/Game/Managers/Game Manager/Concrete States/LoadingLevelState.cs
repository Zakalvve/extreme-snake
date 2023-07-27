using System.Collections.Generic;
using ExtremeSnake.Core;
using ExtremeSnake.Game.Levels;
using ExtremeSnake.Game.Snakes;
using UnityEngine;

namespace ExtremeSnake.Game
{
    public class LoadingLevelState : BaseMonobehaviourState<GameManager>
    {
        public LoadingLevelState (GameManager context) : base(context) { }

        public List<Snake> snakes = new List<Snake>();
        public bool HasLoaded { get { return _snakesLoaded && _levelLoaded; } }
        private bool _snakesLoaded = false;
        private bool _levelLoaded = false;

        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        public override void TransitionTo() {
            Subscriptions.Add(_context.GameEmitter.Subscribe("OnLevelStartComplete",HandleLevelLoaded));
            if (GameManager.Instance.isDevelopment || _context.Settings.ActiveSession == null) {
                CreateDevEntryPoint();
            }

            _context.Settings.ActiveSession.PrepareControllers();
        }

        public override void Update() { 
            if (HasLoaded) {
                UnsubscribeFromAll();
                _context.GameEmitter.Emit("OnLoadComplete",this);
                _context.ChangeState(new IntroState(_context));
            }
        }

        public void RegisterPlayerReady() {
            if (_context.Settings.ActiveSession.Snakes.Count == _context.Settings.ActiveSession.Actors.Count) {
                _snakesLoaded = true;
            }
        }

        //this is for when we want to jump into the normal game flow half way along, we must provide some basic load data
        public void CreateDevEntryPoint() {
            Difficulty devDifficulty = ScriptableObject.Instantiate(_context.Settings.DefaultDifficulty);
            //devDifficulty.SnakeDifficulty.ShrinkTimerLength = 10000;
            _context.Settings.ActiveSession = new SessionData(new List<Actor>() { new Actor(_context.Settings.DefaultSkin,"Sid",ParticipantType.PLAYER_1) ,new Actor(_context.Settings.DefaultSkin,"SuckMyBalls23",ParticipantType.COMPUTER) });
            _context.Settings.ActiveSession.DifficultySettings = devDifficulty;
        }

        public void HandleLevelLoaded(object sender) {
            _context.Settings.ActiveSession.SessionLevel = (Level)sender;

            foreach (var ctrlSettings in _context.Settings.ActiveSession.ControllerData) {
                GameObject controllerGO = GameObject.Instantiate(_context.Settings.DefaultController);
                controllerGO.transform.parent = _context.gameObject.transform;
                IController controller = ctrlSettings.AttachControllerToGameObject(controllerGO);
                _context.Settings.ActiveSession.Controllers.Add(controller);
                foreach (var player in ctrlSettings.Pawns) {
                    player.ActorControls = controller;
                    CreateSnake(controller,_context.Settings.DefaultSnake,player);
                }
            }

            _levelLoaded = true;
        }

        public void CreateSnake(IController controller,GameObject snakePrefab,Actor entity) {
            EventEmitter emitter = new EventEmitter();
            GameObject snake = _context.Level.GetSnakeSpawner().Spawn(snakePrefab);
            Snake s = snake.GetComponent<Snake>();
            entity.ActorSnake = s;
            entity.ActorScore = snake.GetComponent<SnakeScore>();
            s.AssignEmitter(emitter);
            emitter.Subscribe("OnSnakeStartComplete",(object sender) => {
                ISnakeData data = (ISnakeData)sender;
                data.Name = entity.Name;
                SnakeCreatedEventArgs args = new SnakeCreatedEventArgs(data,entity,snake.GetComponent<SnakeScore>());

                //emit events to subscribers that snake creation is complete
                emitter.Emit("SnakeCreated",this,args);
                _context.GameEmitter.Emit("SnakeCreated",this,args);
                _context.Level.RegisterSnake(s.ExtractSegmentPositions());
                _context.Settings.ActiveSession.Snakes.Add(s);
                _context.Settings.ActiveSession.ActiveSnakes.Add(data.UUID,s);
                if (GameManager.Instance.isDevelopment) {
                    //data.Invulnerable = true;
                }
                RegisterPlayerReady();
            });

            controller.AssignEmitter(emitter);
        }
    }
}
