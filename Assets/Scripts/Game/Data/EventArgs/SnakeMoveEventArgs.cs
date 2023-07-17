using ExtremeSnake.Game.Snakes;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Data
{
    public  class SnakeMoveEventArgs : EventArgs
    {
        public LevelPosition Claimed;
        public LevelPosition Released;
        public SnakeMoveEventArgs(LevelPosition claimed,LevelPosition released) {
            Claimed = claimed;
            Released = released;
        }
    }
}
