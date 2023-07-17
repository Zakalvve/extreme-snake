using ExtremeSnake.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private EventEmitter _emitter;
    public void AssignEmitter(EventEmitter emitter) {
        _emitter = emitter;
    }
}
