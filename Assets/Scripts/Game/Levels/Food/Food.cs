using UnityEngine;
using Assets.Scripts.Core;

namespace ExtremeSnake.Game.Levels
{
    public class Food : InstanceTracker<Food>
    {
        public int GrowthValue;
        public int PointsValue;

        protected override void Awake() {
            base.Awake();
        }
        void Start() {
            if (GameManager.Instance.Level == null) {
                GameManager.Instance.GameEmitter.Subscribe("OnLevelStartComplete",(object sender) => {
                    transform.position = GameManager.Instance.Level.CenterInCell(Vector3Int.FloorToInt(transform.position));
                });
            } else {
                transform.position = GameManager.Instance.Level.CenterInCell(Vector3Int.FloorToInt(transform.position));
            }
        }
    }
}
