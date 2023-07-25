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

    public void Initialize(ParticipantType type = ParticipantType.COMPUTER) {
        PlayerType = type;
        IsPlayer = type != ParticipantType.COMPUTER;

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
        return new Actor(SkinOptions[SelectedSkin],Name,PlayerType);
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
}

public enum ParticipantType
{
    PLAYER_1,
    PLAYER_2,
    COMPUTER
}