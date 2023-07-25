using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Levels
{
    [CreateAssetMenu(menuName = "My Assets/Food Spawn Data")]
    public class FoodSpawnData : ScriptableObject
    {
        public GameObject FoodPrefab;
        public float SpawnChance;
    }
}