using UnityEngine.SceneManagement;
using UnityEngine;
using ExtremeSnake.Game;

public class MainMenu : MonoBehaviour
{
    public void PlayGame() {
        GameManager.Instance.GameEmitter.Emit("OnLoadGame",this);
    }

    public void QuitGame() {
        Debug.Log("Game Quit");
        Application.Quit();
    }
}
