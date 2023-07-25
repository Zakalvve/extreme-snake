using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExtremeSnake.Game.Data
{
    public class StringEventArgs : EventArgs
    {
        public string Text { get; set; }
        public StringEventArgs(string text) {
            Text = text;
        }
    }
}
