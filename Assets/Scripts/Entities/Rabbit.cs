/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Herbivore Creature
 *  
 *  References:
 *      Scene: Rabbit GameObjects
 *          - 
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

public class Rabbit : Creature
{
    protected override void Awake()
    {
        base.Awake();
        //MaxAge:       https://agriculture.vic.gov.au/livestock-and-animals/animal-welfare-victoria/other-pets/rabbits/owning-a-rabbit#:~:text=Rabbits%20generally%20live%20for%205,care%20for%20them%20that%20long.
        //FertilityAge: https://www.msdvetmanual.com/all-other-pets/rabbits/breeding-and-reproduction-of-rabbits#:~:text=Rabbit%20breeds%20of%20medium%20to,of%20hormones%20as%20in%20humans.

        BuildAge(.3f, 7);
        BuildGender(Util.Random.IsMale());
        BuildDietary(new Herbivore(this));
        BuildHealth(45);
        BuildWeight(30);
        BuildDamage(1);
        BuildSpeed(.2f);
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StatusManager.Status != StatusManager.State.sleeping)
        {
            Movement.MoveToTarget();
            EvaluateVision();
            MakeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool IsSameSpecies(Creature c)
    {
        if (!c.TryGetComponent<Rabbit>(out _)) return false;
        return true;
    }

    protected override void GiveBirth()
    {
        //https://www.rspca.org.uk/adviceandwelfare/pets/rabbits/health/pregnancy#:~:text=Rabbits%20have%20evolved%20to%20reproduce,eight%20kits%20(baby%20rabbits).
        int amount = Util.Random.Int(5, 8);
        SpawnOptions options = new SpawnOptions()
        {
            Amount = amount,
            Position = gameObject.transform.position,
        };
        Spawner.SpawnRabbits(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.IncrementRabbitDeathReason(dr);
    }

    public override Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            int matingCooldown = 1 * Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY;
            float daysUntilMaxDesire = .5f;
            this.Gender = new Male(matingCooldown, daysUntilMaxDesire);
        }
        else
        {
            //https://de.wikipedia.org/wiki/Wildkaninchen
            int durationPregnancy = (int)(30 / 270 * (float)Gamevariables.HUMAN_PREGNANCY_TIME_DAYS * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.HOURS_PER_DAY);
            int cooldownPregnancy = durationPregnancy / 2;
            this.Gender = new Female(this, cooldownPregnancy, durationPregnancy);
        }
        return this;
    }
}
