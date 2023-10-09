using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public abstract class Creature : MonoBehaviour
{
    //Constants
    public static readonly int MAX_ENERGY = 100;
    public static readonly float MAX_HUNGER = 100f;
    public static readonly float MAX_THIRST = 100f;

    //Attributes
    public int MAX_HEALTH;

    public float energy = 100;
    public float health;
    public int weight;
    public float speed;       //moves per Minute
    [SerializeField] public gender gender;


    //States
    public bool awake = true;

    //Physics
    protected Rigidbody2D rb2D;

    //Map
    protected TileBaseManager tbm;

    //Brain
    protected Senses senses;
    [SerializeField] protected Dictionary<int, Vector2> spottedFood;
    [SerializeField] protected Dictionary<int, Vector2> spottedWater;
    [SerializeField] protected Dictionary<int, Vector2> spottedMate;

    //Movement
    [SerializeField] protected Vector2 target;
    [SerializeField] protected Vector2Int nextSteps = Vector2Int.zero;
    [SerializeField] public Direction facing;
    [SerializeField] private float leftOverSteps = 0;   //in between moves

    //Needs
    [SerializeField] public float hunger = 100f;
    [SerializeField] public float thirst = 100f;


    public enum Direction
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
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        senses = new Senses(this);
        spottedFood = new Dictionary<int, Vector2>();
        spottedWater = new Dictionary<int, Vector2>();
        spottedMate = new Dictionary<int, Vector2>();
    }

    protected virtual void FixedUpdate()
    {
        needAdder();
        needSubtractor();
    }

    /*  This Method needs to be called by every descenant of this Class!
     */
    protected void initAttributes(gender gender, int health, int weight, float speed)
    {
        this.gender = gender;
        this.MAX_HEALTH = health;
        this.health = health;
        this.weight = weight;
        this.speed = speed;

    }

    #region Brain
    protected void evaluateVision()
    {
        if (!awake) return;
        Vector2[] visionCoordinates = senses.getVisionCoordinates();
        for (int i = 0; i < visionCoordinates.Length; i++)
        {
            Vector2 coord = visionCoordinates[i];
            Collider2D[] overlaps = Physics2D.OverlapCircleAll(coord, .49f);
            for (int overlapIndex = 0; overlapIndex < overlaps.Length; overlapIndex++)
            {
                GameObject g = overlaps[overlapIndex].gameObject;
                Debug.Log(g);
                /* If self, or Another Vision Collidor -> do nothing*/
                if (g == gameObject) return;


                /* Potential Partner */
                if (isValidPartner(g))
                {
                    AddPotentialMate(g);
                    continue;
                }

                /* Potential Food */
                if (isEdibleFoodSource(g))
                {
                    AddFoodSource(g);
                    continue;
                }

                /* Potential Map Water*/
                if (spottedWater.ContainsValue(coord)) continue;
                if (tbm.isWater(g))
                {
                    AddWaterSource(g);
                    continue;
                }

                //Debug.LogError("spotted unknown gameobject: " + g);
            }
            
        }
    }

    protected abstract bool isEdibleFoodSource(GameObject g);
    protected void AddFoodSource(GameObject food)
    {
        Vector2Int foodCoords = Util.convertVector3ToVector2Int(food.transform.position);
        spottedFood[food.GetInstanceID()] = foodCoords;
    }

    protected void RemoveFoodSource(int ID)
    {
        if (spottedFood.ContainsKey(ID))
            spottedFood.Remove(ID);
    }

    protected void AddWaterSource(GameObject water)
    {
        Vector2Int waterCoords = Util.convertVector3ToVector2Int(water.transform.position);
        spottedWater.Add(water.GetInstanceID(), waterCoords);
    }

    protected void RemoveWaterSource(int ID)
    {
        if (spottedWater.ContainsKey(ID))
            spottedWater.Remove(ID);
    }

    protected bool isGenericMate(Creature partner)
    {
        return gender != partner.gender;
    }

    protected abstract bool isValidPartner(GameObject g);
    protected void AddPotentialMate(GameObject mate)
    {
        Vector2Int mateCoords = Util.convertVector3ToVector2Int(mate.transform.position);
        spottedMate[mate.GetInstanceID()] = mateCoords;
    }

    protected void RemovePotentialMate(int ID)
    {
        if (spottedMate.ContainsKey(ID))
            spottedMate.Remove(ID);
    }


    #endregion

    public void setTarget(Vector2 coord)
    {
        target = coord;
    }

    #region Movement
    /*  Movement is relative to the fixed update
        - which means that increasing the tickrate, will result in faster movement, but not faster hunger, thirst and day night cycle
     */
    protected void MoveToTarget()
    {
        float theoreticalMoves = speed * Gamevariables.MINUTES_PER_TICK + leftOverSteps;
        int moves = (int)theoreticalMoves;
        leftOverSteps = theoreticalMoves - moves;

        for (int i = 0; i < moves; i++)
        {
            //chance to not make a move based on health
            if (Random.Range(0f, 1f) > health / MAX_HEALTH)
                continue;

            //calculate new destination if reached in between ticks
            if (Util.isDestinationReached(transform.position, target))
                setNewDestination();

            if (nextSteps == Vector2.zero)
                CalculateNextSteps(target);

            if (Mathf.Abs(nextSteps.x) > Mathf.Abs(nextSteps.y))
            {
                if (nextSteps.x > 0)
                {
                    facing = Direction.EAST;
                    MakeStep();
                } else
                {
                    facing = Direction.WEST;
                    MakeStep();
                }
            }
            else
            {
                if (nextSteps.y > 0)
                {
                    facing = Direction.NORTH;
                    MakeStep();
                } else
                {
                    facing = Direction.SOUTH;
                    MakeStep();
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

    protected void MakeStep()
    {
        if (facing == Direction.NORTH)
        {
            nextSteps -= Vector2Int.up;
            transform.position += Vector3.up;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.up);
            return;
        }
        if (facing == Direction.EAST)
        {
            nextSteps -= Vector2Int.right;
            transform.position += Vector3.right;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.right);
            return;
        }
        if (facing == Direction.SOUTH)
        {
            nextSteps -= Vector2Int.down;
            transform.position += Vector3.down;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, Vector3.down);
            return;
        }
        if (facing == Direction.WEST)
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

    #region Needs
    protected void needAdder()
    {
        regenerate();
        if (!awake)
        {
            rest();
        }
    }
    protected void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
        if (awake)
        {
            energySubtractor();
        }
    }

    #region Health
    protected void takeDamage()
    {
        health -= 20;
        if (health <= 0)
        {
            death();
        }
    }
    protected void regenerate(float addPercentPerHour = .01f)
    {
        float add = addPercentPerHour * health * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        health = Mathf.Clamp(health + add, 0, MAX_HEALTH);
    }
    #endregion
    #region Hunger
    /*
     * subPerHour Default = 7 days without food
     */
    protected void hungerSubtractor(float subPerHour = .6f)
    {
        hunger -= subPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        if (hunger <= 0) death();
    }
    #endregion
    #region Thirst
    protected void drink(float addPerMinute = 20f)
    {
        float add = addPerMinute * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        //if (tbm.isWater(GetTile(transform.position)))
        {
            thirst = Mathf.Clamp(thirst + add, 0, MAX_THIRST);
        }
    }

    /*
     * subPerHour Default = 3 days without water
     */
    protected void thirstSubtractor(float subPerHour = 1.38f)
    {
        thirst -= subPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        if (thirst <= 0) death();
    }
    #endregion
    #region Energy
    /*
     * addPerHour Default = 100% Energy after 8h (without lightfactor)
     * lightScaler -> Scales how much the Light affects the rest
     */
    protected void rest(float addPerHour = 12.5f, float lightScaler = 1f)
    {
        addPerHour /= (1f + lightScaler);
        float addPerTick = addPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        float lightfactor = lightScaler * addPerTick * (1f - Gamevariables.LIGHT_INTENSITY);

        energy = Mathf.Clamp(energy + addPerTick + lightfactor, 0, MAX_ENERGY);
        if (energy >= MAX_ENERGY)
        {
            awake = true;
        }
    }

    /*
     * subPerHour Default = 1 day without sleep (without lightfactor)
     * lightScaler -> Scales how much the Light affects the exhaution
     */
    protected void energySubtractor(float subPerHour = 4.17f, float lightScaler = .25f)
    {
        subPerHour /= (1f + lightScaler);
        float subPerTick = subPerHour * (float)Gamevariables.MINUTES_PER_TICK / (float)Gamevariables.MINUTES_PER_HOUR;
        float lightfactor = lightScaler * subPerTick * Gamevariables.LIGHT_INTENSITY;

        energy = Mathf.Clamp(energy - subPerTick - lightfactor, 0, MAX_ENERGY);
        if (energy <= 0) awake = false;
    }
    #endregion

    #endregion



    protected void death()
    {
        Corpse c = GetComponent<Corpse>();
        c.enabled = true;
        c.setWeight(weight);

        GetComponent<Creature>().enabled = false;
    }

    ////DEBUG - REMOVE
    //private void OnDrawGizmos()
    //{
    //    Vector2[] va = senses.getVisionCoordinates();
    //    foreach(Vector2 v in va)
    //    {
    //        Gizmos.DrawSphere(v, .5f);
    //    }
    //}

}
