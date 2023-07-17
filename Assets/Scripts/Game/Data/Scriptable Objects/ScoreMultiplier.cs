using System;
using UnityEngine;

namespace Assets.Scripts.Game.Score
{
    [CreateAssetMenu(menuName = "My Assets/MultiplierProgression")]
    public class ScoreMultiplier : ScriptableObject
    {
        [SerializeField]
        private AnimationCurve MultiplierProgression;
        public int GetMultiplier(int streak) {
            float result = Math.Abs((int)MultiplierProgression.Evaluate(streak));
            int clamp = FloorPower2((int)result);
            return clamp == 0 ? 1 : clamp;
        }

        private static int FloorPower2(int x) {
            if (x < 1) {
                return 1;
            }
            return (int)Math.Pow(2,(int)Math.Log(x,2));
        }
    }
}
