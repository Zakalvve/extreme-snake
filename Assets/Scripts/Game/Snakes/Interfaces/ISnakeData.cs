﻿using ExtremeSnake.Core;
using System.Collections.Generic;
using UnityEngine;

namespace ExtremeSnake.Game.Snakes
{
    public interface ISnakeData
    {
        SnakeViewData ViewData { get; set; }
        int StartingLength { get; }
        LinkedList<SnakeSegment> Segments { get; set; }
        Vector2 MoveDirection { get; set; }
        Transform SnakeTransform { get; }
        EventEmitter SnakeEmitter { get; }
        List<LevelPosition> ExtractSegmentPositions();
    }
}
