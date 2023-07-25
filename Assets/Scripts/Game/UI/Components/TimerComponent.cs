using ExtremeSnake.Game;
using ExtremeSnake.Utils;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking.Match;

public class TimerComponent : MonoBehaviour
{
    private int _timeRemainingInSeconds;
    public TextMeshProUGUI TimeLeft;

    public void Start() {
        _timeRemainingInSeconds = GameManager.Instance.Settings.ActiveSession.Duration;
        var (mins, seconds) = UtilsClass.SecondsToMinutesAndSeconds(_timeRemainingInSeconds);

        TimeLeft.text = UtilsClass.FormatTime(mins,seconds);

        GameManager.Instance.GameEmitter.Subscribe("OnSecondTick",HandleSecondTick);
        GameManager.Instance.GameEmitter.Subscribe("OnPostGame",(object sender) => {
            gameObject.SetActive(false);
        });
    }

    public void HandleSecondTick(object sender) {
        _timeRemainingInSeconds--;
        var (mins, seconds) = UtilsClass.SecondsToMinutesAndSeconds(_timeRemainingInSeconds);
        TimeLeft.text = UtilsClass.FormatTime(mins,seconds);
    }
}
