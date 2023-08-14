using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Creature : MonoBehaviour
{
    [SerializeField] protected Vector2 target;

    private Tilemap tilemap;
    protected TileBaseManager tbm;

    //movement
    protected direction direct;

    //needs
    private int health = 100;
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float thirst = 100f;

    protected enum direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    private void Awake()
    {
        target = Util.getRandomCoordinateInPlayground();
        tilemap = GameObject.Find("Playground/Grid/Tilemap").GetComponent<Tilemap>();
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();
    }

    protected void FixedUpdate()
    {
        //movement
        if (Util.isDestinationReached(transform.position, target))
        {
            target = Util.getRandomCoordinateInPlayground();
        }
        MoveTowards(target);

        //needs
        needSubtractor();
        drink();

    }

    protected TileBase GetTile(Vector3 coord)
    {
        return GetTile(new Vector3Int((int)coord.x, (int)coord.y, (int)coord.z));
    }

    protected TileBase GetTile(Vector3Int coord)
    {
        return tilemap.GetTile(coord);
    }

    #region Movement
    protected void MoveTowards(Vector3 destination)
    {
        Vector3 vect = destination - transform.position;
        if (Mathf.Abs(vect.x) > Mathf.Abs(vect.y))
        {
            if (vect.x > 0)
            {
                TurnTo(direction.EAST);
                transform.position += Vector3.right;
            } else
            {
                TurnTo(direction.WEST);
                transform.position += Vector3.left;
            }
        }
        else
        {
            if (vect.y > 0)
            {
                TurnTo(direction.NORTH);
                transform.position += Vector3.up;
            } else
            {
                TurnTo(direction.SOUTH);
                transform.position += Vector3.down;
            }
        }
    }

    protected void TurnTo(direction dir)
    {
        direct = dir;
        if (dir == direction.NORTH)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
        }
        if (dir == direction.EAST)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
        }
        if (dir == direction.SOUTH)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
        }
        if (dir == direction.WEST)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
        }
    }
    #endregion

    #region needSatisfier
    private void drink()
    {
        if (tbm.isWater(GetTile(transform.position)))
        {
            thirst = Mathf.Clamp(thirst + 1, 0, 100);
        }
    }
    #endregion
    #region needSubtractor
    private void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
    }

    protected void hungerSubtractor(float sub = .1f)
    {
        hunger -= sub;
        if (hunger <= 0) death();
    }


    protected void thirstSubtractor(float sub = .33f)
    {
        thirst -= sub;
        if (thirst <= 0) death();
    }
    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OTE");
        if (collision.CompareTag(tags.ANIMAL))
        {
            Debug.Log("OTE ANIMAL");
            takeDamage();
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
