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

public class GameState : BaseMonobehaviourState<GameManager>
{
    public GameState(GameManager context) : base(context) { }

    public bool isPaused = false;
    public float elapsedTime = 0f;

    public override void TransitionTo() {

        _context._controllers = new List<IController>();

        _context.GameEmitter.Subscribe("OnGameOver",HandleGameOver);
        _context.GameEmitter.Subscribe("OnPause",HandlePause);
        _context.GameEmitter.Subscribe("OnLevelAwake",HandleLevelLoaded);
        CreateControllers();
    }

    public override void Update() { }

    public override void FixedUpdate() {
        if (!isPaused) {
            if (elapsedTime > _context.Settings.DifficultySettings.SnakeDifficulty.GetTickTimeFromSnakeSpeed()) {
                _context.GameEmitter.Emit("OnTick",this);
                elapsedTime = 0f;
            }
            elapsedTime += Time.deltaTime;
        }
    }

    public override void LateUpdate() { }

    public void HandleGameOver(object sender) {
        _context.GameEmitter.UnsubscribeFromAll();
        SceneManager.LoadScene(0);
        _context.ChangeState(new MenuState(_context));
    }
    public void HandlePause(object sender) {
        isPaused = isPaused ? false : true;
    }

    public void CreateControllers() {
        //initialize controllers
        ControllerSettings cs;
        cs = new ControllerSettings();

        if (_context.Settings.SnakeControllingEntity.Count == 0 && GameManager.isDevelopment) {
            //create default setup for development
            _context.Settings.SnakeControllingEntity.Add(new ControllingEntitySettings(_context.defaultSkin,"Sid", ParticipantType.PLAYER_1));
            _context.Settings.SnakeControllingEntity.Add(new ControllingEntitySettings(_context.defaultSkin,"SuckMyBalls23",ParticipantType.COMPUTER));
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
    }

    public void CreateSnake(IController controller,GameObject snakePrefab, ControllingEntitySettings entity) {
        GameObject snake = _context.Level.GetSnakeSpawner().Spawn(snakePrefab);
        EventEmitter emitter = new EventEmitter();
        Snake s = snake.GetComponent<Snake>();
        s.AssignEmitter(emitter);
        emitter.Subscribe("OnSnakeCreated", _ => {
            emitter.Emit("OnReskin",this,new ReskinEventArgs(entity.Skin));
            //if (entity.EntityType != ParticipantType.COMPUTER) {
                emitter.Emit("OnPlayerSnakeCreated",this);
            //}
            _context.Level.RegisterSnake(s.ExtractSegmentPositions());
        });

        controller.AssignEmitter(emitter);
    }
}
