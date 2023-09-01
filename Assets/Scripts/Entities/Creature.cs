using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Creature : MonoBehaviour
{
    //Attributes defined by Child of this class
    protected int health = 100;
    protected int weight = 80;
    protected float speed = .2f;       //moves per Minute

    //Physics
    protected Rigidbody2D rb2D;

    //Map
    private Tilemap tilemap;
    protected TileBaseManager tbm;

    //Brain
    [SerializeField] protected Dictionary<int, Vector2Int> spottedFood;
    [SerializeField] protected Dictionary<int, Vector2Int> spottedWater;
    [SerializeField] protected Dictionary<int, Vector2Int> spottedMate;

    //Movement
    [SerializeField] protected Vector2 target;
    [SerializeField] protected Vector2Int nextSteps = Vector2Int.zero;
    [SerializeField] protected direction facing;
    [SerializeField] private float leftOverSpeed = 0;   //in between moves

    //Needs
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float thirst = 100f;


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

        spottedFood = new Dictionary<int, Vector2Int>();
        spottedWater = new Dictionary<int, Vector2Int>();
        spottedMate = new Dictionary<int, Vector2Int>();

        initFoodTypes();
    }

    protected virtual void FixedUpdate()
    {

    }

    #region Brain
    public void AddFoodSource(GameObject food)
    {
        Vector2Int foodCoords = Util.convertVector3ToVector2Int(food.transform.position);
        spottedFood[food.GetInstanceID()] = foodCoords;
    }

    public void RemoveFoodSource(int ID)
    {
        if (spottedFood.ContainsKey(ID))
            spottedFood.Remove(ID);
    }

    public void AddWaterSource(GameObject water)
    {
        Vector2Int waterCoords = Util.convertVector3ToVector2Int(water.transform.position);
        spottedWater[water.GetInstanceID()] = waterCoords;
    }

    public void RemoveWaterSource(int ID)
    {
        if (spottedWater.ContainsKey(ID))
            spottedWater.Remove(ID);
    }

    public void AddPotentialMate(GameObject mate)
    {
        Vector2Int mateCoords = Util.convertVector3ToVector2Int(mate.transform.position);
        spottedMate[mate.GetInstanceID()] = mateCoords;
    }

    public void RemovePotentialMate(int ID)
    {
        if (spottedMate.ContainsKey(ID))
            spottedMate.Remove(ID);
    }



    protected abstract void initFoodTypes();

    #endregion

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
     */
    protected void MoveToTarget()
    {
        float theoreticalSpeed = speed * Gamevariables.MINUTES_PER_TICK + leftOverSpeed;
        int moves = (int)theoreticalSpeed;
        leftOverSpeed = theoreticalSpeed - moves;

        for (int i = 0; i < moves; i++)
        {
            if (Util.isDestinationReached(transform.position, target))
                setNewDestination();

            if (nextSteps == Vector2.zero)
                CalculateNextSteps(target);

            if (Mathf.Abs(nextSteps.x) > Mathf.Abs(nextSteps.y))
                {
                    if (nextSteps.x > 0)
                    {
                        StepTo(direction.EAST);
                    } else
                    {
                        StepTo(direction.WEST);
                    }
                }
                else
                {
                    if (nextSteps.y > 0)
                    {
                        StepTo(direction.NORTH);
                    } else
                    {
                        StepTo(direction.SOUTH);
                    }
                }
        }
    }

    protected void CalculateNextSteps(Vector3 destination)
    {
        Vector2 vect = Util.convertVector3ToVector2(destination - transform.position);

        if (vect.x == 0 || vect.y == 0)
        {
            nextSteps = new Vector2Int((int)vect.x, (int)vect.y);
            return;
        }

        if (Mathf.Abs(vect.x) >= Mathf.Abs(vect.y))
        {
            int x = Util.roundFloatUpPositiveDownNegative(vect.x / Mathf.Abs(vect.y));
            int y = 1;
            if (vect.y < 0) y = -1;
            nextSteps = new Vector2Int(x, y);
        } else
        {
            int y = Util.roundFloatUpPositiveDownNegative(vect.y / Mathf.Abs(vect.x));
            int x = 1;
            if (vect.x < 0) x = -1;
            nextSteps = new Vector2Int(x, y);
        }
    }

    protected void StepTo(direction dir)
    {
        if (dir == direction.NORTH)
        {
            nextSteps -= Vector2Int.up;
            transform.position += Vector3.up;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            return;
        }
        if (dir == direction.EAST)
        {
            nextSteps -= Vector2Int.right;
            transform.position += Vector3.right;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            return;
        }
        if (dir == direction.SOUTH)
        {
            nextSteps -= Vector2Int.down;
            transform.position += Vector3.down;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            return;
        }
        if (dir == direction.WEST)
        {
            nextSteps -= Vector2Int.left;
            transform.position += Vector3.left;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.left);
            return;
        }
    }

    protected void setNewDestination()
    {
        target = Util.getRandomCoordinateInPlayground();
        nextSteps = Vector2Int.zero;
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


    /*
     * subPerHour Default = 7 days without food
     */
    protected void hungerSubtractor(float subPerHour = .6f)
    {
        hunger -= subPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        if (hunger <= 0) death();
    }

    /*
     * subPerMinute Default = 3 days without water
     */
    protected void thirstSubtractor(float subPerHour = 1.38f)
    {
        thirst -= subPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
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
