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
 *      - Awake will create a Creature with random Settings.
 *      | To make a custom Creature (e.g Birth) you override after Awake (see Spawner.cs)
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public abstract class Creature : MonoBehaviour
{
    public enum DeathReason
    {
        forthirst,          //thirst
        starvation,         //hunger
        casualty_byHuman,   //attack
        casualty_byBoar,    //attack
        casualty_byLion,    //attack
        maternal,           //birth
        senescence          //age
    }

    public class Attributes
    {
        public float MaxAge = -1;
        public float Age = -1;
        public float FertilityAge = -1;
        public int MinBirths = -1;
        public int MaxBirths = -1;
        public float Health = -1;
        public float Weight = -1;
        public float Damage = -1;
        public float Speed = -1;
    }

    //Constants
    public static readonly int MAX_ENERGY = 100;
    public static readonly float MAX_HUNGER = 100f;
    public static readonly float MAX_THIRST = 100f;



    //Attributes
    private Timer _aging;
    public float MaxAge { get; private set; } = 0;
    public float Age { get; set; } = -1;
    public float FertilityAge { get; private set; } = 0;
    private float _health = 0;
    public float Health { get { return _health * GrowthFactor; } }
    private float _maxHealth = 0;
    public float MaxHealth { get { return _maxHealth * GrowthFactor; } }
    public float Energy { get; private set; } = MAX_ENERGY;
    private float _weight = 0;
    public float Weight { get { return _weight * GrowthFactor; } }
    private float _damage = 0;
    public float Damage { get { return _damage * GrowthFactor; } }
    public int MinBirths { get; protected set; } = 0;
    public int MaxBirths { get; protected set; } = 0;

    public float GrowthFactor
    {
        get
        {
            return Mathf.Clamp(Age / FertilityAge, .05f, 1f);
        }
    }
    


    //Components
        //Information storing and handling
    [SerializeField]protected Brain Brain;

        //Sensory of environment
    [SerializeField]protected Senses Senses;

        //Handles food, hunt and flee associated behaviour
    [SerializeField]protected IDietary Dietary;

        //Handles reproductive behaviour
    public IGender Gender { get; protected set; }
        //Handles social behaviour
    public ISocialBehaviour SocialBehaviour { get; protected set; }

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
            return Movement.Facing;
        }
    }

    //GameObject
    [SerializeField] private Material _material;


    //Needs
    public float Hunger { get; private set; } = 100f;
    public float Thirst { get; private set; } = 100f;

    protected virtual void Awake()
    {
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        _aging = new((int)(Gamevariables.AGE_DAYS_AS_YEARS_CONVERISON * (float)Gamevariables.HOURS_PER_DAY * (float)Gamevariables.MINUTES_PER_HOUR));
        _aging.Reset();
        InitializeCreatureComponents();

        

        void InitializeCreatureComponents()
        {
            Senses ??= new(this);
            Brain ??= new(this);
            StatusManager ??= new(Brain);
            Movement ??= new(this);
        }
    }

    protected void Start()
    {
        /*needs to be initialized after Awake*/
        Trail = new(this, Dietary);
    }


    protected virtual void FixedUpdate()
    {
        //Game Logic
        NeedAdder();
        NeedSubtractor();
        Aging();

        //Component updates
            //Gender
        Gender.FixedUpdate();
            //Visualization
        Trail.FixedUpdate();
    }

    #region Attribute & Components inititialize
    /*  Here defined as:
     *      Build: Set without overwriting the variable(s)
     *      Set: override the variable(s)
     */
    public abstract Creature BuildGender(bool isMale);
    
    protected Creature BuildGender(IGender gender)
    {
        this.Gender ??= gender;
        return this;
    }

    protected Creature BuildSocialBehaviour(ISocialBehaviour socialBehaviour)
    {
        this.SocialBehaviour ??= socialBehaviour;
        return this;
    }

    protected Creature BuildDietary(IDietary dietary)
    {
        this.Dietary ??= dietary;
        return this;
    }

    public void SetAttributes(Attributes atr)
    {
        SetAllValuesIfSpecified();
        if (this.Age == -1)
        {
            this.Age = Util.Random.Float(0, MaxAge);
        }

        void SetAllValuesIfSpecified()
        {
            if (atr.Age != -1)
            {
                this.Age = atr.Age;
            }
            if (atr.FertilityAge != -1)
            {
                this.FertilityAge = ApplyEvolutionRate(atr.FertilityAge);
            }
            if (atr.MaxAge != -1)
            {
                this.MaxAge = ApplyEvolutionRate(atr.MaxAge);
            }
            if (atr.Health != -1)
            {
                this._maxHealth = ApplyEvolutionRate(atr.Health);
                this._health = _maxHealth;
            }
            if (atr.Weight != -1)
            {
                this._weight = ApplyEvolutionRate(atr.Weight);
            }
            if (atr.Speed != -1)
            {
                this.Movement.Speed = atr.Speed; //experimental -> no evultion
            }
            if (atr.Damage != -1)
            {
                this._damage = ApplyEvolutionRate(atr.Damage);
            }
            if (atr.MinBirths != -1)
            {
                this.MinBirths = atr.MinBirths;
            }
            if (atr.MaxBirths != -1)
            {
                this.MaxBirths = atr.MaxBirths;
            }
        }

        float ApplyEvolutionRate(float val)
        {
            float dif = val * Gamevariables.EVOLUTION_RATE;
            float r = Util.Random.Float(val - dif, val + dif);
            if (r <= 0) r = .01f; //worst possible value
            return r;
        }

    }

    public Attributes GetAttributesForBirth()
    {
        return new Attributes()
        {
            MaxAge = this.MaxAge,
            Age = 0,
            FertilityAge = this.FertilityAge,
            Health = this.Health,
            Weight = this.Weight,
            Damage = this.Damage,
            Speed = Movement.Speed,
            MinBirths = this.MinBirths,
            MaxBirths = this.MaxBirths,
        };
    }

    protected Attributes MixAttributes(Attributes mother, Attributes father)
    {
        return new Attributes()
        {
            MaxAge =  DecideBetweenValues(mother.MaxAge, father.MaxAge),
            Age = mother.Age,
            FertilityAge = DecideBetweenValues(mother.FertilityAge, father.FertilityAge),
            MinBirths = mother.MinBirths,
            MaxBirths = mother.MaxBirths,
            Health = DecideBetweenValues(mother.Health, father.Health),
            Weight = DecideBetweenValues(mother.Weight, father.Weight),
            Damage = DecideBetweenValues(mother.Damage, father.Damage),
            Speed = DecideBetweenValues(mother.Speed, father.Speed),
        };

        float DecideBetweenValues(float x, float y)
        {
            //both values set
            if (x != -1 && y != -1)
            {
                if (Util.Random.Bool()) return x;
                else return y;
            }
            //one value set
            if (x == -1) return y;
            if (y == -1) return x;

            //no value set
            return -1;
        }
    }
    #endregion

    #region Brain
    protected void EvaluateVision()
    {
        if (StatusManager.Status == StatusManager.State.sleeping) return;
        Vector2[] visionCoordinates = Senses.GetVisionCoordinates();
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
                    Brain.AddFoodSource(g);
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
                    Brain.addWaterSource(g);
                    continue;
                }
            }
        }
    }

    protected virtual void DetermineStatus()
    {
        if (StatusManager.Status == StatusManager.State.sleeping) return;
        if (StatusManager.Status == StatusManager.State.giving_birth) return;


        float desire_cap = IGender.MAX_DESIRE - Gender.Desire;
        float important_cap = 35;
        float normal_cap = 80;

        //Important States
        if (Thirst <= important_cap && Thirst < Hunger)
        {
            StatusManager.SetState(StatusManager.State.dehydrated);
            return;
        }
        if (Hunger <= important_cap)
        {
            StatusManager.SetState(StatusManager.State.starving);
            return;
        }

        if (Gender.IsReadyForMating && Gender.IsMale && Hunger > desire_cap && Thirst > desire_cap)
        {
            StatusManager.SetState(StatusManager.State.looking_for_partner);
            return;
        }

        //Normal States
        if (Thirst <= normal_cap && Thirst < Hunger)
        {
            StatusManager.SetState(StatusManager.State.thirsty);
            return;
        }
        if (Hunger <= normal_cap)
        {
            StatusManager.SetState(StatusManager.State.hungry);
            return;
        }

        StatusManager.SetState(StatusManager.State.wandering);
    }

    protected virtual void MakeStatusBasedMove()
    {
        AutomaticStatusDetermination();

        if (StatusManager.Status == StatusManager.State.thirsty || StatusManager.Status == StatusManager.State.dehydrated)
        {
            OnThirst();
            return;
        }

        if (StatusManager.Status == StatusManager.State.hungry || StatusManager.Status == StatusManager.State.starving)
        {
            OnHunger();
            return;
        }

        if (StatusManager.Status == StatusManager.State.fleeing)
        {
            OnFleeing();
            return;
        }

        if (StatusManager.Status == StatusManager.State.hunting)
        {
            OnHunting();
            return;
        }

        if (StatusManager.Status == StatusManager.State.looking_for_partner)
        {
            OnLookingForPartner();
            return;
        }

        if (StatusManager.Status == StatusManager.State.giving_birth)
        {
            OnGivingBirth();
            return;
        }

        if (StatusManager.Status == StatusManager.State.wandering)
        {
            OnWandering();
        }
    }

    private void EvaluateCreature(Creature creature)
    {
        if (IsSameSpecies(creature))
        {
            if (IsValidPartner(creature))
            {
                Brain.AddPotentialMate(creature);
            }
        } else
        {
            if (Dietary.IsInDangerZone(creature))
            {
                StatusManager.State evaluation = Dietary.OnApproached();
                if (evaluation != StatusManager.State.wandering)
                    StatusManager.SetState(evaluation);
            }
            Brain.AddSpottedCreature(creature);
        }
    }
    #region State Machine Actions
    protected void AutomaticStatusDetermination()
    {
        if (_automaticStatusUpdate.Finished())
        {
            DetermineStatus();
            _automaticStatusUpdate.Reset();
            return;
        }

        _automaticStatusUpdate.Tick();
    }

    protected void OnFleeing()
    {
        int stopFleeDistance = 15;
        /*Set Target*/
        if (Brain.ActiveFlee == null)
        {
            Brain.SetActiveFlee();
        }

        if (Brain.ActiveFlee == null) return;

        /*Exit Condition*/
        if (Util.InRange(gameObject.transform.position, Brain.ActiveFlee.transform.position, stopFleeDistance))
        {
            StatusManager.SetState(StatusManager.State.wandering);
        } else
        {
            /*Target Too Close*/
            Movement.SetStaticTarget(-Brain.ActiveFlee.gameObject.transform.position);
            SocialBehaviour.OnFleeing(Brain.ActiveFlee);
        }
    }
    
    protected void OnHunting()
    {
        /*Exit Condition*/
        if (Hunger >= MAX_HUNGER)
        {
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (Brain.ActiveHunt == null || !Movement.IsFollowing())
        {
            if (Brain.HasSpottedCreature())
            {
                Brain.SetActiveHunt();
            }

            if (Brain.ActiveHunt != null)
            {
                Movement.SetMovingTarget(Brain.ActiveHunt.gameObject);
            }
            else
            {
                Movement.SetRandomTargetIfReached();
                return;
            }
        }
        /*Target Reached*/
        if (Util.InRange(transform.position, Brain.ActiveHunt.transform.position))
        {
            SocialBehaviour.OnAttacking(Brain.ActiveHunt);
            Brain.ActiveHunt.Attack(Damage * GrowthFactor, this);
        }
    }

    protected void OnHunger()
    {
        /*Exit Condition*/
        if (Hunger >= MAX_HUNGER)
        {
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (Brain.ActiveFood == null)
        {
            if (Brain.HasFoodSource())
            {
                Brain.SetActiveFoodSource();
                if (Brain.ActiveFood != null)
                    Movement.SetStaticTarget(Brain.ActiveFood.gameObject.transform.position);
                else
                    Movement.SetRandomTargetIfReached();
            }
            else
            {
                StatusManager.SetState(Dietary.OnNoFood());
                if (StatusManager.Status == StatusManager.State.hunting)
                    return;
                else
                    Movement.SetRandomTargetIfReached();
            }
        }
        /*Target Reached*/
        if (Movement.TargetReached())
        {
            if (!Brain.ActiveFood.HasFood)
            {
                Brain.SetInactiveFoodSource(Brain.ActiveFood);
                DetermineStatus();
                return;
            }

            Eat(Brain.ActiveFood.Consume(Damage));
            return;
        }
    }

    protected void OnThirst()
    {
        /*Exit Condition*/
        if (Thirst >= MAX_THIRST)
        {
            //brain.activeWater = null;
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (Brain.ActiveWater == null)
        {
            Brain.setActiveWaterSource();
            if (Brain.ActiveWater != null)
                Movement.SetStaticTarget(Brain.ActiveWater.transform.position);
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

    protected void OnLookingForPartner()
    {
        /*Exit Condition*/
        if (!Gender.IsReadyForMating)
        {
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (Brain.ActiveMate == null || !Movement.IsFollowing())
        {
            Brain.SetActiveMate();
            if (Brain.ActiveMate != null)
                Movement.SetMovingTarget(Brain.ActiveMate.gameObject);
            else
                Movement.SetRandomTargetIfReached();
            return;
        }
        /*Target Reached*/
        if (Util.InRange(transform.position, Brain.ActiveMate.transform.position))
        {
            Gender.MateWith(Brain.ActiveMate.Gender);
        }
    }

    protected void OnGivingBirth()
    {
        GiveBirth(Gender.Partner);
        Gender.Partner = null;  
        BirthDamage(20);
        StatusManager.SetState(StatusManager.State.wandering);
    }

    protected void OnWandering()
    {
        Movement.SetStaticTargetIfReached(SocialBehaviour.GetHomeArea());
        DetermineStatus();
    }
    #endregion

    #region Survival

    #endregion
    #region Hunger

    protected bool IsEdibleFoodSource(GameObject g)
    {
        if (!g.TryGetComponent<IConsumable>(out IConsumable food)) return false;    //if null return false, if not save in food variable
        return Dietary.IsEdibleFoodSource(food);
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

    protected abstract void GiveBirth(Creature male);
    #endregion



    #endregion

    #region Needs
    protected void NeedAdder()
    {
        Regenerate();
        if (StatusManager.Status == StatusManager.State.sleeping)
        {
            Rest();
        }
    }
    protected void NeedSubtractor()
    {
        HungerSubtractor();
        ThirstSubtractor();
        if (StatusManager.Status != StatusManager.State.sleeping)
        {
            EnergySubtractor();
        }
    }

    #region Hunger
    protected void Eat(float value)
    {
        Hunger = Mathf.Clamp(Hunger + value, 0, MAX_HUNGER);
    }
    
    /*
     * subPerHour Default = 7 days without food
     */
    protected void HungerSubtractor(float subPerMinute = .00992f)
    {
        float restingFactor = 1f;
        if (StatusManager.Status == StatusManager.State.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        Hunger -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (Hunger <= 0) OnDeath(DeathReason.starvation);
    }
    #endregion
    #region Thirst
    protected void Drink(float addPerMinute = 20f)
    {
        float add = addPerMinute * (float)Gamevariables.MinutesPerTick;
        //if (tbm.isWater(GetTile(transform.position)))
        {
            Thirst = Mathf.Clamp(Thirst + add, 0, MAX_THIRST);
        }
    }

    /*
     * subPerMinute Default = 3 days without water
     */
    protected void ThirstSubtractor(float subPerMinute = .0231f)
    {
        float restingFactor = 1f;
        if (StatusManager.Status == StatusManager.State.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        Thirst -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (Thirst <= 0) OnDeath(DeathReason.forthirst);
    }
    #endregion
    #region Energy
    /*
     * addPerMinute Default = 100% Energy after 8h (without lightfactor)
     * lightScaler -> Scales how much the Light affects the rest
     */
    protected void Rest(float addPerMinute = .208f, float lightScaler = 1f)
    {
        addPerMinute /= (1f + lightScaler);
        float addPerTick = addPerMinute * (float)Gamevariables.MinutesPerTick;
        float lightfactor = lightScaler * addPerTick * (1f - Gamevariables.LightIntensity);

        Energy = Mathf.Clamp(Energy + addPerTick + lightfactor, 0, MAX_ENERGY);
        if (Energy >= MAX_ENERGY    ||  //Full
            Energy >= Hunger        ||  //Hungry
            Energy >= Thirst)           //Thirsty
        {
            //REGULAR WAKE UP
            Brain.ActivateAllInactiveFoodSources();
            StatusManager.SetState(StatusManager.State.wandering);
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
        if (Energy <= 0) StatusManager.SetState(StatusManager.State.sleeping);
    }
    #endregion

    #endregion

    #region Health

    public void Attack(float damage, Creature attacker)
    {
        _health -= damage;
        if (Health <= 0)
        {
            if (attacker.TryGetComponent<Lion>(out _))
                OnDeath(DeathReason.casualty_byLion);
            if (attacker.TryGetComponent<Boar>(out _))
                OnDeath(DeathReason.casualty_byBoar);
            if (attacker.TryGetComponent<Human>(out _))
                OnDeath(DeathReason.casualty_byHuman);
        }
        
        StatusManager.SetState(Dietary.OnAttacked());
        SocialBehaviour.OnAttacked(attacker);
    }

    private void BirthDamage(float birthDamage)
    {
        _health -= birthDamage;
        if (Health <= 0)
        {
            OnDeath(DeathReason.maternal);
        }
    }

    protected void Regenerate(float addPercentPerHour = .05f)
    {
        float add = addPercentPerHour * _maxHealth * (float)Gamevariables.MinutesPerTick / (float)Gamevariables.MINUTES_PER_HOUR;
        _health = Mathf.Clamp(_health + add, 0, _maxHealth);
    }

    protected abstract void OnDeath(DeathReason dr);


    #endregion

    private void Aging()
    {
        if (_aging.Finished())
        {
            if (Age == MaxAge)
            {
                OnDeath(DeathReason.senescence);
                return;
            }

            Age++;
            _aging.Reset();
        }
        _aging.Tick();

        if (Age <= FertilityAge) //cannot used GrowthFactor, since it's capped to one
        {
            float scale = Mathf.Clamp(GrowthFactor, .25f, 1);
            transform.localScale = new Vector3(scale, scale, scale);
        }
    }

}
