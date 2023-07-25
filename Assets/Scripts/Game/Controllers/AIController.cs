using ExtremeSnake.Core;
using UnityEngine;

public class AIController : MonoBehaviour, IController
{
    private EventEmitter _emitter;
    public void AssignEmitter(EventEmitter emitter) {
        _emitter = emitter;
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    private void Start() {
        //_emitter.Emit("OnControllerAttached", this);
    }

    [ContextMenu("StartAI")]
    public void Attach() {
        _emitter.Emit("OnControllerAttached",this);
    }
}