using System;

namespace ExtremeSnake.Game.Food
{
    public class EatEventArgs : EventArgs
    {
        public Food FoodEaten { get; set; }

        public EatEventArgs(Food food) {
            FoodEaten = food;
        }
    }
}
