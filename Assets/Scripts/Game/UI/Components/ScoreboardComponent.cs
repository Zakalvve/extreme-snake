using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreboardComponent : MonoBehaviour
{
    public Text Name;
    public TextMeshProUGUI Score;

    public void Display(string name, int score) {
        Name.text = name;
        Score.text = string.Format("{0:N0}",score);
    }
}
