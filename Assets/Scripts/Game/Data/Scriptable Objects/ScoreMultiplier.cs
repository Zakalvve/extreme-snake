using System;
using UnityEngine;
using ExtremeSnake.Utils;

namespace Assets.Scripts.Game.Score
{
    [CreateAssetMenu(menuName = "My Assets/MultiplierProgression")]
    public class ScoreMultiplier : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve MultiplierProgression;
        public int GetMultiplier(int streak) {
            float result = Math.Abs((int)MultiplierProgression.Evaluate(streak));
            int clamp = UtilsClass.FloorPower2((int)result);
            return clamp == 0 ? 1 : clamp;
        }
    }
}
