using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeCreatedEventArgs : EventArgs
    {
        public ISnakeData Data { get; }
        public Actor Settings { get; }
        public Transform SnakeHead { get; }
        public SnakeSprites Skin { get; }
        public string Name { get; }
        public SnakeScore Score { get; }

        public SnakeCreatedEventArgs(ISnakeData data,Actor settings,SnakeScore score) {
            Data = data;
            Settings = settings;
            SnakeHead = data.Segments.First.Value.Segment.transform;
            Skin = settings.Skin;
            Name = settings.Name;
            Score = score;
        }
    }
}
