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

    public float GrowthFactor
    {
        get
        {
            return Mathf.Clamp(Age / FertilityAge, .05f, 1f);
        }
    }
    


    //Components
        //Information storing and handling
    [SerializeField]protected Brain brain;

        //Sensory of environment
    [SerializeField]protected Senses senses;

        //Handles food, hunt and flee associated behaviour
    [SerializeField]protected IDietary dietary;

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
    public float hunger = 100f;
    public float thirst = 100f;

    protected virtual void Awake()
    {
        tbm = GameObject.Find("Playground").GetComponent<TileBaseManager>();

        _aging = new((int)(Gamevariables.AGE_DAYS_AS_YEARS_CONVERISON * (float)Gamevariables.HOURS_PER_DAY * (float)Gamevariables.MINUTES_PER_HOUR));
        _aging.Reset();
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
        this.dietary ??= dietary;
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
                this.FertilityAge = atr.FertilityAge;
            }
            if (atr.MaxAge != -1)
            {
                this.MaxAge = atr.MaxAge;
            }
            if (atr.Health != -1)
            {
                this._maxHealth = atr.Health;
                this._health = atr.Health;
            }
            if (atr.Weight != -1)
            {
                this._weight = atr.Weight;
            }
            if (atr.Speed != -1)
            {
                this.Movement.Speed = atr.Speed;
            }
            if (atr.Damage != -1)
            {
                this._damage = atr.Damage;
            }
        }
    }

    protected Attributes GetAttributesForBirth()
    {
        return new Attributes()
        {
            MaxAge = this.MaxAge,
            Age = 0,
            FertilityAge = this.FertilityAge,
            Health = this.Health,
            Weight = this.Weight,
            Damage = this.Damage,
            Speed = Movement.Speed
        };
    }
    #endregion

    #region Brain
    protected void EvaluateVision()
    {
        if (StatusManager.Status == StatusManager.State.sleeping) return;
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

    protected virtual void DetermineStatus()
    {
        if (StatusManager.Status == StatusManager.State.sleeping) return;
        if (StatusManager.Status == StatusManager.State.giving_birth) return;


        float desire_cap = IGender.MAX_DESIRE - Gender.Desire;
        float important_cap = 35;
        float normal_cap = 80;

        //Important States
        if (thirst <= important_cap && thirst < hunger)
        {
            StatusManager.SetState(StatusManager.State.dehydrated);
            return;
        }
        if (hunger <= important_cap)
        {
            StatusManager.SetState(StatusManager.State.starving);
            return;
        }

        if (Gender.IsReadyForMating && Gender.IsMale && hunger > desire_cap && thirst > desire_cap)
        {
            StatusManager.SetState(StatusManager.State.looking_for_partner);
            return;
        }

        //Normal States
        if (thirst <= normal_cap && thirst < hunger)
        {
            StatusManager.SetState(StatusManager.State.thirsty);
            return;
        }
        if (hunger <= normal_cap)
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
            Movement.SetRandomTargetIfReached();
            DetermineStatus();
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
                StatusManager.State evaluation = dietary.OnApproached();
                if (evaluation != StatusManager.State.wandering)
                    StatusManager.SetState(evaluation);
            }
            brain.AddSpottedCreature(creature);
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
        if (brain.ActiveFlee == null)
        {
            brain.SetActiveFlee();
        }
        /*Exit Condition*/
        if (brain.ActiveFlee == null) return;
        if (!Util.InRange(gameObject.transform.position, brain.ActiveFlee.transform.position, stopFleeDistance)) return;

        /*Target Too Close*/
        Movement.SetStaticTarget(-brain.ActiveFlee.gameObject.transform.position);
        SocialBehaviour.OnFleeing(brain.ActiveFlee);
    }
    
    protected void OnHunting()
    {
        /*Exit Condition*/
        if (hunger >= MAX_HUNGER)
        {
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (brain.ActiveHunt == null || !Movement.IsFollowing())
        {
            if (brain.HasSpottedCreature())
            {
                brain.SetActiveHunt();
            }

            if (brain.ActiveHunt != null)
            {
                Movement.SetMovingTarget(brain.ActiveHunt.gameObject);
            }
            else
            {
                Movement.SetRandomTargetIfReached();
                return;
            }
        }
        /*Target Reached*/
        if (Util.InRange(transform.position, brain.ActiveHunt.transform.position))
        {
            SocialBehaviour.OnAttacking(brain.ActiveHunt);
            brain.ActiveHunt.Attack(Damage * GrowthFactor, this);
        }
    }

    protected void OnHunger()
    {
        /*Exit Condition*/
        if (hunger >= MAX_HUNGER)
        {
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (brain.ActiveFood == null)
        {
            if (brain.HasFoodSource())
            {
                brain.SetActiveFoodSource();
                if (brain.ActiveFood != null)
                    Movement.SetStaticTarget(brain.ActiveFood.gameObject.transform.position);
                else
                    Movement.SetRandomTargetIfReached();
            }
            else
            {
                StatusManager.SetState(dietary.OnNoFood());
                if (StatusManager.Status == StatusManager.State.hunting)
                    return;
                else
                    Movement.SetRandomTargetIfReached();
            }
        }
        /*Target Reached*/
        if (Movement.TargetReached())
        {
            if (!brain.ActiveFood.HasFood)
            {
                brain.SetInactiveFoodSource(brain.ActiveFood);
                DetermineStatus();
                return;
            }

            Eat(brain.ActiveFood.Consume(Damage));
            return;
        }
    }

    protected void OnThirst()
    {
        /*Exit Condition*/
        if (thirst >= MAX_THIRST)
        {
            //brain.activeWater = null;
            DetermineStatus();
            return;
        }
        /*Set Target*/
        if (brain.ActiveWater == null)
        {
            brain.setActiveWaterSource();
            if (brain.ActiveWater != null)
                Movement.SetStaticTarget(brain.ActiveWater.transform.position);
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
        if (brain.ActiveMate == null || !Movement.IsFollowing())
        {
            brain.SetActiveMate();
            if (brain.ActiveMate != null)
                Movement.SetMovingTarget(brain.ActiveMate.gameObject);
            else
                Movement.SetRandomTargetIfReached();
            return;
        }
        /*Target Reached*/
        if (Util.InRange(transform.position, brain.ActiveMate.transform.position))
        {
            Gender.MateWith(brain.ActiveMate.Gender);
        }
    }

    protected void OnGivingBirth()
    {
        GiveBirth();
        BirthDamage(20);
        StatusManager.SetState(StatusManager.State.wandering);
    }
    #endregion

    #region Survival

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
        hunger = Mathf.Clamp(hunger + value, 0, MAX_HUNGER);
    }
    
    /*
     * subPerHour Default = 7 days without food
     */
    protected void HungerSubtractor(float subPerMinute = .00992f)
    {
        float restingFactor = 1f;
        if (StatusManager.Status == StatusManager.State.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        hunger -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (hunger <= 0) OnDeath(DeathReason.starvation);
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
        if (StatusManager.Status == StatusManager.State.sleeping) restingFactor = .85f; //[4] https://www.ncbi.nlm.nih.gov/pmc/articles/PMC2929498/#:~:text=It%20is%20believed%20that%20during,prolonged%20state%20of%20physical%20inactivity.
        thirst -= subPerMinute * (float)Gamevariables.MinutesPerTick * restingFactor;
        if (thirst <= 0) OnDeath(DeathReason.forthirst);
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
        
        StatusManager.SetState(dietary.OnAttacked());
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

    protected void Regenerate(float addPercentPerHour = .01f)
    {
        float add = addPercentPerHour * _health * (float)Gamevariables.MinutesPerTick / (float)Gamevariables.MINUTES_PER_HOUR;
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
