using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SnakeDifficulty
{
    public int SnakeSpeed = 4;
    public int SnakeStartingLength = 4;
    public int NumberRepeatedFailsAllowed = 0;
    public int ShrinkTimerLength = 5;
    public int InitialGraceLength = 5;

    public float GetTickTimeFromSnakeSpeed() {
        return 1f / SnakeSpeed;
    }
}
