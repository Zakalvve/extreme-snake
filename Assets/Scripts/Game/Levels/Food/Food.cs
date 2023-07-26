using UnityEngine;
using Assets.Scripts.Core;
using System;

namespace ExtremeSnake.Game.Levels
{
    public class Food : InstanceTracker<Food>
    {
        public int GrowthValue;
        public int PointsValue;
        public Action<GameObject> OnEaten;

        protected override void Awake() {
            base.Awake();
        }
        public void Eaten() {
            gameObject.SetActive(false);
            OnEaten?.Invoke(gameObject);
        }
    }
}
