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
using ExtremeSnake.Game.UI;
using ExtremeSnake.Utils;

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
    bool isTwoPlayer = false;

    public GameObject onePlayerToggle;
    public GameObject twoPlayerToggle;
    public List<string> SnakeNames = new List<string>();


    //initialization
    private void Awake() {
        SelectedLevelIndex = 0;
        DifficultyDropdown.ClearOptions();
        DifficultyDropdown.AddOptions(DifficultyOptions.Select(option => option.Name).ToList());

        SelectedDifficultyName = DifficultyDropdown.options[DifficultyDropdown.value].text;

        InitializePlayersPanel(true);

        if (gameObject.activeSelf) OnActivate();

        GameManager.Instance.GameEmitter.Subscribe<AddPlayerEventArgs>("OnPlayerComponentAdded",HandleAddPlayer);
        GameManager.Instance.GameEmitter.Subscribe("OnLevelChanged",SyncPlayers);
        GameManager.Instance.GameEmitter.Subscribe("RemovePlayerComponent",RemovePlayer);
    }
    public void OnActivate() {
        UpdateLevelSelectPanel();
        InitializePlayersPanel(true);
    }
    public void InitializePlayersPanel(bool destroyAll = false) {
        if (destroyAll) {
            while (_playerComponents.Count > 0) {
                GameObject.Destroy(_playerComponents.Pop());
            }
        }

        if (GameManager.Instance.Settings.Player1 == null) {
            _playerComponents.Push(CreateMenuPlayerComponent(ParticipantType.PLAYER_1));
        } else {
            _playerComponents.Push(CreateMenuPlayerComponent(GameManager.Instance.Settings.Player1));
        }

        if (GameManager.Instance.Settings.Player2 != null) {
            toggleTwoPlayer();
            addSyncRequired = true;
        } else {
            _playerComponents.Push(CreateMenuPlayerComponent());
        }
        
        //if (isTwoPlayer) _playerComponents.Push(CreateMenuPlayerComponent(ParticipantType.PLAYER_2));
        //addSyncRequired = false;
    }

    //gather data and start game
    public void OnPlayGame() {
        List<Actor> chosenActors = _playerComponents
            .Select(component => component.GetComponent<MenuPlayerComponent>())
            .Where(component => component.IsPlayerActive)
            .Select(component => component.GetOutput())
            .ToList();

        Difficulty chosenDifficulty = DifficultyOptions.Where(item => item.Name == SelectedDifficultyName).First();

        //Session generatedSession = new Session(chosenDuration, chosenActors,chosenDifficulty);
        SessionData generatedSession = new SessionData(chosenActors,chosenDifficulty);
        GameManager.Instance.Settings.ActiveSession = generatedSession;

        GameManager.Instance.GameEmitter.Emit("OnLoadGame",this,new LoadLevelArgs(Levels[SelectedLevelIndex]));
    }

    //changing level
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
    public void UpdateLevelSelectPanel() {
        LevelName.text = Levels[SelectedLevelIndex].LevelName;
        LevelPreviewImage.sprite = Levels[SelectedLevelIndex].Thumbnail;
    }

    //change difficulty
    public void OnDifficultyDrowpdownChange() {
        Debug.Log(DifficultyDropdown.options[DifficultyDropdown.value].text);
        SelectedDifficultyName = DifficultyDropdown.options[DifficultyDropdown.value].text;
    }

    //players
    //should sync all the player components
    public void SyncPlayers(object sender) {
        //sync players state with level state
        SyncPlayersWithLevel();

        //sync display
        SyncPlayersDisplay();
    }

    public void SyncPlayersWithLevel() {
        if (_playerComponents.Count < Levels[SelectedLevelIndex].MaxPlayers) {
            if (addSyncRequired) {
                _playerComponents.Push(CreateMenuPlayerComponent());
                addSyncRequired = false;
            }
        }
        else {
            while (_playerComponents.Count > Levels[SelectedLevelIndex].MaxPlayers) {
                GameObject.Destroy(_playerComponents.Pop());
                addSyncRequired = true;
            }
        }
    }
    public void SyncPlayersDisplay() {
        Stack<GameObject> store = new Stack<GameObject>();
        while (_playerComponents.Count > 1) {
            store.Push(_playerComponents.Pop());
        }
        Vector3 root = _playerComponents.Peek().transform.localPosition;

        while(store.Count > 0) {
            GameObject go = store.Pop();
            go.transform.localPosition = root + Vector3.down * _playerComponents.Count * (go.GetComponent<RectTransform>().rect.height + PlayerComponentSpacing);
            go.transform.SetAsLastSibling();
            _playerComponents.Push(go);
        }
    }

    public GameObject CreateMenuPlayerComponent(Actor actor) {
        GameObject go = GameObject.Instantiate(MenuPlayerComponentPrefab,PlayerSetupPanel.transform);
        go.transform.localPosition += Vector3.down * _playerComponents.Count * (go.GetComponent<RectTransform>().rect.height + PlayerComponentSpacing);
        go.GetComponent<MenuPlayerComponent>().Initialize(actor);
        go.transform.SetAsLastSibling();
        return go;
    }

    public GameObject CreateMenuPlayerComponent(ParticipantType type = ParticipantType.COMPUTER) {
        GameObject go = GameObject.Instantiate(MenuPlayerComponentPrefab,PlayerSetupPanel.transform);
        go.transform.localPosition += Vector3.down * _playerComponents.Count * (go.GetComponent<RectTransform>().rect.height + PlayerComponentSpacing);
        go.GetComponent<MenuPlayerComponent>().Initialize(type,UtilsClass.RandomElement<string>(SnakeNames));
        go.transform.SetAsLastSibling();
        return go;
    }
    //called when an add player button is clicked
    public void HandleAddPlayer(object sender,AddPlayerEventArgs args) {
        //toggle visibility of current panel
        _playerComponents.Peek().GetComponent<MenuPlayerComponent>().EnableDisplay();

        if (_playerComponents.Count < Levels[SelectedLevelIndex].MaxPlayers) {
            _playerComponents.Push(CreateMenuPlayerComponent(args.playerType));
        }
        else {
            addSyncRequired = true;
        }
    }

    public void HandleAddPlayer(Actor actor) {
        //toggle visibility of current panel
        _playerComponents.Peek().GetComponent<MenuPlayerComponent>().EnableDisplay();

        if (_playerComponents.Count < Levels[SelectedLevelIndex].MaxPlayers) {
            _playerComponents.Push(CreateMenuPlayerComponent(actor));
        }
        else {
            addSyncRequired = true;
        }
    }

    public void RemovePlayer(object sender) {
        GameObject toRemove = ((MenuPlayerComponent)sender).gameObject;
        bool deleted = false;
        Stack<GameObject> store = new Stack<GameObject>();

        while (!deleted) {
            GameObject found = _playerComponents.Pop();
            if (found == toRemove) {
                deleted = true;
                GameObject.Destroy(toRemove);
            }
            else {
                store.Push(found);
            }
        }

        while (store.Count > 0) {
            _playerComponents.Push(store.Pop());
        }

        GameManager.Instance.GameEmitter.Emit("OnLevelChanged",this);
    }

    public void toggleTwoPlayer() {

        //need to add the supports two player bool to levels for this to work
        //if (!isTwoPlayer && !Levels[SelectedLevelIndex].SupportsTwoPlayer) return;

        isTwoPlayer = !isTwoPlayer;

        Debug.Log(isTwoPlayer);
        //update the player component stack
        if (isTwoPlayer) {
            //we need to pop off all entries other than
            onePlayerToggle.SetActive(false);
            twoPlayerToggle.SetActive(true);

            Stack<GameObject> store = new Stack<GameObject>();
            while (_playerComponents.Count > 1) {
                store.Push(_playerComponents.Pop());
            }

            //add a new player
            if (GameManager.Instance.Settings.Player2 == null) {
                GameManager.Instance.GameEmitter.Emit("OnPlayerComponentAdded",this,new AddPlayerEventArgs(ParticipantType.PLAYER_2));
            } else {
                HandleAddPlayer(GameManager.Instance.Settings.Player2);
            }

            while (store.Count > 0) {
                _playerComponents.Push(store.Pop());
            }
            GameManager.Instance.GameEmitter.Emit("OnLevelChanged",this);
        } else {
            onePlayerToggle.SetActive(true);
            twoPlayerToggle.SetActive(false);

            Stack<GameObject> store = new Stack<GameObject>();
            while (_playerComponents.Count > 2) {
                store.Push(_playerComponents.Pop());
            }

            //if these is a second player (there should be) pop it off and discard it
            if (_playerComponents.Peek().GetComponent<MenuPlayerComponent>().IsTwoPlayer) {
                GameObject.Destroy(_playerComponents.Pop());
            }

            while (store.Count > 0) {
                _playerComponents.Push(store.Pop());
            }
            GameManager.Instance.GameEmitter.Emit("OnLevelChanged",this);
        }
    }
}
