using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using System.Collections.Generic;
using UnityEngine;
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
        //only initialize input when we have somewhere for it to go
        InitializeInput();
    }

    protected virtual void InitializeInput() {
        _up = PlayerInput.actions["Up"];
        _down = PlayerInput.actions["Down"];
        _left = PlayerInput.actions["Left"];
        _right = PlayerInput.actions["Right"];
        _pause = PlayerInput.actions["Pause"];

        _up.performed += context => MoveUp(context, (int)Player.PLAYER_1);
        _down.performed += context => MoveDown(context, (int)Player.PLAYER_1);
        _left.performed += context => MoveLeft(context, (int)Player.PLAYER_1);
        _right.performed += context => MoveRight(context ,(int)Player.PLAYER_1);
        _pause.performed += PauseGame;
    }

    protected void MoveUp(InputAction.CallbackContext context, int playerIndex) {
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.up));
    }
    protected void MoveDown(InputAction.CallbackContext context,int playerIndex) {
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.down));
    }

    protected void MoveLeft(InputAction.CallbackContext context,int playerIndex) {
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.left));
    }
    protected void MoveRight(InputAction.CallbackContext context,int playerIndex) {
        if (_emitters.Count < 2 && playerIndex == (int)Player.PLAYER_2) {
            playerIndex = (int)Player.PLAYER_1;
            Debug.LogWarning("Two player controller has only one emitter assigned");
        }
        _emitters[playerIndex].Emit("OnChangeDirection",this,new ControllerEventArgs(Vector2.right));
    }
    private void PauseGame(InputAction.CallbackContext context) {
        GameManager.Instance.GameEmitter.Emit("OnPause",this);
    }

    public enum Player
    {
        PLAYER_1 = 0,
        PLAYER_2 = 1
    }
}
