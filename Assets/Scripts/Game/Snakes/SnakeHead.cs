using ExtremeSnake.Game.Levels;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public class SnakeHead : MonoBehaviour
    {
        private Snake _snake;
        void Start() {
            _snake = gameObject.transform.parent.gameObject.GetComponent<Snake>();
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Food" && collision.gameObject.GetComponent<SpriteRenderer>().sortingLayerName == gameObject.GetComponent<SpriteRenderer>().sortingLayerName) {
                int value = -1;
                Food food = collision.gameObject.GetComponent<Food>();
                value = collision.gameObject.GetComponent<Food>().GrowthValue;
                if (value > 0) {
                    food.Eaten();
                    _snake.SnakeEmitter.Emit("OnEat",this,new EatEventArgs(food));
                }
            }
        }
    }
}
