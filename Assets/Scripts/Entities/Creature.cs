using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Creature : MonoBehaviour
{
    [SerializeField] protected Vector2 target;


    //Physics
    protected Rigidbody2D rb2D;

    //Map
    private Tilemap tilemap;
    protected TileBaseManager tbm;

    //Brain
    protected List<GameObject> spottedFood;
    protected List<GameObject> spottedWater;
    protected List<GameObject> spottedMate;

    //Movement
    protected direction direct;

    //Needs
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
        rb2D = GetComponent<Rigidbody2D>();
        target = Util.getRandomCoordinateInPlayground();
        tilemap = GameObject.Find("Playground/Grid/Tilemap").GetComponent<Tilemap>();
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();
    }

    public void setTarget(Vector2 coord)
    {
        target = coord;
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
        Vector2 velocity;
        if (dir == direction.NORTH)
        {
            velocity = Vector2.up;
            rb2D.MovePosition(rb2D.position + velocity);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            return;
        }
        if (dir == direction.EAST)
        {
            velocity = Vector2.right;
            rb2D.MovePosition(rb2D.position + velocity);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            return;
        }
        if (dir == direction.SOUTH)
        {
            velocity = Vector2.down;
            rb2D.MovePosition(rb2D.position + velocity);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            return;
        }
        if (dir == direction.WEST)
        {
            velocity = Vector2.left;
            rb2D.MovePosition(rb2D.position + velocity);
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
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
        transform.GetChild(0).GetComponent<Senses>().enabled = false;
    }
}
