using ExtremeSnake.Core;
using UnityEngine;
using UnityEngine.InputSystem;

//the default controller for local multiplayer
public class LocalTwoPlayerController : PlayerController
{
    private InputAction _P2up;
    private InputAction _P2down;
    private InputAction _P2left;
    private InputAction _P2right;

    protected override void Awake() {
        base.Awake();
        PlayerInput.SwitchCurrentActionMap("TwoPlayerLocalLayout");
    }

    protected override void Start() {
        base.Start();
    }
    public override void AssignEmitter(EventEmitter emitter) {
        _emitters.Add(emitter);
        InitializeInput();
    }
    protected override void InitializeInput() {
        base.InitializeInput();

        _P2up = PlayerInput.actions["P2 Up"];
        _P2down = PlayerInput.actions["P2 Down"];
        _P2left = PlayerInput.actions["P2 Left"];
        _P2right = PlayerInput.actions["P2 Right"];
    }
    protected override void Update() {
        base.Update();
        if (_P2up.triggered) {
            MoveUp((int)Player.PLAYER_2);
        }
        if (_P2down.triggered) {
            MoveDown((int)Player.PLAYER_2);
        }
        if (_P2left.triggered) {
            MoveLeft((int)Player.PLAYER_2);
        }
        if (_P2right.triggered) {
            MoveRight((int)Player.PLAYER_2);
        }
    }

}
