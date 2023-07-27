using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;

public class TimerComponent : MonoBehaviour, IPausable
{
    private int _timeRemainingInSeconds;
    public TextMeshProUGUI TimeLeft;

    public bool IsPaused {get; private set;} = false;

    public void Start() {
        _timeRemainingInSeconds = GameManager.Instance.Settings.ActiveSession.Duration;
        var (mins, seconds) = UtilsClass.SecondsToMinutesAndSeconds(_timeRemainingInSeconds);

        TimeLeft.text = UtilsClass.FormatTime(mins,seconds);

        GameManager.Instance.GameEmitter.Subscribe("GamePaused",HandlePause);
        GameManager.Instance.GameEmitter.Subscribe("ResumeGame",HandleResume);
        GameManager.Instance.GameEmitter.Subscribe("OnSecondTick",HandleSecondTick);
        GameManager.Instance.GameEmitter.Subscribe("OnPostGame",(object sender) => {
            gameObject.SetActive(false);
        });
    }

    public void HandleSecondTick(object sender) {
        if (IsPaused) return;
        _timeRemainingInSeconds--;
        var (mins, seconds) = UtilsClass.SecondsToMinutesAndSeconds(_timeRemainingInSeconds);
        TimeLeft.text = UtilsClass.FormatTime(mins,seconds);
    }

    public void HandlePause(object sender) {
        Pause();
    }

    public void Pause() {
        IsPaused = true;
        gameObject.SetActive(false);
    }

    public void HandleResume(object sender) {
        Resume();
    }

    public void Resume() {
        IsPaused = false;
        gameObject.SetActive(true);
    }
}
