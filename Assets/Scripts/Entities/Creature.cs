using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Creature : MonoBehaviour
{
    private Tilemap tilemap;
    private int health = 100;

    private void Awake()
    {
        tilemap = GameObject.Find("Playground/Grid/Tilemap").GetComponent<Tilemap>();
    }

    protected void LogTile(Vector3 coord)
    {
        Vector3Int coordInt = new Vector3Int((int)coord.x, (int)coord.y, (int)coord.z);
        LogTile(coordInt);
    }

    protected void LogTile(Vector3Int coord)
    {
        TileBase tb = tilemap.GetTile(coord);
        Debug.Log(tb.name);
    }

    protected void takeDamage()
    {
        health -= 20;
        if (health <= 0)
        {
            death();
        }
    }

    protected void death()
    {
        Destroy(gameObject);
    }
}
