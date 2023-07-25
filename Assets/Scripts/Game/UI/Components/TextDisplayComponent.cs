using Assets.Scripts.Game.Data;
using ExtremeSnake.Game;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDisplayComponent : MonoBehaviour
{
    public TextMeshProUGUI Display;
    // Start is called before the first frame update
    void Start()
    {
        Display = GetComponent<TextMeshProUGUI>();
        GameManager.Instance.GameEmitter.Subscribe<CinematicPipelineEventArgs>("CinematicCreate",HandleCreateCinematic);
        GameManager.Instance.GameEmitter.Subscribe<CinematicPipelineEventArgs>("CinematicCreate",HandleCreateCinematic);
    }

    public void HandleCreateCinematic(object sender,CinematicPipelineEventArgs args) {
        args.Display = DisplayProcess;
    }

    public IEnumerator DisplayProcess(CinematicArgs args,Func<IEnumerator> next) {
        Display.enabled = true;
        Display.text = args.Name;
        yield return next();
        Display.enabled = false;
    }
}
