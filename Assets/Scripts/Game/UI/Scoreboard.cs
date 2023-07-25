using ExtremeSnake.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    public GameObject Root;
    public GameObject ComponentContainer;
    public GameObject ComponentPrefab;

    public List<ScoreboardComponent> Components = new List<ScoreboardComponent>();
    void Start() {
        GameManager.Instance.GameEmitter.Subscribe("OnLoadComplete",Initialize);
        GameManager.Instance.GameEmitter.Subscribe("ShowScoreboard",Display);
        GameManager.Instance.GameEmitter.Subscribe("OnPostGame",Display);
        GameManager.Instance.GameEmitter.Subscribe("HideScoreboard",Hide);
    }

    public void Initialize(object sender) {
        //calculate containers height
        RectTransform container = ComponentContainer.GetComponent<RectTransform>();
        Vector2 containerSize = container.sizeDelta;
        Vector2 componentSize = ComponentPrefab.GetComponent<RectTransform>().sizeDelta;
        container.sizeDelta = new Vector2(containerSize.x,componentSize.y * GameManager.Instance.Settings.ActiveSession.Actors.Count);
        //spawn components
        List<Actor> actors = GameManager.Instance.Settings.ActiveSession.Actors;
        for (int i = 0; i < actors.Count; i++) {
            GameObject go = GameObject.Instantiate(ComponentPrefab,ComponentContainer.transform);
            go.transform.localPosition += Vector3.down * i * componentSize.y;
            Components.Add(go.GetComponent<ScoreboardComponent>());
        }
    }

    public void Display(object sender) {
        //update the components with snake scores

        //set self as active
        if (Components.Count != GameManager.Instance.Settings.ActiveSession.Actors.Count) {
            throw new System.Exception("Score components not synced with actor count. Session actors must had been modified after component initialization.");
        } else {
            for (int i = 0; i < GameManager.Instance.Settings.ActiveSession.Actors.Count; i++) {
                Components[i].Display(GameManager.Instance.Settings.ActiveSession.Actors[i].Name,GameManager.Instance.Settings.ActiveSession.Actors[i].ActorScore.Score);
            }
            Root.SetActive(true);
        }
    }

    public void HandleFinish() {
        GameManager.Instance.GameEmitter.Emit("OnFinish",this);
    }

    public void Hide(object sender) {
        Root.SetActive(false);
    }

    [ContextMenu("Display")]
    public void DebugDisplay() {
        Display(this);
    }
    [ContextMenu("Hide")]
    public void DebugHide() {
        Hide(this);
    }
}
