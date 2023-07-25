using System.Collections.Generic;
using System;
using UnityEngine;

public class ControllerSettings
{
    //A higher order function which attaches the IController to the supplied gameobject
    public Func<GameObject,IController> AttachControllerToGameObject { get; private set; }
    //The players this controller will process input from
    public List<Actor> Pawns = new List<Actor>();

    //Creates the higher order function by attaching the monobehaviour, IController T to the supplied gameobject
    public void CreateAttachAction<T>() where T : MonoBehaviour, IController {
        AttachControllerToGameObject = (prefab) => { return prefab.AddComponent<T>(); };
    }
}
