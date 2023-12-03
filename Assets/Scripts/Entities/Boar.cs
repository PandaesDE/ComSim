/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Omnivore, Strong Creature
 *  
 *  References:
 *      Scene: 
 *          - Boar GameObject
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

public class Boar : Creature
{
    protected override void Awake()
    {
        base.Awake();

        BuildGender(Util.Random.IsMale());
        BuildDietary(new Omnivore(this));
        BuildHealth(150);
        BuildWeight(130);
        BuildDamage(15);
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
        if (!c.TryGetComponent<Boar>(out _)) return false;
        return true;
    }

    protected override void GiveBirth()
    {
        //https://www.researchgate.net/publication/259823483_Birth_rate_and_offspring_survival_in_a_free-ranging_wild_boar_Sus_scrofa_population
        int amount = Util.Random.Int(5, 10);
        SpawnOptions options = new SpawnOptions()
        {
            Amount = amount,
            Position = gameObject.transform.position,
        };
        Spawner.SpawnBoars(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.IncrementBoarDeathReason(dr);
    }

    public override Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            int matingCooldown = 1 * Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY;
            float daysUntilMaxDesire = 3f;
            this.Gender = new Male(matingCooldown, daysUntilMaxDesire);
        }
        else
        {
            //https://de.wikipedia.org/wiki/Wildschwein
            int durationPregnancy = (int)(120 / 270 * (float)Gamevariables.HUMAN_PREGNANCY_TIME_DAYS * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.HOURS_PER_DAY);
            int cooldownPregnancy = durationPregnancy / 2;
            this.Gender = new Female(this, cooldownPregnancy, durationPregnancy);
        }
        return this;
    }
}
