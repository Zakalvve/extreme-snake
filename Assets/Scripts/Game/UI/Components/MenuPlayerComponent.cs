using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerController;

public class MenuPlayerComponent : MonoBehaviour
{
    public GameObject AddPlayerButton;
    public GameObject Controls;

    public GameObject ReadyIcon;
    public GameObject NotReadyIcon;

    public int SelectedSkin = 0;
    public List<SnakeSprites> SkinOptions;

    public Image Head;
    public Image Body;
    public Image Tail;

    public TMP_InputField NameTextBox;

    public Text PlayerTypeText;

    private bool _isReady = false;
    public bool IsPlayer = false;
    
    public bool IsReady { get { return _isReady; } }

    private ParticipantType PlayerType;

    public bool IsPlayerActive { get; set; }
    public string Name { get { return NameTextBox.text; } }

    private void Awake() {
        UpdateSnakeAppearance();
        SetReadyIcons();
    }
    public void EnableDisplay() {
        AddPlayerButton.SetActive(false);
        Controls.SetActive(true);
        IsPlayerActive = true;
    }

    public bool IsTwoPlayer { get { return PlayerType == ParticipantType.PLAYER_2 ? true : false; } }

    public void Initialize(Actor actor) {
        Initialize(actor.ActorType,actor.Name);
        UpdateSnakeAppearance(actor.Skin);
    }

    public void Initialize(ParticipantType type = ParticipantType.COMPUTER, string startingName = "") {
        PlayerType = type;
        IsPlayer = type != ParticipantType.COMPUTER;
        NameTextBox.text = startingName;
        AddPlayerButton.SetActive(!IsPlayer);
        Controls.SetActive(IsPlayer);
        IsPlayerActive = IsPlayer;

        switch (type) {
            case ParticipantType.PLAYER_1:
                PlayerTypeText.text = "P1";
                break;
            case ParticipantType.PLAYER_2:
                PlayerTypeText.text = "P2";
                break;
            case ParticipantType.COMPUTER:
                PlayerTypeText.text = "COM";
                _isReady = true;
                SetReadyIcons();
                break;
            default: 
                PlayerTypeText.text = "COM";
                _isReady = true;
                SetReadyIcons();
                break;
        }
        SetReadyIcons();
    }

    public Actor GetOutput() {
        Actor output = new Actor(SkinOptions[SelectedSkin],Name,PlayerType);
        if (PlayerType == ParticipantType.PLAYER_1) {
            GameManager.Instance.Settings.Player1 = output;
        } else if (PlayerType == ParticipantType.PLAYER_2) {
            GameManager.Instance.Settings.Player2 = output;
        }
        return output;
    }

    public void OnNextSkin() {
        SelectedSkin++;
        if (SelectedSkin >= SkinOptions.Count) SelectedSkin = 0;
        UpdateSnakeAppearance();
    }

    public void OnPreviousSkin() {
        SelectedSkin--;
        if (SelectedSkin < 0) SelectedSkin = SkinOptions.Count - 1;
        UpdateSnakeAppearance();
    }

    public void NotReadyAnimation() {
        StartCoroutine(RiseAndFall(NotReadyIcon));
    }

    public void UpdateSnakeAppearance(SnakeSprites skin) {
        Head.sprite = skin.HeadRight;
        Body.sprite = skin.Horizontal;
        Tail.sprite = skin.TailLeft;
    }
    public void UpdateSnakeAppearance() {
        Head.sprite = SkinOptions[SelectedSkin].HeadRight;
        Body.sprite = SkinOptions[SelectedSkin].Horizontal;
        Tail.sprite = SkinOptions[SelectedSkin].TailLeft;
    }

    public void SetReadyIcons() {
        ReadyIcon.SetActive(_isReady);
        NotReadyIcon.SetActive(!_isReady);
    }
    public void OnReady() {
        if (PlayerType == ParticipantType.COMPUTER) {
            GameManager.Instance.GameEmitter.Emit("RemovePlayerComponent",this);
            return;
        }
        _isReady = !_isReady;
        SetReadyIcons();
    }

    public void AddPlayer() {
        GameManager.Instance.GameEmitter.Emit("OnPlayerComponentAdded",this, new AddPlayerEventArgs(ParticipantType.COMPUTER));
    }

    private IEnumerator RiseAndFall(GameObject go) {
        float duration = 0.5f / 2;
        float elapsed = 0f;
        Vector3 initialScale = Vector3.one;
        Vector3 targetScale = new Vector3(1.6f, 1.6f, 1);

        //rise
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            go.transform.localScale = Vector3.Lerp(initialScale,targetScale,t);
            yield return null;
        }
        elapsed = 0f;
        //and fall
        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            go.transform.localScale = Vector3.Lerp(targetScale,initialScale,t);
            yield return null;
        }
    }
}

public enum ParticipantType
{
    PLAYER_1,
    PLAYER_2,
    COMPUTER
}