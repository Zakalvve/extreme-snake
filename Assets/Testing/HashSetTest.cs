using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HashSetTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        HashSet<Vector3Int> set1 = new HashSet<Vector3Int>(new Vector3Int[] { Vector3Int.up, Vector3Int.down, Vector3Int.zero });
        DisplaySet("Set 1: ",set1);
        HashSet<Vector3Int> set2 = new HashSet<Vector3Int>(new Vector3Int[] { Vector3Int.left,Vector3Int.right,Vector3Int.zero });
        DisplaySet("Set 2: ",set1);
        HashSet<Vector3Int> addSet = new HashSet<Vector3Int>(set1);
        addSet.UnionWith(set2);
        DisplaySet("1 + 2: ",addSet);
        HashSet<Vector3Int> subtractSet = new HashSet<Vector3Int>(set1);
        subtractSet.ExceptWith(set2);
        DisplaySet("1 - 2: ",subtractSet);
    }

    void DisplaySet<T>(string output, HashSet<T> collection) {
        output+= "{";

        foreach (T i in collection) {
            output += String.Format(" {0}",i);
        }
        output += " }";

        Debug.Log(output);
    }
}
