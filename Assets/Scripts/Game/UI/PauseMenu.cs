using ExtremeSnake.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void MainMenu() {
        GameManager.Instance.GameEmitter.Emit("EndSession",this);
    }
    public void Resume() {
        GameManager.Instance.GameEmitter.Emit("TryResume",this);
    }
    public void Quit() {
        GameManager.Instance.GameEmitter.Emit("QuitGame",this);
    }
}
