using Assets.Scripts.Game.Controllers;
using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeDirectionalArrow : MonoBehaviour
{
    public GameObject Aid;
    private SpriteRenderer _spriteRenderer;
    public Sprite Up;
    public Sprite Down;
    public Sprite Left;
    public Sprite Right;
    public EventEmitter Emitter;
    public Vector2 UpDirection = Vector2.up;

    
    // Start is called before the first frame update
    void Start()
    {
        Aid.layer = LayerMask.NameToLayer("Layer 3");
        _spriteRenderer = Aid.GetComponent<SpriteRenderer>();
        _spriteRenderer.sortingLayerName = LayerMask.LayerToName(Aid.layer);
        _spriteRenderer.sortingOrder = 10;
        Snake s = GetComponentInParent<Snake>();
        Emitter = s.SnakeEmitter;
        Emitter.Subscribe<ControllerEventArgs>("FinalSnakeDirection",OnChangeDirection);
        Emitter.Subscribe("OnFlash",Warning);
        ChangeDirection(s.MoveDirection);
        _spriteRenderer.color = Color.red;
    }

    public void OnChangeDirection(object sender, ControllerEventArgs args) {
        ChangeDirection(args.Direction);
    }

    private void ChangeDirection(Vector2 dir) {
        _spriteRenderer.enabled = false;

        if (dir == Vector2.up) {
            Aid.transform.position = transform.position + Vector3.up;
            _spriteRenderer.sprite = Up;
        }
        else if (dir == Vector2.down) {
            Aid.transform.position = transform.position + Vector3.down;
            _spriteRenderer.sprite = Down;
        }
        else if (dir == Vector2.left) {
            Aid.transform.position = transform.position + Vector3.left;
            _spriteRenderer.sprite = Left;
        }
        else if (dir == Vector2.right) {
            Aid.transform.position = transform.position + Vector3.right;
            _spriteRenderer.sprite = Right;
        }
    }

    public void Warning(object sender) {
        _spriteRenderer.enabled = true;
    }

    [ContextMenu("Up")]
    public void GoUp() {
        Aid.transform.position = transform.position + Vector3.up;
        Aid.GetComponent<SpriteRenderer>().sprite = Up;
    }

    [ContextMenu("Down")]
    public void GoDown() {
        Aid.transform.position = transform.position + Vector3.down;
        Aid.GetComponent<SpriteRenderer>().sprite = Down;
    }

    [ContextMenu("Left")]
    public void GoLeft() {
        Aid.transform.position = transform.position + Vector3.left;
        Aid.GetComponent<SpriteRenderer>().sprite = Left;
    }

    [ContextMenu("Right")]
    public void GoRight() {
        Aid.transform.position = transform.position + Vector3.right;
        Aid.GetComponent<SpriteRenderer>().sprite = Right;
    }
}
