using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeSnake.Game.UI
{
    public class AddPlayerEventArgs : EventArgs
    {
        public ParticipantType playerType;
        public AddPlayerEventArgs(ParticipantType t) {
            playerType = t;
        }
    }
}
