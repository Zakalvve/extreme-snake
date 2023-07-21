using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public interface ISnakeModel
    {
        Vector2 HeadPosition { get; }
        Vector2 MoveDirection { get; }

        void ChangeLength(int amount);
        void Draw();
        void Move();
        Vector2 UpdateDirection(Vector2 newDirection);
        public void AddGrowth(int amount);
    }
}