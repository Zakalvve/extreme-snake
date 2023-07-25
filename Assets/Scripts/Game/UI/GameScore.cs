using ExtremeSnake.Game.Snakes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ExtremeSnake.Game.UI
{
    public class GameScore : MonoBehaviour
    {
        public Queue<ScoreComponent> OpenSlots = new Queue<ScoreComponent>();
        public void Start() {
            foreach (var component in GetComponentsInChildren<ScoreComponent>().ToList()) {
                OpenSlots.Enqueue(component);
                component.gameObject.SetActive(false);
            }
            GameManager.Instance.GameEmitter.Subscribe<SnakeCreatedEventArgs>("SnakeCreated",HandleSnakeCreated);
            GameManager.Instance.GameEmitter.Subscribe("OnPostGame",(object sender) => {
                gameObject.SetActive(false);
            });
        }

        public void HandleSnakeCreated(object sender, SnakeCreatedEventArgs args) {
            if (OpenSlots.Count > 0) {
                OpenSlots.Dequeue().InitializeComponent(args);
            }
        }
    }
}
