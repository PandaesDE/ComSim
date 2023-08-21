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

    protected int weight;

    protected enum direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    protected virtual void Awake()
    {
        target = Util.getRandomCoordinateInPlayground();
        tilemap = GameObject.Find("Playground/Grid/Tilemap").GetComponent<Tilemap>();
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();
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
    /*  Movement is relative to the fixed update
        - which means that increasing the tickrate, will result in faster movement, but not faster hunger, thirst and day night cycle
        - it is yet not decided whether or not this will be fixed
     */
    protected void MoveTowards(Vector3 destination)
    {
        Vector3 vect = destination - transform.position;
        if (Mathf.Abs(vect.x) > Mathf.Abs(vect.y))
        {
            if (vect.x > 0)
            {
                MoveTo(direction.EAST);
            } else
            {
                MoveTo(direction.WEST);
            }
        }
        else
        {
            if (vect.y > 0)
            {
                MoveTo(direction.NORTH);
            } else
            {
                MoveTo(direction.SOUTH);
            }
        }
    }

    protected void MoveTo(direction dir)
    {
        if (dir == direction.NORTH)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            transform.position += Vector3.up;
            return;
        }
        if (dir == direction.EAST)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            transform.position += Vector3.right;
            return;
        }
        if (dir == direction.SOUTH)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            transform.position += Vector3.down;
            return;
        }
        if (dir == direction.WEST)
        {
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
            transform.position += Vector3.left;
            return;
        }
    }
    #endregion

    #region needSatisfier
    protected void drink()
    {
        if (tbm.isWater(GetTile(transform.position)))
        {
            thirst = Mathf.Clamp(thirst + 1, 0, 100);
        }
    }
    #endregion
    #region needSubtractor
    protected void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
    }

    protected void hungerSubtractor(float subPerHour = .6f) //7 days without food
    {
        hunger -= subPerHour / (float)Gamevariables.TICKS_PER_HOUR;
        if (hunger <= 0) death();
    }


    protected void thirstSubtractor(float subPerHour = 1.38f) //3 days without water
    {
        thirst -= subPerHour / (float)Gamevariables.TICKS_PER_HOUR;
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
        Corpse c = GetComponent<Corpse>();
        c.enabled = true;
        c.setWeight(weight);

        GetComponent<Creature>().enabled = false;
        
    }
}
