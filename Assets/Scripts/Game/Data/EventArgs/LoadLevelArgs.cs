using Assets.Scripts.Game.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Game.Events
{
    public class LoadLevelArgs: EventArgs
    {
        public LoadLevelArgs(LevelModel model) {
            LevelToLoad = model;
        }
        public LevelModel LevelToLoad;
    }
}
