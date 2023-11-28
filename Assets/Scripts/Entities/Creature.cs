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

    public float energy { get; protected set; } = MAX_ENERGY;
    public float health { get; protected set; } = 0;
    public int weight { get; protected set; } = 0;
    


    //Components
        //Information storing and handling
    [SerializeField]protected Brain brain;

        //Sensory of environment
    [SerializeField]protected Senses senses;

        //Handles food, hunt and flee associated behavior
    [SerializeField]protected IDietary dietary;

        //Handles social associated behavior
    public IGender gender { get; protected set; }

        //Movement
    public Movement movement { get; protected set; }

        //Visualization
    public Trail trail { get; private set; }

        //States
    private Timer automatic_status_update = new Timer(Gamevariables.MINUTES_PER_HOUR);
    public StatusManager statusManager { get; protected set; }


    //Map
    protected TileBaseManager tbm;



    //Movement
    public Movement.Direction facing
    { 
        get
        {
            return movement.facing;
        }
    }

    //GameObject
    [SerializeField] private Material _material;


    //Needs
    public float hunger = 100f;
    public float thirst = 100f;

    protected virtual void Awake()
    {
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        initializeCreatureComponents();

        

        void initializeCreatureComponents()
        {
            if (senses == null) senses = new(this);
            if (brain == null) brain = new(this);
            if (statusManager == null) statusManager = new(brain);
            if (movement == null) movement = new(this);
        }
    }

    protected void Start()
    {
        /*needs to be initialized after Awake*/
        trail = new(this, dietary);
    }


    protected virtual void FixedUpdate()
    {
        //Game Logic
        needAdder();
        needSubtractor();

        //Component updates
            //Gender
        gender.FixedUpdate();
            //Visualization
        trail.FixedUpdate();
    }

    #region Builder
    public Creature build_Gender(bool isMale)
    {
        if (isMale)
        {
            this.gender = new Male();
        } else
        {
            this.gender = new Female(this);
        }
        return this;
    }
    
    protected Creature build_Gender(IGender gender)
    {

        if (this.gender == null) 
            this.gender = gender;
        return this;
    }

    protected Creature build_Dietary(IDietary dietary)
    {
        if (this.dietary == null)
            this.dietary = dietary;
        return this;
    }
    protected Creature build_Health(int health)
    {
        if (this.health == 0)
        {
            this.MAX_HEALTH = health;
            this.health = health;
        }
        return this;
    }

    protected Creature build_Weight(int weight)
    {
        if (this.weight == 0)
            this.weight = weight;
        return this;
    }

    protected Creature build_Speed(float speed)
    {
        if (this.movement.speed == 0)
            this.movement.speed = speed;
        return this;
    }
    #endregion

    #region Brain
    protected void evaluateVision()
    {
        if (statusManager.status == StatusManager.Status.SLEEPING) return;
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
                if (g.TryGetComponent<Creature>(out _))
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
        if (statusManager.status == StatusManager.Status.SLEEPING) return;
        if (statusManager.status == StatusManager.Status.GIVING_BIRTH) return;


        int normal_cap = 80;
        int important_cap = 35;

        //Important States
        if (thirst <= important_cap && thirst < hunger)
        {
            statusManager.setState(StatusManager.Status.DEHYDRATED);
            return;
        }
        if (hunger <= important_cap)
        {
            statusManager.setState(StatusManager.Status.STARVING);
                return;
        }


        //Normal States
        if (thirst <= normal_cap && thirst < hunger)
        {
            statusManager.setState(StatusManager.Status.THIRSTY);
                    return;
        }
        if (hunger <= normal_cap)
        {
            statusManager.setState(StatusManager.Status.HUNGRY);
                        return;
        }

        if (gender.isReadyForMating && gender.isMale)
        {
            statusManager.setState(StatusManager.Status.LOOKING_FOR_PARTNER);
            return;
        }


        statusManager.setState(StatusManager.Status.WANDERING);
    }

    protected void makeStatusBasedMove()
    {
        automaticStatusDetermination();

        if (statusManager.status == StatusManager.Status.HUNGRY || statusManager.status == StatusManager.Status.STARVING)
        {
            onHunger();
            return;
        }

        if (statusManager.status == StatusManager.Status.THIRSTY || statusManager.status == StatusManager.Status.DEHYDRATED)
        {
            onThirst();
            return;
        }

        if (statusManager.status == StatusManager.Status.FLEEING)
        {
            onFleeing();
            return;
        }

        if (statusManager.status == StatusManager.Status.HUNTING)
        {
            onHunting();
            return;
        }

        if (statusManager.status == StatusManager.Status.LOOKING_FOR_PARTNER)
        {
            onLookingForPartner();
            return;
        }

        if (statusManager.status == StatusManager.Status.GIVING_BIRTH)
        {
            onGivingBirth();
            return;
        }

        if (statusManager.status == StatusManager.Status.WANDERING)
        {
            movement.setRandomTargetIfReached();
            determineStatus();
        }

        void automaticStatusDetermination()
        {
            if (automatic_status_update.finished())
            {
                determineStatus();
                automatic_status_update.reset();
                return;
            }
            
            automatic_status_update.tick();
        }

        void onFleeing()
        {
            /*Exit Condition*/
            //TODO
            /*Set Target*/
            //TODO
            /*Target Reached*/
            //TODO

            //movement.setTarget(-brain.activeFlee.transform.position);
        }

        void onHunting()
        {
            /*Exit Condition*/
            if (hunger >= MAX_HUNGER)
            {
                determineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeHunt == null)
            {
                setActiveHunt();
                return;
            }
            /*Target Reached*/
            if (Util.inRange(transform.position, brain.activeHunt.transform.position))
            {
                brain.activeHunt.attack(20);
            }
        }

        void onHunger()
        {
            /*Exit Condition*/
            if (hunger >= MAX_HUNGER)
            {
                determineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeFood == null)
            {
                setActiveFoodSource();
                return;
            }
            /*Target Reached*/
            if (Util.inRange(transform.position, movement.target))
            {
                if (!brain.activeFood.hasFood)
                {
                    brain.SetInactiveFoodSource(brain.activeFood);
                    determineStatus();
                    return;
                }

                eat(brain.activeFood.Consume());
                return;
            }
        }

        void onThirst()
        {
            /*Exit Condition*/
            if (thirst >= MAX_THIRST)
            {
                //brain.activeWater = null;
                determineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeWater == null)
            {
                brain.setActiveWaterSource();
                if (brain.activeWater != null)
                {
                    movement.setTarget(brain.activeWater);
                }
                return;
            }
            /*Target Reached*/
            if (movement.targetReached())
            {
                if (tbm.isWater(brain.activeWater))
                    drink();
                else
                    brain.setActiveWaterSource();
            }
        }

        void onLookingForPartner()
        {
            /*Exit Condition*/
            if (!gender.isReadyForMating)
            {
                determineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeMate == null)
            {
                setActiveMate();
                return;
            }
            /*Target Reached*/
            if (Util.inRange(transform.position, brain.activeMate.transform.position))
            {
                brain.activeMate.gender.mating(gender);
            }
        }

        void onGivingBirth()
        {
            health -= 20;
            giveBirth();
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
                StatusManager.Status evaluation = dietary.onApproached();
                if (evaluation != StatusManager.Status.WANDERING)
                    statusManager.setState(evaluation);
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
            statusManager.setState(dietary.onNoFood());
            if (statusManager.status == StatusManager.Status.HUNTING)
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

    #endregion
    #region Social
    protected void setActiveMate()
    {

    }

    protected abstract bool isSameSpecies(Creature g);

    protected bool isValidPartner(Creature partner)
    {
        if (partner == null) return false;
        if (!isSameSpecies(partner)) return false;
        return gender.isMale != partner.gender.isMale;
    }

    protected abstract void giveBirth();
    #endregion



    #endregion

    #region Needs
    protected void needAdder()
    {
        regenerate();
        if (statusManager.status == StatusManager.Status.SLEEPING)
        {
            rest();
        }
    }
    protected void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
        if (statusManager.status != StatusManager.Status.SLEEPING)
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
        if (statusManager.status == StatusManager.Status.SLEEPING) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
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
        if (statusManager.status == StatusManager.Status.SLEEPING) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
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
            statusManager.setState(StatusManager.Status.WANDERING);
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
        if (energy <= 0) statusManager.setState(StatusManager.Status.SLEEPING);
    }
    #endregion

    #endregion

    #region Health

    public void attack(int damage = 20)
    {
        health -= damage;
        statusManager.setState(dietary.onAttacked());
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
        Spawner.spawnCorpse(this);
    }

    #endregion

}
