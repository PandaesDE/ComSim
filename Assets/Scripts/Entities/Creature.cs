using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Creature : MonoBehaviour
{
    [SerializeField] protected Vector2 target;

    private Tilemap tilemap;
    private int health = 100;

    private void Awake()
    {
        target = Util.getRandomCoordinateInPlayground();
        tilemap = GameObject.Find("Playground/Grid/Tilemap").GetComponent<Tilemap>();
    }

    protected void FixedUpdate()
    {
        if (Util.isDestinationReached(transform.position, target))
        {
            target = Util.getRandomCoordinateInPlayground();
        }
        MoveTowards(target);
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

    protected void MoveTowards(Vector3 destination)
    {
        Vector3 vect = destination - transform.position;
        if (Mathf.Abs(vect.x) > Mathf.Abs(vect.y))
        {
            transform.position += new Vector3(vect.x / Mathf.Abs(vect.x), 0, 0);
        }
        else
        {
            transform.position += new Vector3(0, vect.y / Mathf.Abs(vect.y), 0);
        }
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
