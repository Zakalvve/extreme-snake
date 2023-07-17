using UnityEngine;
using Assets.Scripts.Core;

namespace ExtremeSnake.Game.Food
{
    public class Food : InstanceTracker<Food>
    {
        public int GrowthValue;
        public int PointsValue;

        protected override void Awake() {
            base.Awake();
        }
        void Start() {
            transform.position = GameManager.Instance.Level.CenterInCell(Vector3Int.FloorToInt(transform.position));
        }
    }
}
