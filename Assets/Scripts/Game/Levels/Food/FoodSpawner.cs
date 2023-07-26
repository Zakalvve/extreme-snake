using Assets.Scripts.Core.Instance_Control_Patterns;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace ExtremeSnake.Game.Levels
{
    public class FoodSpawner
    {
        [SerializeField]
        private LevelFood _levelFood;
        private Dictionary<string,InstancePooler<GameObject>> FoodPool = new Dictionary<string, InstancePooler<GameObject>>();

        public FoodSpawner(LevelFood levelFood) {
            _levelFood = levelFood;
        }
        public void Spawn(string layer, Vector3 at) {
            FoodSpawnData spawn;
            try {
                spawn = ChooseRandomFood();
            } catch (System.ArithmeticException e) {
                Debug.LogError(e);
                spawn = _levelFood.LevelFoods.FirstOrDefault();
            }

            if (!FoodPool.ContainsKey(spawn.name)) {
                FoodPool.Add(spawn.name, new InstancePooler<GameObject>(() => GameObject.Instantiate(spawn.FoodPrefab)));
            }

            GameObject foodGO = FoodPool[spawn.name].Get();
            foodGO.SetActive(true);
            foodGO.transform.position = at;
            foodGO.layer = LayerMask.NameToLayer(layer);
            foodGO.GetComponent<SpriteRenderer>().sortingLayerName = layer;
            foodGO.GetComponent<Food>().OnEaten = (GameObject go) => FoodPool[spawn.name].Return(go);
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