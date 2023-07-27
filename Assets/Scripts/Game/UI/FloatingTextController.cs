using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Snakes;
using ExtremeSnake.Game.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Snake))]
public class FloatingTextController : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    public GameObject FloatingMultiplierPrefab;

    [Range(0f, 5f)]
    public float HeightScale = 1f;

    private Transform Parent;
    private Queue<Action> TextQueue = new Queue<Action>();
    private string SnakeUUID;
    private float timeSinceLastSpawn = 0f;
    private float staggerTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(WaitToLoadComponents());
    }

    IEnumerator WaitToLoadComponents() {
        yield return new WaitForEndOfFrame();
        Snake s = GetComponent<Snake>();
        SnakeUUID = s.UUID;
        GameManager.Instance.GameEmitter.Subscribe<ScoreEventArgs>($"{SnakeUUID}OnScoreChanged",HandleSpawnText);
        Parent = s.Segments.First.Value.Segment.transform;
    }

    private void Update() {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > staggerTime && TextQueue.Count > 0) {
            if (TextQueue.Count > 5) {
                staggerTime = 0.15f;
            } else {
                staggerTime = 0.3f;
            }
            TextQueue.Dequeue()();
        }
    }

    void HandleSpawnText(object sender,ScoreEventArgs args) {
        if (args.PointsChange != 0) {
            TextQueue.Enqueue(() => SpawnText(String.Format("{0:N0}",args.PointsChange),args.TextColor));
        }
        if (args.MultiplierChanged && args.Score.Multiplier > 1) {
            TextQueue.Enqueue(() => SpawnText(String.Format("x{0:N0}",args.Score.Multiplier),args.MultiplierColors.GetGradient(args.Score.Multiplier)));
        }
    }

    private void SpawnText(string text, Color color) {
        GameObject go = SpawnText(FloatingTextPrefab);
        go.transform.GetChild(0).GetComponent<TextMesh>().text = text;
        go.transform.GetChild(0).GetComponent<TextMesh>().color = color;
    }

    private void SpawnText(string text,TMP_ColorGradient color) {
        GameObject go = SpawnText(FloatingMultiplierPrefab);
        go.transform.GetChild(0).GetComponent<TextMeshPro>().text = text;
        go.transform.GetChild(0).GetComponent<TextMeshPro>().colorGradientPreset = color;
    }

    private GameObject SpawnText(GameObject prefab) {
        GameObject go = GameObject.Instantiate(prefab,Parent.transform.position,Quaternion.identity);
        go.transform.position += Vector3.up * HeightScale;
        float duration = go.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).length;
        GameObject.Destroy(go,duration);
        timeSinceLastSpawn = 0f;
        return go;
    }

    [ContextMenu("Spawn Text")]
    public void MenuSpawn() {
        SpawnText("Hello World", Color.red);
        SpawnText("Hello World", Color.blue);
    }
}
