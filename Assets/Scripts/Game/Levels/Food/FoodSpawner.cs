using System.Linq;
using UnityEngine;

namespace ExtremeSnake.Game.Levels
{
    public class FoodSpawner : MonoBehaviour
    {
        [SerializeField]
        private LevelFood _levelFood;

        public FoodSpawner(LevelFood levelFood) {
            _levelFood = levelFood;
        }
        public void Spawn(string layer, Vector2 at) {
            FoodSpawnData spawn;
            try {
                spawn = ChooseRandomFood();
            } catch (System.ArithmeticException e) {
                Debug.LogError(e);
                spawn = _levelFood.LevelFoods.FirstOrDefault();
            }
            GameObject foodGO = Instantiate(spawn.FoodPrefab);
            foodGO.transform.position = at;
            foodGO.layer = LayerMask.NameToLayer(layer);
            foodGO.GetComponent<SpriteRenderer>().sortingLayerName = layer;
        }

        private FoodSpawnData ChooseRandomFood() {
            int randomResult = Random.Range(1,100);
            float accumulator = 0f;
            foreach(var food in _levelFood.LevelFoods) {
                accumulator += food.SpawnChance;
                if (randomResult < accumulator * 100) {
                    return food;
                }
            }
            throw new System.ArithmeticException("Combined spawn rate of level food does not equal 100%");
        }
    }
}