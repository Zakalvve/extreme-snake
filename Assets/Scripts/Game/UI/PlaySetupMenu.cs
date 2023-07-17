using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Assets.Scripts.Game.Data;
using System.Linq;
using ExtremeSnake.Game;
using Assets.Scripts.Game.Events;
using ExtremeSnake.Game.Snakes;
using System;

public class PlaySetupMenu : MonoBehaviour
{
    private int SelectedLevelIndex;
    public List<LevelModel> Levels;

    private string SelectedDifficultyName;
    public List<Difficulty> DifficultyOptions;

    public List<SnakeSprites> Skins;

    public TMP_Dropdown DifficultyDropdown;
    public Text LevelName;
    public Image LevelPreviewImage;

    public GameObject PlayerSetupPanel;
    public GameObject MenuPlayerComponentPrefab;

    private Stack<GameObject> _playerComponents = new Stack<GameObject>();

    public float PlayerComponentSpacing = 14;

    bool addSyncRequired = false;

    private void Awake() {
        SelectedLevelIndex = 0;
        DifficultyDropdown.ClearOptions();
        DifficultyDropdown.AddOptions(DifficultyOptions.Select(option => option.Name).ToList());

        SelectedDifficultyName = DifficultyDropdown.options[DifficultyDropdown.value].text;

        UpdatePlayersPanel(true);

        if (gameObject.activeSelf) OnActivate();

        GameManager.Instance.GameEmitter.Subscribe("OnPlayerComponentAdded",AddPlayer);
        GameManager.Instance.GameEmitter.Subscribe("OnLevelChanged",SyncPlayers);
    }

    public void OnActivate() {
        UpdateLevelSelectPanel();
        UpdatePlayersPanel(true);
    }

    public void UpdateLevelSelectPanel() {
        LevelName.text = Levels[SelectedLevelIndex].LevelName;
        LevelPreviewImage.sprite = Levels[SelectedLevelIndex].Thumbnail;
    }

    public void UpdatePlayersPanel(bool destroyAll = false) {
        if (destroyAll) {
            while (_playerComponents.Count > 0) {
                GameObject.Destroy(_playerComponents.Pop());
            }
        }

        _playerComponents.Push(CreateMenuPlayerComponent(ParticipantType.PLAYER_1));
        _playerComponents.Push(CreateMenuPlayerComponent());
        addSyncRequired = false;
    }

    public void SyncPlayers(object sender) {
        if (_playerComponents.Count < Levels[SelectedLevelIndex].MaxPlayers) {
            if (addSyncRequired) {
                _playerComponents.Push(CreateMenuPlayerComponent());
                addSyncRequired = false;
            }
        } else {
            while (_playerComponents.Count > Levels[SelectedLevelIndex].MaxPlayers) {
                GameObject.Destroy(_playerComponents.Pop());
                addSyncRequired = true;
            }
        }
    }

    public void NextLevel() {
        SelectedLevelIndex++;
        if (SelectedLevelIndex >= Levels.Count) SelectedLevelIndex = 0;
        UpdateLevelSelectPanel();
        GameManager.Instance.GameEmitter.Emit("OnLevelChanged",this);
    }

    public void PreviousLevel() {
        SelectedLevelIndex--;
        if (SelectedLevelIndex < 0) SelectedLevelIndex = Levels.Count - 1;
        UpdateLevelSelectPanel();
        GameManager.Instance.GameEmitter.Emit("OnLevelChanged",this);
    }

    public void OnDifficultyDrowpdownChange() {
        Debug.Log(DifficultyDropdown.options[DifficultyDropdown.value].text);
        SelectedDifficultyName = DifficultyDropdown.options[DifficultyDropdown.value].text;
    }

    public void OnPlayGame() {
        GameManager.Instance.Settings.DifficultySettings = DifficultyOptions.Where(item => item.Name == SelectedDifficultyName).First();
        GameManager.Instance.Settings.SnakeControllingEntity = _playerComponents
            .Select(component => component.GetComponent<MenuPlayerComponent>())
            .Where(component => component.IsPlayerActive)
            .Select(component => component.GetOutput())
            .ToList();

        GameManager.Instance.GameEmitter.Emit("OnLoadGame",this,new LoadLevelArgs(Levels[SelectedLevelIndex]));
    }

    public GameObject CreateMenuPlayerComponent(ParticipantType type = ParticipantType.COMPUTER) {
        GameObject go = GameObject.Instantiate(MenuPlayerComponentPrefab,PlayerSetupPanel.transform);
        go.transform.localPosition += Vector3.down * _playerComponents.Count * (go.GetComponent<RectTransform>().rect.height + PlayerComponentSpacing);
        go.GetComponent<MenuPlayerComponent>().Initialize(type);
        return go;
    }

    public void AddPlayer(object sender) {
        //toggle visibility of current panel
        _playerComponents.Peek().GetComponent<MenuPlayerComponent>().EnableDisplay();

        if (_playerComponents.Count < Levels[SelectedLevelIndex].MaxPlayers) {
            _playerComponents.Push(CreateMenuPlayerComponent());
        } else {
            addSyncRequired = true;
        }
    }
}
