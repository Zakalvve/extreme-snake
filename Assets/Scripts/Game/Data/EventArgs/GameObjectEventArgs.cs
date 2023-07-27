using System;
using UnityEngine;

namespace ExtremeSnake.Game.Data
{
    public class GameObjectEventArgs : EventArgs
    {
        public GameObject GO;
        public GameObjectEventArgs(GameObject go) {
            GO = go;
        }
    }
}
