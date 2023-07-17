using ExtremeSnake.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteEvent : EventArgs
{
    public int number { get; set; }
    public string word { get; set; }
    public ConcreteEvent(int i, string str) {
        number = i;
        word = str;
    }
}
public class EmitterTest : MonoBehaviour
{
    public static EventEmitter TestEmitter = new EventEmitter();

    void Start()
    {
        TestEmitter.Subscribe<ConcreteEvent>("OnTwenty", TestCallback);
        TestEmitter.Subscribe<ConcreteEvent>("OnTwenty",TimesTwo);
        TestEmitter.Subscribe("OnNothing", Nothing);
    }

    void TestCallback(object sender, ConcreteEvent args) {
        Debug.Log($"{((EmitterInvoker)sender).testString} {args.word}");
    }

    void TimesTwo(object sender, ConcreteEvent args) {
        Debug.Log($"{sender.ToString()} {args.number*2}");
    }

    void Nothing(object sender) {
        Debug.Log($"Nothing happenbed! Hooray!");
    }
}
