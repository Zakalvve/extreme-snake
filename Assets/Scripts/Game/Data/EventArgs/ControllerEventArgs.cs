using System;
using UnityEngine;

namespace Assets.Scripts.Game.Controllers
{
    public class ControllerEventArgs : EventArgs
    {
        public Vector2 Direction { get; set; }
        public ControllerEventArgs(Vector2 dir) {
            Direction = dir;
        }
    }
}
