using ExtremeSnake.Game;
using ExtremeSnake.Game.Snakes;
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
                OnReady();
                break;
            default: 
                PlayerTypeText.text = "COM";
                OnReady();
                break;
        }
        SetReadyIcons();
    }

    public ControllingEntitySettings GetOutput() {
        return new ControllingEntitySettings(SkinOptions[SelectedSkin],Name,PlayerType);
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
        _isReady = !_isReady;
        SetReadyIcons();
    }

    public void AddPlayer() {
        GameManager.Instance.GameEmitter.Emit("OnPlayerComponentAdded",this);
    }
}

public enum ParticipantType
{
    PLAYER_1,
    PLAYER_2,
    COMPUTER
}