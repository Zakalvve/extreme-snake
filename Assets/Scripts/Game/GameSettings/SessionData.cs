using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.Levels;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnake.Game
{
    //a session exncapsulates a single session
    //it is responsible for storing and cleaning up all the data for the session when it is terminated
    public class SessionData
    {
        private static int _defaultDuration = 5 * 60; //5 minutes

        public SessionData(int duration, List<Actor> actors, Difficulty difficulty) {
            Duration = duration;
            Actors = actors;
            DifficultySettings = difficulty;
        }

        public float ElapsedTimeSinceLastSecond { get; set; } = 0f;
        public float ElapsedTimeSinceLastTick { get; set; } = 0f;
        public int SecondsRemaining { get; set; } = 0;
        public bool BypassFinalScoreboard { get; set; } = false;
        public SessionData(List<Actor> actors,Difficulty difficulty) : this(SessionData._defaultDuration, actors, difficulty) { }
        public SessionData(List<Actor> actors) : this(SessionData._defaultDuration,actors,GameManager.Instance.Settings.DefaultDifficulty) { }

        //session actors
        public int Duration { get; set; }
        public List<Actor> Actors { get; set; } = new List<Actor>();
        public Difficulty DifficultySettings;

        //derrived settings
        public List<ControllerSettings> ControllerData { get; set; } = new List<ControllerSettings>();
        public List<IController> Controllers = new List<IController>();
        public List<Snake> Snakes = new List<Snake>();
        public Dictionary<string,Snake> ActiveSnakes = new Dictionary<string, Snake>();
        public Level SessionLevel { get; set; }

        public void PrepareControllers() {
            //prepare the ControllerInfo from the Actors
            //initialize controllers
            ControllerSettings controllerSettings = new ControllerSettings();

            //setup player controller (there is only ever one for now)
            Actors.Where(actor => {
                return actor.ActorType == ParticipantType.PLAYER_1 || actor.ActorType == ParticipantType.PLAYER_2;
            }).ToList().ForEach(playerActor => {
                controllerSettings.Pawns.Add(playerActor);
            });

            if (controllerSettings.Pawns.Count == 2) controllerSettings.CreateAttachAction<LocalTwoPlayerController>();
            else controllerSettings.CreateAttachAction<PlayerController>();

            ControllerData.Add(controllerSettings);

            //setup AI controllers
            Actors.Where(entity => {
                return entity.ActorType == ParticipantType.COMPUTER;
            }).ToList().ForEach(computer => {
                controllerSettings = new ControllerSettings();
                controllerSettings.CreateAttachAction<AIController>();
                controllerSettings.Pawns.Add(computer);
                ControllerData.Add(controllerSettings);
            });
        }

        public void EndSession() {
            Controllers.ForEach(controller => {
                GameObject.Destroy(controller.GetGameObject());
            });
            Snakes.ForEach(snake => {
                snake.RemoveAll();
                GameObject.Destroy(snake.gameObject);
            });
        }
    }
}
