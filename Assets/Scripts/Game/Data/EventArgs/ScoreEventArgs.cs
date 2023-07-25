using ExtremeSnake.Game.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtremeSnake.Game.Data
{
    public class ScoreEventArgs : EventArgs
    {
        public string SnakeUUID { get; }
        public int PointsTotal { get; }
        public int PointsChange { get; }
        public bool MultiplierChanged { get; }
        public Color TextColor { get; }
        public SnakeScore Score { get; set; } = null;
        public MultiplierGradients MultiplierColors { get; set; }

        public ScoreEventArgs(string snakeUUID, int pointsTotal, int pointsGained, bool multiplierChanged, Color textColor) {
            SnakeUUID = snakeUUID;
            PointsTotal = pointsTotal;
            PointsChange = pointsGained;
            MultiplierChanged = multiplierChanged;
            TextColor = textColor;
        }
        public ScoreEventArgs(string snakeUUID, int pointsGained) : this(snakeUUID,0, pointsGained, false, Color.white) { }
    }
}
