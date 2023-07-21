using ExtremeSnake.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    //public float SmoothTime = 2;

    //private Transform _focus;
    //private float _z;
    //private Vector3 offset;
    //private bool isActive = false;

    //public void Awake() {
    //    GameManager.Instance.GameEmitter.Subscribe<CameraEventArgs>("OnPlayerSnakeCreated",Initialize);
    //}

    ////An event handler which initialises the controller
    //public void Initialize(object sender,CameraEventArgs args) {
    //    _focus = args.Focus;
    //    transform.position = new Vector3(_focus.position.x, _focus.position.y, transform.position.z);
    //    offset = _focus.position - transform.position;
    //    isActive = true;
    //}

    ////smooth follow the focus
    //void Update() {
    //    if (isActive)
    //        transform.position = Vector3.Lerp(transform.position,_focus.position - offset,Time.deltaTime * SmoothTime);
    //}
}