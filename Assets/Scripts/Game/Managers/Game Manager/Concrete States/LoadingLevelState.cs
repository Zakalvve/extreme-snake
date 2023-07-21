using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            _context._controllers = new List<IController>();
            _context.GameEmitter.Subscribe("OnLevelStartComplete",HandleLevelLoaded);
            CreateControllers();
        }

        public override void Update() { 
            if (HasLoaded) {
                _context.ChangeState(new GameState(_context));
            }
        }

        public void RegisterPlayerReady(Snake s) {
            snakes.Add(s);
            if (snakes.Count == _context.Settings.SnakeControllingEntity.Count) {
                _snakesLoaded = true;
            }
        }

        public void CreateControllers() {
            //initialize controllers
            ControllerSettings cs;
            cs = new ControllerSettings();

            if (_context.Settings.SnakeControllingEntity.Count == 0 && GameManager.isDevelopment) {
                //create default setup for development
                _context.Settings.SnakeControllingEntity.Add(new ControllingEntitySettings(_context.defaultSkin,"Sid",ParticipantType.PLAYER_1));
                _context.Settings.SnakeControllingEntity.Add(new ControllingEntitySettings(_context.defaultSkin,"SuckMyBalls23",ParticipantType.COMPUTER));
                _context.Settings.DifficultySettings.SnakeDifficulty.SnakeSpeed = 5;
                _context.Settings.DifficultySettings.SnakeDifficulty.SnakeStartingLength = 12;
            }

            //setup player controller
            GameManager.Instance.Settings.SnakeControllingEntity.Where(entity => {
                return entity.EntityType == ParticipantType.PLAYER_1 || entity.EntityType == ParticipantType.PLAYER_2;
            }).ToList().ForEach(player => {
                cs.Entities.Add(player);
            });

            if (cs.Entities.Count == 2) cs.CreateAttachAction<LocalTwoPlayerController>();
            else cs.CreateAttachAction<PlayerController>();

            _context.Settings.ControllerInfo.Add(cs);

            //setup AI controllers
            GameManager.Instance.Settings.SnakeControllingEntity.Where(entity => {
                return entity.EntityType == ParticipantType.COMPUTER;
            }).ToList().ForEach(computer => {
                cs = new ControllerSettings();
                cs.CreateAttachAction<AIController>();
                cs.Entities.Add(computer);
                _context.Settings.ControllerInfo.Add(cs);
            });
        }
        public void HandleLevelLoaded(object sender) {
            //normally would cycle through game settings to determine number of players/computers create controllers for each one
            //for now we will create a single player controller
            _context.Level = (Level)sender;

            foreach (var ctrlSettings in _context.Settings.ControllerInfo) {
                GameObject controllerGO = GameObject.Instantiate(_context.Settings.BaseEntityControllerPrefab);
                controllerGO.transform.parent = _context.gameObject.transform;
                IController controller = ctrlSettings.AttachControllerToGameObject(controllerGO);
                _context._controllers.Add(controller);
                foreach (var player in ctrlSettings.Entities) {
                    CreateSnake(controller,_context.Settings.ExtremeSnakePrefab,player);
                }
            }

            _levelLoaded = true;
        }

        public void CreateSnake(IController controller,GameObject snakePrefab,ControllingEntitySettings entity) {
            EventEmitter emitter = new EventEmitter();
            GameObject snake = _context.Level.GetSnakeSpawner().Spawn(snakePrefab);
            Snake s = snake.GetComponent<Snake>();
            s.AssignEmitter(emitter);
            emitter.Subscribe("OnSnakeStartComplete",(object sender) => {
                ISnakeData data = (ISnakeData)sender;
                SnakeCreatedEventArgs args = new SnakeCreatedEventArgs(data,entity,snake.GetComponent<SnakeScore>());

                //emit events to subscribers that snake creation is complete
                emitter.Emit("SnakeCreated",this,args);
                _context.GameEmitter.Emit("SnakeCreated",this,args);

                _context.Level.RegisterSnake(s.ExtractSegmentPositions());
                RegisterPlayerReady(s);
            });

            controller.AssignEmitter(emitter);
        }
    }
}
