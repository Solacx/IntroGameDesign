using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TestTilemap : MonoBehaviour
{
    public Tilemap map;
    public Vector3Int pos;

    void Update() {
        Debug.Log(map.GetTile(pos).name);
    }
}
