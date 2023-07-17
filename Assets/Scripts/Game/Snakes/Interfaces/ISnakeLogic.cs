using ExtremeSnake.Game.Food;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public interface ISnakeLogic
    {
        Vector2 NewDirection { get; set; }
        void OnChangeDirection(Vector2 inputDirection);
        void HandleEat(object sender, EatEventArgs args);
        void OnMove();
    }
}