/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Carnivore Creature
 *  
 *  References:
 *      Scene: Lion GameObjects
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

using UnityEngine;

public class Lion : Creature
{
    protected override void Awake()
    {
        base.Awake();

        SetAttributes(Gamevariables.LION_ATTRIBUTES);
        BuildGender(Util.Random.Bool());
        BuildSocialBehaviour(new NonSocializer());
        BuildDietary(new Carnivore(this));
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
        if (!c.TryGetComponent<Lion>(out _)) return false;
        return true;
    }

    protected override void GiveBirth(Creature partner)
    {
        //https://nationalzoo.si.edu/animals/lion#:~:text=They%20typically%20give%20birth%20to,eating%20meat%20at%20three%20months.
        int amount = Util.Random.Int(MinBirths, MaxBirths);

        Attributes femaleDNA = GetAttributesForBirth();
        Attributes maleDNA = partner.GetAttributesForBirth();

        SpawnOptions options = new()
        {
            Amount = amount,
            Attributes = MixAttributes(femaleDNA, maleDNA),
            Position = gameObject.transform.position,
        };
        Gender.Children += amount;
        partner.Gender.Children += amount;
        Spawner.SpawnLions(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.IncrementLionDeathReason(dr);
    }

    public override Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            int matingCooldown = 1 * Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY;
            float daysUntilMaxDesire = 1.5f;
            this.Gender = new Male(this, matingCooldown, daysUntilMaxDesire);
        }
        else
        {
            int durationPregnancy = (int)( 110/270 * (float)Gamevariables.HUMAN_PREGNANCY_TIME_DAYS * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.HOURS_PER_DAY);
            int cooldownPregnancy = durationPregnancy / 2;
            this.Gender = new Female(this, cooldownPregnancy, durationPregnancy);
        }
        return this;
    }
}
