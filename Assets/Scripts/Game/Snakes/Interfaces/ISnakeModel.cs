using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public interface ISnakeModel
    {
        Vector2 HeadPosition { get; }
        Vector2 MoveDirection { get; }

        bool ChangeLength(int amount, bool releaseSegment = false);
        void Draw();
        void Move();
        Vector2 UpdateDirection(Vector2 newDirection);
        void AddGrowth(int amount);
        void Clear();
    }
}