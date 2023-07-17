using UnityEngine;
using System;
public class CameraEventArgs : EventArgs
{
    public Transform Focus { get; set; }
    public CameraEventArgs(Transform focus) {
        Focus = focus;
    }
}