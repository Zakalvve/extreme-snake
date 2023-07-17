using ExtremeSnake.Game.Food;
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
            if (collision.tag == "Food") {
                int value = -1;
                Food.Food food = collision.gameObject.GetComponent<Food.Food>();
                value = collision.gameObject.GetComponent<Food.Food>().GrowthValue;
                if (value > 0) {
                    GameObject.Destroy(collision.gameObject);
                    _snake.SnakeEmitter.Emit("OnEat",this,new EatEventArgs(food));
                }
            }
        }
    }
}
