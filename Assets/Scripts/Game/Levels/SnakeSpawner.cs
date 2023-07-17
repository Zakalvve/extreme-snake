using Assets.Scripts.Core;
using ExtremeSnake.Game.Snakes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpawner : InstanceTracker<SnakeSpawner>, ISpawner
{
    public SnakeStartingDirections SpawnDirection;
    public string SpawnLayerName;
    //simple script that spawns a given game object at this game objects position, returning the created game object
    public GameObject Spawn(GameObject prefab) {
        var go = GameObject.Instantiate(prefab);
        go.transform.position = gameObject.transform.position;
        go.GetComponent<Snake>().InitialDirection = SpawnDirection;

        go.layer = LayerMask.NameToLayer(SpawnLayerName);
        return go;
    }
}
