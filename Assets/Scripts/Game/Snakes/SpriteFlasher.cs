using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SpriteFlasher : MonoBehaviour
{
    public bool IsFlashing { get; private set; }
    [SerializeField]
    [Range(0f, 5f)]
    private float TotalDuration = 1f;
    [SerializeField]
    private int Flashes = 3;
    [SerializeField]
    private float StartAlpha = 1.0f;
    [SerializeField]
    private float EndAlpha = 0.0f;

    public float ElapsedTime { get { return _elapsedTime; } }
    private float _elapsedTime = 0f;

    private bool _stopFlashing = false;
    private SpriteRenderer _spriteRenderer;
    private Coroutine _existingCoroutine;
    private Queue<Func<Action, IEnumerator>> _coroutineQueue = new Queue<Func<Action,IEnumerator>>();
    private Action fastStart = null;

    [ContextMenu("Flash")]
    public void MenuFlash() {
        StartFlash(false);
    }

    [ContextMenu("Flash Force")]
    public void MenuFlashForce() {
        StartFlash(true);
    }

    private void Awake() {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        fastStart?.Invoke();
        fastStart = null;
    }

    //pass a value for elapsedTime if you wish to sync a new flash with an existing flash on another game object
    public void StartFlash(bool force = false) {
        if (_spriteRenderer == null) {
            fastStart = () => {
                StartFlash(force);
            };
            return;
        }

        if (_existingCoroutine != null) {
            if (force) {
                _coroutineQueue.Clear();
                StopCoroutine(_existingCoroutine);
            } else {
                _coroutineQueue.Enqueue(Flash);
                return;
            }
        }
        _existingCoroutine = StartCoroutine(Flash(OnCompleteFlash));
        IsFlashing = true;
    }
    [ContextMenu("Stop Flash")]
    public void StopFlash() {
        _coroutineQueue.Clear();
        _stopFlashing = true;
    }
    private void OnCompleteFlash() {
        if (_coroutineQueue.Count > 0) {
            _existingCoroutine = StartCoroutine(_coroutineQueue.Dequeue()(OnCompleteFlash));
        }
        else {
            IsFlashing = false;
            _existingCoroutine = null;
        }
    }
    private IEnumerator Flash(Action OnComplete) {
        float duration = (TotalDuration / Flashes) / 2;
        for (int i = 0; i < Flashes; i++) {
            if (_stopFlashing) break;
            _elapsedTime = 0f;
            while (_elapsedTime < duration) {
                _elapsedTime += Time.deltaTime;
                float time = Mathf.Clamp01(_elapsedTime / duration);
                //lerp to alpha value
                Color lerpedColor = _spriteRenderer.color;
                lerpedColor.a = Mathf.Lerp(StartAlpha,EndAlpha,time);
                _spriteRenderer.color = lerpedColor;
                yield return null;
            }
            _elapsedTime = 0f;
            while (_elapsedTime < duration) {
                _elapsedTime += Time.deltaTime;
                float time = Mathf.Clamp01(_elapsedTime / (duration / 2));
                //lerp to alpha value
                Color lerpedColor = _spriteRenderer.color;
                lerpedColor.a = Mathf.Lerp(EndAlpha,StartAlpha,time);
                _spriteRenderer.color = lerpedColor;
                yield return null;
            }
        }
        _stopFlashing = false;
        Color finalColor = _spriteRenderer.color;
        finalColor.a = StartAlpha;
        _spriteRenderer.color = finalColor;
        OnComplete();
    }
}
