using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeSnake.Game.Data
{
    public class ScoreEventArgs : EventArgs
    {
        public string SnakeUUID { get; }
        public int Points { get; }

        public SnakeScore Score { get; set; }

        public ScoreEventArgs(string snakeUUID, int points) {
            SnakeUUID = snakeUUID;
            Points = points;
        }
        public ScoreEventArgs(string snakeUUID) : this(snakeUUID,0) { }
    }
}
