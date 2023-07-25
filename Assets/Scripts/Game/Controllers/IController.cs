using ExtremeSnake.Core;
using UnityEngine;

public interface IController
{
    GameObject GetGameObject();
    void AssignEmitter(EventEmitter emitter);
}