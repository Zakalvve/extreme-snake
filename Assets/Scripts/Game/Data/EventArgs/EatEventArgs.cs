using System;

namespace ExtremeSnake.Game.Levels
{
    public class EatEventArgs : EventArgs
    {
        public Food FoodEaten { get; set; }

        public EatEventArgs(Food food) {
            FoodEaten = food;
        }
    }
}
