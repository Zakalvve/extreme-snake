using ExtremeSnake.Game.Snakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Events
{
    public class ReskinEventArgs : EventArgs
    {
        public SnakeSprites Sprites { get; set; }
        public ReskinEventArgs(SnakeSprites sprites) {
            Sprites = sprites;
        }
    }
}
