using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Game.Data
{
    public class CinematicPipelineEventArgs : EventArgs
    {
        public Camera Cinecam;
        public Func<CinematicArgs,Func<IEnumerator>,IEnumerator> Pan;
        public Func<CinematicArgs,Func<IEnumerator>,IEnumerator> Display;
    }
}
