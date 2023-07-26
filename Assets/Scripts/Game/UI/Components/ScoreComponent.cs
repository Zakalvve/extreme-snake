using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExtremeSnake.Game.UI
{
    public class ScoreComponent : MonoBehaviour
    {
        public Text Name;
        public TextMeshProUGUI Score;
        public TextMeshProUGUI Multiplier;

        public void InitializeComponent(SnakeCreatedEventArgs args) {
            Name.text = args.Name;
             
            Score.text = string.Format("{0:N0}",args.Score.Score);

            if (args.Score.Multiplier < 2) Multiplier.enabled = false;
            else {
                Multiplier.enabled = true;
                Multiplier.colorGradientPreset = GameManager.Instance.Settings.DefaultMultiplierGradients.GetGradient(args.Score.Multiplier);
                Multiplier.text = $"x{args.Score.Multiplier}";
            }

            gameObject.SetActive(true);
            GameManager.Instance.GameEmitter.Subscribe<ScoreEventArgs>($"{args.Data.UUID}OnScoreChanged",HandleScoreChanged);
        }

        public void HandleScoreChanged(object sender, ScoreEventArgs args) {
            Score.text = string.Format("{0:N0}",args.Score.Score);

            if (args.Score.Multiplier < 2) Multiplier.enabled = false;
            else {
                Multiplier.enabled = true;
                Multiplier.colorGradientPreset = args.MultiplierColors.GetGradient(args.Score.Multiplier);
                Multiplier.text = $"x{args.Score.Multiplier}";
            }
        }
    }
}