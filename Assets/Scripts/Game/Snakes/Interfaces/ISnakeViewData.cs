using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public interface ISnakeViewData
    {
        GameObject BodyPrefab { get; set; }
        GameObject HeadPrefab { get; set; }
        SnakeSprites Sprites { get; set; }
    }
}