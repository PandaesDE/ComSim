/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *     
 *  Description:
 *      - Defines generic functionality of a Creature
 *  
 *  References:
 *      Scene: 
 *          - Any Creature GameObjects
 *      Script:
 *          - 
 *          
 *  Notes:
 *      - 
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    //Constants
    public static readonly int MAX_ENERGY = 100;
    public static readonly float MAX_HUNGER = 100f;
    public static readonly float MAX_THIRST = 100f;



    //Attributes
    public int maxHealth;

    public float Energy { get; protected set; } = MAX_ENERGY;
    public float Health { get; protected set; } = 0;
    public int Weight { get; protected set; } = 0;
    


    //Components
        //Information storing and handling
    [SerializeField]protected Brain brain;

        //Sensory of environment
    [SerializeField]protected Senses senses;

        //Handles food, hunt and flee associated behavior
    [SerializeField]protected IDietary dietary;

        //Handles social associated behavior
    public IGender Gender { get; protected set; }

        //Movement
    public Movement Movement { get; protected set; }

        //Visualization
    public Trail Trail { get; private set; }

        //States
    private Timer _automaticStatusUpdate = new(Gamevariables.MINUTES_PER_HOUR);
    public StatusManager StatusManager { get; protected set; }


    //Map
    protected TileBaseManager tbm;



    //Movement
    public Movement.Direction Facing
    { 
        get
        {
            return Movement.facing;
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

        InitializeCreatureComponents();

        

        void InitializeCreatureComponents()
        {
            senses ??= new(this);
            brain ??= new(this);
            StatusManager ??= new(brain);
            Movement ??= new(this);
        }
    }

    protected void Start()
    {
        /*needs to be initialized after Awake*/
        Trail = new(this, dietary);
    }


    protected virtual void FixedUpdate()
    {
        //Game Logic
        NeedAdder();
        NeedSubtractor();

        //Component updates
            //Gender
        Gender.FixedUpdate();
            //Visualization
        Trail.FixedUpdate();
    }

    #region Builder
    public Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            this.Gender = new Male();
        } else
        {
            this.Gender = new Female(this);
        }
        return this;
    }
    
    protected Creature BuildGender(IGender gender)
    {
        this.Gender ??= gender;
        return this;
    }

    protected Creature BuildDietary(IDietary dietary)
    {
        this.dietary ??= dietary;
        return this;
    }
    protected Creature BuildHealth(int health)
    {
        if (this.Health == 0)
        {
            this.maxHealth = health;
            this.Health = health;
        }
        return this;
    }

    protected Creature BuildWeight(int weight)
    {
        if (this.Weight == 0)
            this.Weight = weight;
        return this;
    }

    protected Creature BuildSpeed(float speed)
    {
        if (this.Movement.speed == 0)
            this.Movement.speed = speed;
        return this;
    }
    #endregion

    #region Brain
    protected void EvaluateVision()
    {
        if (StatusManager.status == StatusManager.Status.sleeping) return;
        Vector2[] visionCoordinates = senses.GetVisionCoordinates();
        for (int i = 0; i < visionCoordinates.Length; i++)
        {
            Collider2D[] overlaps = Physics2D.OverlapPointAll(visionCoordinates[i]);
            for (int overlapIndex = 0; overlapIndex < overlaps.Length; overlapIndex++)
            {
                GameObject g = overlaps[overlapIndex].gameObject;

                /* If self, or Another Vision Collidor -> do nothing*/
                if (g == this.gameObject) return;

                /* Food Source (IConsumable)*/
                if (IsEdibleFoodSource(g))
                {
                    brain.AddFoodSource(g);
                    continue;
                }

                /* Creature Evaluation */
                if (g.TryGetComponent<Creature>(out _))
                {
                    EvaluateCreature(g.GetComponent<Creature>());
                    continue;
                }

                /* Water Source*/
                if (tbm.IsWater(g))
                {
                    brain.addWaterSource(g);
                    continue;
                }
            }
        }
    }

    protected void DetermineStatus()
    {
        if (StatusManager.status == StatusManager.Status.sleeping) return;
        if (StatusManager.status == StatusManager.Status.giving_birth) return;


        int normal_cap = 80;
        int important_cap = 35;

        //Important States
        if (thirst <= important_cap && thirst < hunger)
        {
            StatusManager.SetState(StatusManager.Status.dehydrated);
            return;
        }
        if (hunger <= important_cap)
        {
            StatusManager.SetState(StatusManager.Status.starving);
                return;
        }


        //Normal States
        if (thirst <= normal_cap && thirst < hunger)
        {
            StatusManager.SetState(StatusManager.Status.thirsty);
                    return;
        }
        if (hunger <= normal_cap)
        {
            StatusManager.SetState(StatusManager.Status.hungry);
                        return;
        }

        if (Gender.IsReadyForMating && Gender.IsMale)
        {
            StatusManager.SetState(StatusManager.Status.looking_for_partner);
            return;
        }


        StatusManager.SetState(StatusManager.Status.wandering);
    }

    protected void MakeStatusBasedMove()
    {
        AutomaticStatusDetermination();

        if (StatusManager.status == StatusManager.Status.thirsty || StatusManager.status == StatusManager.Status.dehydrated)
        {
            OnThirst();
            return;
        }

        if (StatusManager.status == StatusManager.Status.hungry || StatusManager.status == StatusManager.Status.starving)
        {
            OnHunger();
            return;
        }

        if (StatusManager.status == StatusManager.Status.fleeing)
        {
            OnFleeing();
            return;
        }

        if (StatusManager.status == StatusManager.Status.hunting)
        {
            OnHunting();
            return;
        }

        if (StatusManager.status == StatusManager.Status.looking_for_partner)
        {
            OnLookingForPartner();
            return;
        }

        if (StatusManager.status == StatusManager.Status.giving_birth)
        {
            OnGivingBirth();
            return;
        }

        if (StatusManager.status == StatusManager.Status.wandering)
        {
            Movement.SetRandomTargetIfReached();
            DetermineStatus();
        }

        void AutomaticStatusDetermination()
        {
            if (_automaticStatusUpdate.Finished())
            {
                DetermineStatus();
                _automaticStatusUpdate.Reset();
                return;
            }
            
            _automaticStatusUpdate.Tick();
        }

        void OnFleeing()
        {
            /*Exit Condition*/
            //TODO
            /*Set Target*/
            //TODO
            /*Target Reached*/
            //TODO

            //movement.setTarget(-brain.activeFlee.transform.position);
        }

        void OnHunting()
        {
            /*Exit Condition*/
            if (hunger >= MAX_HUNGER)
            {
                DetermineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeHunt == null)
            {
                SetActiveHunt();
                return;
            }
            /*Target Reached*/
            if (Util.InRange(transform.position, brain.activeHunt.transform.position))
            {
                brain.activeHunt.Attack(20);
            }
        }

        void OnHunger()
        {
            /*Exit Condition*/
            if (hunger >= MAX_HUNGER)
            {
                DetermineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeFood == null)
            {
                if (brain.HasFoodSource())
                {
                    brain.SetActiveFoodSource();
                    if (brain.activeFood != null)
                        Movement.SetTarget(brain.activeFood.gameObject.transform.position);
                    else
                        Movement.SetRandomTargetIfReached();
                }
                else
                {
                    StatusManager.SetState(dietary.OnNoFood());
                    if (StatusManager.status == StatusManager.Status.hunting)
                        SetActiveHunt();
                    else
                        Movement.SetRandomTargetIfReached();
                }
            }
            /*Target Reached*/
            if (Movement.TargetReached())
            {
                if (!brain.activeFood.HasFood)
                {
                    brain.SetInactiveFoodSource(brain.activeFood);
                    DetermineStatus();
                    return;
                }

                Eat(brain.activeFood.Consume());
                return;
            }
        }

        void OnThirst()
        {
            /*Exit Condition*/
            if (thirst >= MAX_THIRST)
            {
                //brain.activeWater = null;
                DetermineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeWater == null)
            {
                brain.setActiveWaterSource();
                if (brain.activeWater != null)
                    Movement.SetTarget(brain.activeWater.transform.position);
                else
                    Movement.SetRandomTargetIfReached();
                return;
            }
            /*Target Reached*/
            if (Movement.TargetReached())
            {
                Drink();
            }
        }

        void OnLookingForPartner()
        {
            /*Exit Condition*/
            if (!Gender.IsReadyForMating)
            {
                DetermineStatus();
                return;
            }
            /*Set Target*/
            if (brain.activeMate == null)
            {
                brain.SetActiveMate();
                if (brain.activeMate != null)
                    Movement.SetMovingTarget(brain.activeMate.gameObject);
                else
                    Movement.SetRandomTargetIfReached();
                return;
            }
            /*Target Reached*/
            if (Util.InRange(transform.position, brain.activeMate.transform.position))
            {
                brain.activeMate.Gender.MateWith(Gender);
            }
        }

        void OnGivingBirth()
        {
            Health -= 20;
            GiveBirth();
        }
    }

    private void EvaluateCreature(Creature creature)
    {
        if (IsSameSpecies(creature))
        {
            if (IsValidPartner(creature))
            {
                brain.AddPotentialMate(creature);
            }
        } else
        {
            if (dietary.IsInDangerZone(creature))
            {
                StatusManager.Status evaluation = dietary.OnApproached();
                if (evaluation != StatusManager.Status.wandering)
                    StatusManager.SetState(evaluation);
            }
            brain.AddSpottedCreature(creature);
        }
    }
    #region Survival
    protected void SetActiveHunt()
    {
        if (brain.HasSpottedCreature())
        {
            brain.SetActiveHunt();
        } 

        if (brain.activeHunt != null)
        {
            Movement.SetMovingTarget(brain.activeHunt.gameObject);
        }
    }
    #endregion
    #region Hunger

    protected bool IsEdibleFoodSource(GameObject g)
    {
        if (!g.TryGetComponent<IConsumable>(out IConsumable food)) return false;    //if null return false, if not save in food variable
        return dietary.IsEdibleFoodSource(food);
    }
    #endregion
    #region Thirst

    #endregion
    #region Social

    protected abstract bool IsSameSpecies(Creature g);

    protected bool IsValidPartner(Creature partner)
    {
        if (partner == null) return false;
        if (!IsSameSpecies(partner)) return false;
        return Gender.IsMale != partner.Gender.IsMale;
    }

    protected abstract void GiveBirth();
    #endregion



    #endregion

    #region Needs
    protected void NeedAdder()
    {
        Regenerate();
        if (StatusManager.status == StatusManager.Status.sleeping)
        {
            Rest();
        }
    }
    protected void NeedSubtractor()
    {
        HungerSubtractor();
        ThirstSubtractor();
        if (StatusManager.status != StatusManager.Status.sleeping)
        {
            EnergySubtractor();
        }
    }

    #region Hunger
    protected void Eat(float value)
    {
        hunger = Mathf.Clamp(hunger + value, 0, MAX_HUNGER);
    }
    
    /*
     * subPerHour Default = 7 days without food
     */
    protected void HungerSubtractor(float subPerMinute = .00992f)
    {
        float restingFactor = 1f;
        if (StatusManager.status == StatusManager.Status.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        hunger -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (hunger <= 0) Death();
    }
    #endregion
    #region Thirst
    protected void Drink(float addPerMinute = 20f)
    {
        float add = addPerMinute * (float)Gamevariables.MinutesPerTick;
        //if (tbm.isWater(GetTile(transform.position)))
        {
            thirst = Mathf.Clamp(thirst + add, 0, MAX_THIRST);
        }
    }

    /*
     * subPerHour Default = 3 days without water
     */
    protected void ThirstSubtractor(float subPerMinute = .0231f)
    {
        float restingFactor = 1f;
        if (StatusManager.status == StatusManager.Status.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        thirst -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (thirst <= 0) Death();
    }
    #endregion
    #region Energy
    /*
     * addPerHour Default = 100% Energy after 8h (without lightfactor)
     * lightScaler -> Scales how much the Light affects the rest
     */
    protected void Rest(float addPerMinute = .208f, float lightScaler = 1f)
    {
        addPerMinute /= (1f + lightScaler);
        float addPerTick = addPerMinute * (float)Gamevariables.MinutesPerTick;
        float lightfactor = lightScaler * addPerTick * (1f - Gamevariables.LightIntensity);

        Energy = Mathf.Clamp(Energy + addPerTick + lightfactor, 0, MAX_ENERGY);
        if (Energy >= MAX_ENERGY    ||  //Full
            Energy >= hunger        ||  //Hungry
            Energy >= thirst)           //Thirsty
        {
            //REGULAR WAKE UP
            brain.ActivateAllInactiveFoodSources();
            StatusManager.SetState(StatusManager.Status.wandering);
        }
    }

    /*
     * subPerHour Default = 1 day without sleep (without lightfactor)
     * lightScaler -> Scales how much the Light affects the exhaution
     */
    protected void EnergySubtractor(float subPerMinute = .0694f, float lightScaler = .25f)
    {
        subPerMinute /= (1f + lightScaler);
        float addPerTick = subPerMinute * (float)Gamevariables.MinutesPerTick;
        float lightfactor = lightScaler * addPerTick * Gamevariables.LightIntensity;

        Energy = Mathf.Clamp(Energy - addPerTick - lightfactor, 0, MAX_ENERGY);
        if (Energy <= 0) StatusManager.SetState(StatusManager.Status.sleeping);
    }
    #endregion

    #endregion

    #region Health

    public void Attack(int damage = 20)
    {
        Health -= damage;
        StatusManager.SetState(dietary.OnAttacked());
        if (Health <= 0)
        {
            Death();
        }
    }
    protected void Regenerate(float addPercentPerHour = .01f)
    {
        float add = addPercentPerHour * Health * (float)Gamevariables.MinutesPerTick / (float)Gamevariables.MINUTES_PER_HOUR;
        Health = Mathf.Clamp(Health + add, 0, maxHealth);
    }

    protected void Death()
    {
        Spawner.MakeCorpse(this);
    }

    #endregion

}
