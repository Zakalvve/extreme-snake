using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeViewData : ISnakeViewData
    {
        public SnakeViewData(GameObject head,GameObject body,SnakeSprites sprites) {
            HeadPrefab = head;
            BodyPrefab = body;
            Sprites = sprites;
        }
        public GameObject HeadPrefab { get; set; }
        public GameObject BodyPrefab { get; set; }
        public SnakeSprites Sprites { get; set; }
    }
}
