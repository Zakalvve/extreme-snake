using Assets.Scripts.Game.Data;
using Assets.Testing;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using Pipeline;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Collections;
using Assets.Scripts.Game.Controllers;

namespace ExtremeSnake.Game
{
    public class IntroState : BaseMonobehaviourState<GameManager>
    {
        public IntroState(GameManager context) : base(context) { }
        private CinematicController cineControls;


        public override void TransitionTo() {
            if (!GameManager.isDevelopment) {
                cineControls = _context.GetComponent<CinematicController>();
                cineControls.PlayIntro(CinematicComplete);
            }
            else CinematicComplete();
        }

        public void CinematicComplete() {
            UnsubscribeFromAll();
            _context.ChangeState(new GameState(_context));
        }

        public override void FixedUpdate() { }

        public override void LateUpdate() { }

        public override void Update() { }

        public void HandleEnter(object sender) {
            UnsubscribeFromAll();
            _context.ChangeState(new GameState(_context));
        }
    }
}
