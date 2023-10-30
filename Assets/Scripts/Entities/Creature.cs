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

using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    //Constants
    public static readonly int MAX_ENERGY = 100;
    public static readonly float MAX_HUNGER = 100f;
    public static readonly float MAX_THIRST = 100f;

    //Attributes
    public int MAX_HEALTH;

    public float energy { get; protected set; } = 100;
    public float health { get; protected set; }
    public int weight { get; protected set; }
    [SerializeField] public IGender gender;


    //States
    private readonly int MINUTES_UNTIL_STATUS_DETERMINING = 60;
    private int minutes_left_until_status_determining = 0;
    public Status mission { get; protected set; }  = Status.WANDERING;

    //Physics
    protected Rigidbody2D rb2D;

    //Map
    protected TileBaseManager tbm;

    //Brain
    protected Senses senses;
    public IDietary dietary { get; protected set; }
    protected Brain brain;

    //Movement
    protected Movement movement;
    public Movement.Direction facing
    { 
        get
        {
            return movement.facing;
        }
    }

    //Needs
    public float hunger = 100f;
    public float thirst = 100f;

    //Corpse
    [SerializeField] private GameObject PREFAB_CORPSE;

    //Visualization
    public Trail trail { get; private set; }

    public enum Status
    {
        WANDERING,
        SLEEPING,
        HUNTING,
        FLEEING,
        THIRSTY,
        DEHYDRATED,
        HUNGRY,
        STARVING,
        LOOKING_FOR_PARTNER
    }


    protected virtual void Awake()
    {
        rb2D = GetComponent<Rigidbody2D>();
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        senses = new(this);
        brain = new(this);
        movement = new(this);
    }

    protected void Start()
    {
        /*needs to be initialized after Awake*/
        trail = new(this);
    }


    protected virtual void FixedUpdate()
    {
        //Game Logic
        needAdder();
        needSubtractor();

        //Visualization
        trail.FixedUpdate();
    }

    /*  This Method needs to be called by every descenant of this Class!
     */
    protected void initAttributes(IGender gender, IDietary dietary, int health, int weight, float speed)
    {
        this.gender = gender;
        this.dietary = dietary;
        this.MAX_HEALTH = health;
        this.health = health;
        this.weight = weight;
        movement.speed = speed;

    }

    #region Brain
    protected void evaluateVision()
    {
        if (mission == Status.SLEEPING) return;
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
                    brain.AddFoodSource(g);
                    continue;
                }

                /* Creature Evaluation */
                if (isCreature(g))
                {
                    evaluateCreature(g.GetComponent<Creature>());
                    continue;
                }

                /* Water Source*/
                if (tbm.isWater(g))
                {
                    brain.addWaterSource(g);
                    continue;
                }
            }
        }
    }

    protected void determineStatus()
    {
        if (mission == Status.SLEEPING) return;

        int normal_cap = 80;
        int important_cap = 35;

        //Important States
        if (thirst <= important_cap && thirst < hunger)
        {
            mission = Status.DEHYDRATED;
            setWaterTarget();
            return;
        }
        if (hunger <= important_cap)
        {
            mission = Status.STARVING;
            setActiveFoodSource();
            return;
        }


        //Normal States
        if (thirst <= normal_cap && thirst < hunger)
        {
            mission = Status.THIRSTY;
            setWaterTarget();
            return;
        }
        if (hunger <= normal_cap)
        {
            mission = Status.HUNGRY;
            setActiveFoodSource();
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
        else
        {
            minutes_left_until_status_determining -= Gamevariables.MINUTES_PER_TICK;
        }

        if (mission == Status.FLEEING)
        {
            //movement.setTarget(-brain.activeFlee.transform.position);
        }

        if (mission == Status.HUNTING)
        {
            if (brain.activeHunt == null)
            {
                setActiveHunt();
                return;
            }
            if (Util.inRange(transform.position, brain.activeHunt.transform.position))
            {
                if (brain.activeHunt == null)
                {
                    //?
                }
                if (hunger >= MAX_HUNGER)
                {
                    //?
                }

                brain.activeHunt.attack(20);
            }
        }

        if (mission == Status.LOOKING_FOR_PARTNER)
        {

        }


        if (mission == Status.HUNGRY || mission == Status.STARVING)
        {
            if (Util.inRange(transform.position, movement.target))
            {
                if (brain.activeFood == null)
                {
                    determineStatus();
                    return;
                }

                if (!brain.activeFood.hasFood)
                {
                    brain.SetInactiveFoodSource(brain.activeFood);
                    determineStatus();
                    return;
                }

                if (hunger >= MAX_HUNGER)
                {
                    determineStatus();
                    return;
                }

                eat(brain.activeFood.Consume());
                return;
            }
        }

        if (mission == Status.THIRSTY || mission == Status.DEHYDRATED)
        {
            if (Util.inRange(transform.position, movement.target))
            {
                if (thirst >= MAX_THIRST)
                {
                    determineStatus();
                    return;
                }
                //make sure destination reached active water source and not random source
                drink();
            }
        }

        if (mission == Status.WANDERING)
        {
            movement.setRandomTargetIfReached();
            determineStatus();
        }
    }

    private void evaluateCreature(Creature creature)
    {
        if (isSameSpecies(creature))
        {
            if (isValidPartner(creature))
            {
                brain.AddPotentialMate(creature);
            }
        } else
        {
            if (dietary.isInDangerZone(creature))
            {
                Status evaluation = dietary.onApproached();
                if (evaluation != Status.WANDERING)
                    mission = evaluation;
            }
            brain.AddSpottedCreature(creature);
        }
    }
    #region Survival
    protected void setActiveHunt()
    {
        if (brain.hasSpottedCreature())
        {
            brain.setActiveHunt();
        } 

        if (brain.activeHunt != null)
        {
            movement.setTarget(brain.activeHunt.gameObject);
        } else
        {
            movement.setRandomTargetIfReached();
        }
    }
    #endregion

    #region Hunger
    protected void setActiveFoodSource()
    {
        if (brain.HasFoodSource())
        {
            brain.setActiveFoodSource();
        } else
        {
            mission = dietary.onNoFood();
            if (mission == Status.HUNTING)
                setActiveHunt();
            return;
        }
        if (brain.activeFood != null)
            movement.setTarget(brain.activeFood.gameObject);
        else
            movement.setRandomTargetIfReached();
    }

    protected bool isEdibleFoodSource(GameObject g)
    {
        IConsumable food = g.GetComponent<IConsumable>();
        if (food == null) return false;
        return dietary.isEdibleFoodSource(food);
    }
    #endregion
    #region Thirst
    public void setWaterTarget()
    {
        if (brain.hasWaterSource())
        {
            movement.setTarget(brain.getNearestWaterSource());
            return;
        }
        movement.setRandomTargetIfReached();
    }
    #endregion
    #region Social
    protected bool isCreature(GameObject g)
    {
        return g.GetComponent<Creature>() != null;
    }
    protected abstract bool isSameSpecies(Creature g);

    protected bool isValidPartner(Creature partner)
    {
        if (partner == null) return false;
        if (!isSameSpecies(partner)) return false;
        return gender != partner.gender;
    }
    #endregion



    #endregion

    #region Needs
    protected void needAdder()
    {
        regenerate();
        if (mission == Status.SLEEPING)
        {
            rest();
        }
    }
    protected void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
        if (mission != Status.SLEEPING)
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
        if (mission == Status.SLEEPING) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
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
        if (mission == Status.SLEEPING) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
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
            //REGULAR WAKE UP
            brain.ActivateAllInactiveFoodSources();
            mission = Status.WANDERING;
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
        if (energy <= 0) mission = Status.SLEEPING;
    }
    #endregion

    #endregion

    #region Health

    public void attack(int damage = 20)
    {
        health -= damage;
        mission = dietary.onAttacked();
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
        ObjectManager.deleteCreature(this);
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
