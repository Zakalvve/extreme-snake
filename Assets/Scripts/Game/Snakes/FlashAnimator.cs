using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class FlashAnimator : MonoBehaviour
{
    LinkedList<SnakeSegment> Segments;
    public bool IsFlashing { get; private set; }
    [SerializeField]
    [Range(0f,5f)]
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
    private Coroutine _existingCoroutine;
    private Queue<Func<Action,IEnumerator>> _coroutineQueue = new Queue<Func<Action,IEnumerator>>();

    private void Start() {
        GameManager.Instance.GameEmitter.Subscribe("OnLoadComplete",HandleSnakeCreated);
    }

    [ContextMenu("Flash")]
    public void MenuFlash() {
        StartFlash();
    }

    [ContextMenu("Flash Force")]
    public void MenuFlashForce() {
        StartFlash(true);
    }

    public void HandleFlash(object sender) {
        if (IsFlashing) StopFlash();
        StartFlash();
    }
    public void HandleSnakeCreated(object sender) {
        Snake s = GetComponent<Snake>();
        Segments = s.Segments;
        s.SnakeEmitter.Subscribe("OnFlash",HandleFlash);

    }

    public void StartFlash(bool force = false) {
        if (_existingCoroutine != null) {
            if (force) {
                _coroutineQueue.Clear();
                StopCoroutine(_existingCoroutine);
            }
            else {
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
                for (var segment = Segments.First; segment != null; segment = segment.Next) {
                    Color lerpedColor = segment.Value.Renderer.color;
                    lerpedColor.a = Mathf.Lerp(StartAlpha,EndAlpha,time);
                    segment.Value.Renderer.color = lerpedColor;
                }
                yield return null;
            }
            _elapsedTime = 0f;
            while (_elapsedTime < duration) {
                _elapsedTime += Time.deltaTime;
                float time = Mathf.Clamp01(_elapsedTime / (duration / 2));
                //lerp to alpha value
                for (var segment = Segments.First; segment != null; segment = segment.Next) {
                    Color lerpedColor = segment.Value.Renderer.color;
                    lerpedColor.a = Mathf.Lerp(EndAlpha,StartAlpha,time);
                    segment.Value.Renderer.color = lerpedColor;
                }
                yield return null;
            }
        }
        _stopFlashing = false;
        for (var segment = Segments.First; segment != null; segment = segment.Next) {
            Color finalColor = segment.Value.Renderer.color;
            finalColor.a = StartAlpha;
            segment.Value.Renderer.color = finalColor;
        }
        OnComplete();
    }
}
