using ExtremeSnake.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmitterInvoker : MonoBehaviour
{
    //string to test that recieved has access to sender public data
    public string testString = "NO U";

    // Update is called once per frame
    void Update()
    {
        EmitterTest.TestEmitter.Emit("OnTwenty", this,  new ConcreteEvent(20, "Twenty"));
        EmitterTest.TestEmitter.Emit("OnNothing", this);
    }
}
