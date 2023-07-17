using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "My Assets/ Difficulty")]
public class Difficulty : ScriptableObject
{
    public string Name = "Default";
    [SerializeField]
    public SnakeDifficulty SnakeDifficulty;
    [SerializeField]
    public FoodDifficulty FoodScarcity;
}
