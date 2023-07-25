using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Levels
{
    [CreateAssetMenu(menuName = "My Assets/Level Food")]
    public class LevelFood : ScriptableObject
    {
        public List<FoodSpawnData> LevelFoods;
    }
}
