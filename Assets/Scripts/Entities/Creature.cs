/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      - This class can not be instanciated alone, it only defines the generic funcionality of a Creature
 *  Class References:
 *      
 */

using System.Collections.Generic;
using UnityEngine;

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
    [SerializeField] protected Status mission = Status.WANDERING;

    //Physics
    protected Rigidbody2D rb2D;

    //Map
    protected TileBaseManager tbm;

    //Brain
    protected Senses senses;
    protected IDietary dietary;
    private readonly int MINUTES_UNTIL_STATUS_DETERMINING = 60;
    private int minutes_left_until_status_determining = 0;

    [SerializeField] protected Dictionary<int, IConsumable> spottedFood;
    [SerializeField] protected IConsumable activeFood;
    [SerializeField] protected Dictionary<int, Creature> spottedCreature;
    [SerializeField] protected Creature activeHunt;
    [SerializeField] protected Creature activeFlee;
    [SerializeField] protected Dictionary<int, Vector2> spottedWater;
    [SerializeField] protected bool foundValidWaterSpot = false;
    [SerializeField] protected Dictionary<int, Vector2> spottedMate;

    //Movement
    [SerializeField] protected Vector2 target;
    [SerializeField] protected Vector2Int nextSteps = Vector2Int.zero;
    [SerializeField] public Direction facing;
    [SerializeField] private float leftOverSteps = 0;   //in between moves

    //Needs
    [SerializeField] public float hunger = 100f;
    [SerializeField] public float thirst = 100f;

    //Corpse
    [SerializeField] private GameObject PREFAB_CORPSE;


    public enum Direction
    {
        NORTH,
        EAST,
        SOUTH,
        WEST
    }

    protected enum Status
    {
        FLEEING,
        STARVING,
        HUNGRY,
        DEHYDRATED,
        THIRSTY,
        HUNTING,
        LOOKING_FOR_PARTNER,
        WANDERING
    }


    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        senses = new Senses(this);
        spottedFood = new Dictionary<int, IConsumable>();

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
    protected void initAttributes(gender gender, IDietary dietary, int health, int weight, float speed)
    {
        this.gender = gender;
        this.dietary = dietary;
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
            Collider2D[] overlaps = Physics2D.OverlapPointAll(visionCoordinates[i]);
            for (int overlapIndex = 0; overlapIndex < overlaps.Length; overlapIndex++)
            {
                GameObject g = overlaps[overlapIndex].gameObject;

                /* If self, or Another Vision Collidor -> do nothing*/
                if (g == this.gameObject) return;

                /* Food Source (IConsumable)*/
                if (isEdibleFoodSource(g))
                {
                    AddFoodSource(g);
                    continue;
                }

                /* Creature Evaluation */
                if (isCreature(g))
                {
                    evaluateCreature(g);
                }



                /* Water Source*/
                if (spottedWater.ContainsKey(g.GetInstanceID())) continue;
                if (tbm.isWater(g))
                {
                    AddWaterSource(g);
                    continue;
                }
            }
        }
    }

    protected void determineStatus()
    {
        int normal_cap = 80;
        int important_cap = 20;

        //has a Mission, Check for important Missions
        if (hunger <= important_cap)
        {
            mission = Status.STARVING;
            setActiveFoodSource();
            return;
        }
        if (thirst <= important_cap)
        {
            mission = Status.DEHYDRATED;
            setWaterTarget();
            return;
        }
        if (hunger <= normal_cap)
        {
            mission = Status.HUNGRY;
            setActiveFoodSource();
            return;
        }
        if (thirst <= normal_cap)
        {
            mission = Status.THIRSTY;
            setWaterTarget();
            return;
        }

        mission = Status.WANDERING;
    }

    protected void makeStatusBasedMove()
    {
        //automatic check over time for more important task
        if (minutes_left_until_status_determining <= 0)
        {
            determineStatus();
            minutes_left_until_status_determining = MINUTES_UNTIL_STATUS_DETERMINING;
        }
        //always search for a more important task than default
        else if (mission == Status.WANDERING)
        {
            determineStatus();
        }
        else
        {
            minutes_left_until_status_determining -= Gamevariables.MINUTES_PER_TICK;
        }

        if (mission == Status.HUNGRY || mission == Status.STARVING)
        {
            if (Util.isDestinationReached(transform.position, target))
            {
                if (activeFood == null)
                {
                    determineStatus();
                    return;
                }

                if (!activeFood.hasFood)
                {
                    determineStatus();
                    return;
                }

                if (hunger >= MAX_HUNGER)
                {
                    determineStatus();
                    return;
                }

                eat(activeFood.Consume());
            }
        }

        if (mission == Status.THIRSTY || mission == Status.DEHYDRATED)
        {
            if (Util.isDestinationReached(transform.position, target))
            {
                if (thirst >= MAX_THIRST)
                {
                    determineStatus();
                    return;
                }

                if (!foundValidWaterSpot)
                {
                    determineStatus();
                    return;
                }

                drink();
            }
        }
    }

    private bool isCreature(GameObject g)
    {
        return g.GetComponent<Creature>() != null;
    }

    private void evaluateCreature(GameObject g)
    {
        if (isSameSpecies(g))
        {
            if (isValidPartner(g))
            {
                AddPotentialMate(g);
            }
        } else
        {
            dietary.evaluateCreature(g);
        }
    }

    #region Hunger
    protected bool isEdibleFoodSource(GameObject g)
    {
        IConsumable food = g.GetComponent<IConsumable>();
        if (food == null) return false;
        return dietary.isEdibleFoodSource(food);
    }
    protected void AddFoodSource(GameObject g)
    {
        IConsumable food = g.GetComponent<IConsumable>();
        spottedFood[food.gameObject.GetInstanceID()] = food;
    }

    protected void setActiveFoodSource()
    {
        activeFood = getNearestFoodSource();
        if (activeFood != null)
            setTarget(activeFood.gameObject.transform.position);
        else
            setRandomTarget();
    }

    protected IConsumable getNearestFoodSource()
    {
        IConsumable closest = null;
        float minDistance = 100000f;
        if (spottedFood.Count <= 0) return null;
        foreach (KeyValuePair<int, IConsumable> keyValue in spottedFood)
        {
            if (!keyValue.Value.hasFood) continue;

            float distance = Vector3.Distance(keyValue.Value.gameObject.transform.position, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        return closest;
    }

    protected void RemoveFoodSource(IConsumable food)
    {
        RemoveFoodSource(food.gameObject.GetInstanceID());
    }

    protected void RemoveFoodSource(int ID)
    {
        if (spottedFood.ContainsKey(ID))
            spottedFood.Remove(ID);
    }
    #endregion

    #region Thirst
    protected void AddWaterSource(GameObject water)
    {
        Vector2Int waterCoords = Util.Conversion.Vector3ToVector2Int(water.transform.position);
        spottedWater.Add(water.GetInstanceID(), waterCoords);
    }

    protected bool hasWaterSource()
    {
        return spottedWater.Count > 0;
    }

    protected void setWaterTarget()
    {
        if (hasWaterSource())
        {
            setTarget(getNearestWaterSource());
            foundValidWaterSpot = true;
            return;
        }
        foundValidWaterSpot = false;
        setRandomTarget();
    }
    
    protected Vector2 getNearestWaterSource()
    {
        Vector2 closest = Gamevariables.ERROR_VECTOR2;
        float minDistance = Mathf.Infinity;
        foreach (KeyValuePair<int, Vector2> keyValue in spottedWater)
        {
            float distance = Vector3.Distance(keyValue.Value, transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = keyValue.Value;
            }
        }
        return closest;
    }

    protected void RemoveWaterSource(int ID)
    {
        if (spottedWater.ContainsKey(ID))
            spottedWater.Remove(ID);
    }
    #endregion

    #region Social
    protected bool isValidPartner(GameObject g)
    {
        Creature partner = g.GetComponent<Creature>();
        if (partner == null) return false;
        if (!isSameSpecies(g)) return false;
        return gender != partner.gender;
    }

    protected abstract bool isSameSpecies(GameObject g);
    protected void AddPotentialMate(GameObject mate)
    {
        Vector2Int mateCoords = Util.Conversion.Vector3ToVector2Int(mate.transform.position);
        spottedMate[mate.GetInstanceID()] = mateCoords;
    }

    protected void RemovePotentialMate(int ID)
    {
        if (spottedMate.ContainsKey(ID))
            spottedMate.Remove(ID);
    }
    #endregion

    #endregion

    #region Movement
    protected void MoveToTarget()
    {
        float theoreticalMoves = speed * Gamevariables.MINUTES_PER_TICK + leftOverSteps;
        int moves = (int)theoreticalMoves;
        leftOverSteps = theoreticalMoves - moves;

        for (int i = 0; i < moves; i++)
        {
            //chance to not make a move based on health
            if (Util.Random.Float(0f, 1f) > health / MAX_HEALTH)
                continue;

            //calculate new destination if reached in between ticks
            if (Util.isDestinationReached(transform.position, target))
                setRandomTarget();

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
        Vector2 vect = Util.Conversion.Vector3ToVector2(destination - transform.position);

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

    protected void setRandomTarget()
    {
        target = Util.Random.CoordinateInPlayground();
        nextSteps = Vector2Int.zero;
    }

    public void setTarget(Vector2 destination)
    {
        target = destination;
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

    #region Hunger
    protected void eat(float value)
    {
        hunger = Mathf.Clamp(hunger + value, 0, MAX_HUNGER);
    }
    
    /*
     * subPerHour Default = 7 days without food
     */
    protected void hungerSubtractor(float subPerMinute = .00992f)
    {
        float restingFactor = 1f;
        if (!awake) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        hunger -= subPerMinute * (float)Gamevariables.MINUTES_PER_TICK * restingFactor;
        if (hunger <= 0) death();
    }
    #endregion
    #region Thirst
    protected void drink(float addPerMinute = 20f)
    {
        float add = addPerMinute * (float)Gamevariables.MINUTES_PER_TICK;
        //if (tbm.isWater(GetTile(transform.position)))
        {
            thirst = Mathf.Clamp(thirst + add, 0, MAX_THIRST);
        }
    }

    /*
     * subPerHour Default = 3 days without water
     */
    protected void thirstSubtractor(float subPerMinute = .0231f)
    {
        float restingFactor = 1f;
        if (!awake) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        thirst -= subPerMinute * (float)Gamevariables.MINUTES_PER_TICK * restingFactor;
        if (thirst <= 0) death();
    }
    #endregion
    #region Energy
    /*
     * addPerHour Default = 100% Energy after 8h (without lightfactor)
     * lightScaler -> Scales how much the Light affects the rest
     */
    protected void rest(float addPerMinute = .208f, float lightScaler = 1f)
    {
        addPerMinute /= (1f + lightScaler);
        float addPerTick = addPerMinute * (float)Gamevariables.MINUTES_PER_TICK;
        float lightfactor = lightScaler * addPerTick * (1f - Gamevariables.LIGHT_INTENSITY);

        energy = Mathf.Clamp(energy + addPerTick + lightfactor, 0, MAX_ENERGY);
        if (energy >= MAX_ENERGY    ||  //Full
            energy >= hunger        ||  //Hungry
            energy >= thirst)           //Thirsty
        {
            awake = true;
        }
    }

    /*
     * subPerHour Default = 1 day without sleep (without lightfactor)
     * lightScaler -> Scales how much the Light affects the exhaution
     */
    protected void energySubtractor(float subPerMinute = .0694f, float lightScaler = .25f)
    {
        subPerMinute /= (1f + lightScaler);
        float addPerTick = subPerMinute * (float)Gamevariables.MINUTES_PER_TICK;
        float lightfactor = lightScaler * addPerTick * Gamevariables.LIGHT_INTENSITY;

        energy = Mathf.Clamp(energy - addPerTick - lightfactor, 0, MAX_ENERGY);
        if (energy <= 0) awake = false;
    }
    #endregion

    #endregion

    #region Health

    public void attack(int damage = 20)
    {
        health -= damage;
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

    protected void death()
    {
        GameObject instance = Instantiate(PREFAB_CORPSE, transform.position, transform.rotation);
        Corpse c = instance.GetComponent<Corpse>();
        c.setWeight(weight);
        Destroy(gameObject);
    }

    #endregion

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
