using ExtremeSnake.Core;
using ExtremeSnake.Game;
using ExtremeSnake.Game.Data;
using ExtremeSnake.Game.Snakes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FloatingTextController : MonoBehaviour
{
    public GameObject FloatingTextPrefab;
    [Range(0f, 5f)]
    public float HeightScale = 1f;

    private Transform Parent;
    private Queue<FloatingTextData> TextQueue = new Queue<FloatingTextData>();
    private string SnakeUUID;
    private float timeSinceLastSpawn = 0f;
    private float staggerTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
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
            SpawnText(TextQueue.Dequeue());
        }
    }

    void HandleSpawnText(object sender,ScoreEventArgs args) {
        if (args.PointsChange != 0) {
            TextQueue.Enqueue(new FloatingTextData(String.Format("{0:N0}",args.PointsChange), args.TextColor));
        }
        if (args.MultiplierChanged && args.Score.Multiplier > 1) {
            TextQueue.Enqueue(new FloatingTextData(String.Format("x{0:N0}",args.Score.Multiplier),args.MultiplierColors.GetFlatColor(args.Score.Multiplier)));
        }
    }

    void SpawnText(FloatingTextData data) {
        GameObject go = GameObject.Instantiate(FloatingTextPrefab,Parent.transform.position, Quaternion.identity);
        go.transform.position += Vector3.up * HeightScale;
        go.transform.GetChild(0).GetComponent<TextMesh>().text = data.text;
        go.transform.GetChild(0).GetComponent<TextMesh>().color = data.textColor;
        //giving wrong duration
        //float duration = go.transform.GetChild(0).GetComponent<Animator>().GetCurrentAnimatorClipInfo(0).Length;
        float duration = 0.717f;
        GameObject.Destroy(go,duration);
        timeSinceLastSpawn = 0f;
    }

    [ContextMenu("Spawn Text")]
    public void MenuSpawn() {
        SpawnText(new FloatingTextData("Hello World", Color.red));
        SpawnText(new FloatingTextData("Hello World", Color.blue));
    }
}

public struct FloatingTextData
{
    public FloatingTextData(string t,Color c) { text = t; textColor = c; }
    public string text;
    public Color textColor;
}
