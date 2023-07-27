using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

//The default controller for a single player
[RequireComponent(typeof(PlayerInput))]
public class PlayerController : MonoBehaviour, IController
{
    protected List<EventEmitter> _emitters = new List<EventEmitter>();

    public PlayerInput PlayerInput;

    protected InputAction _up;
    private InputAction _down;
    private InputAction _left;
    private InputAction _right;
    private InputAction _pause;
    private InputAction _enter;
    private InputAction _escape;

    protected bool _initialized = false;

    protected virtual void Awake() {
        PlayerInput = GetComponent<PlayerInput>();
    }

    protected virtual void Start() {
        _emitters.ForEach(emitter => {
            emitter.Emit("OnControllerAttached",this);
        });
    }

    //not very safe so worth thinking about changing anything can assign a emitter to this controller....
    public virtual void AssignEmitter(EventEmitter emitter) {
        _emitters.Add(emitter);
        InitializeInput();
    }

    //ensure action events are unsubscribed from if this object is destroyed
    protected virtual void OnDestroy() {
        _pause.performed -= PauseGame;
        _enter.performed -= Enter;
        _escape.performed -= Escape;
    }

    protected virtual void InitializeInput() {
        _up = PlayerInput.actions["Up"];
        _down = PlayerInput.actions["Down"];
        _left = PlayerInput.actions["Left"];
        _right = PlayerInput.actions["Right"];

        _pause = PlayerInput.actions["Pause"];
        _enter = PlayerInput.actions["Enter"];
        _escape = PlayerInput.actions["Escape"];

        _pause.performed += PauseGame;
        _enter.performed += Enter;
        _escape.performed += Escape;
    }

    protected virtual void Update() {
        if (_up.triggered) {
            MoveUp((int)Player.PLAYER_1);
        }
        if (_down.triggered) {
            MoveDown((int)Player.PLAYER_1);
        }
        if (_left.triggered) {
            MoveLeft((int)Player.PLAYER_1);
        }
        if (_right.triggered) {
            MoveRight((int)Player.PLAYER_1);
        }
    }

    protected void MoveUp(int playerIndex) {
        if (_emitters[playerIndex] == null) return;
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.up));
    }
    protected void MoveDown(int playerIndex) {
        if (_emitters[playerIndex] == null) return;
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.down));
    }

    protected void MoveLeft(int playerIndex) {
        if (_emitters[playerIndex] == null) return;
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.left));
    }

    protected void MoveRight(int playerIndex) {
        if (_emitters[playerIndex] == null) return;
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.right));
    }

    private void PauseGame(InputAction.CallbackContext context) {
        GameManager.Instance.GameEmitter.Emit("TryPause",this);
    }

    private void Enter(InputAction.CallbackContext context) {
        GameManager.Instance.GameEmitter.Emit("OnEnter",this);
    }
    private void Escape(InputAction.CallbackContext context) {
        GameManager.Instance.GameEmitter.Emit("OnEscape",this);
    }

    public GameObject GetGameObject() {
        return gameObject;
    }

    public enum Player
    {
        PLAYER_1 = 0,
        PLAYER_2 = 1
    }
}
