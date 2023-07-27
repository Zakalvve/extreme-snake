using ExtremeSnake.Core;
using ExtremeSnake.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIRoot : MonoBehaviour, IPausable
{
    public GameObject PauseMenu;
    public GameObject LiveScore;
    public GameObject Scoreboard;

    public bool IsPaused { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.GameEmitter.Subscribe("GamePaused",HandlePause);
        GameManager.Instance.GameEmitter.Subscribe("ResumeGame",HandleResume);
    }

    public void HandlePause(object sender) {
        Pause();
    }

    public void Pause() {
        IsPaused = true;
        PauseMenu.SetActive(IsPaused);
        LiveScore.SetActive(!IsPaused);
    }

    public void HandleResume(object sender) {
        Resume();
    }

    public void Resume() {
        IsPaused = false;
        PauseMenu.SetActive(IsPaused);
        LiveScore.SetActive(!IsPaused);
    }
}
