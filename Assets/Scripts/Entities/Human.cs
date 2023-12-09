/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Omnivore, Social Creature
 *  
 *  References:
 *      Scene: Human GameObjects
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

public class Human : Creature
{
    protected override void Awake()
    {
        base.Awake();

        SetAttributes(Gamevariables.HUMAN_ATTRIBUTES);
        BuildGender(Util.Random.Bool());
        BuildSocialBehaviour(new TribeMember(this));
        BuildDietary(new Omnivore(this));
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (StatusManager.Status == StatusManager.State.giving_birth) return;
        if (StatusManager.Status == StatusManager.State.sleeping) return;

        Movement.MoveToTarget();
        EvaluateVision();
        MakeStatusBasedMove();
    }

    public void Alert(Creature toAttack)
    {
        if (StatusManager.Status == StatusManager.State.giving_birth) return;
        if (StatusManager.Status == StatusManager.State.sleeping) return;

        StatusManager.SetState(StatusManager.State.hunting);

        Brain.SetActiveHunt(toAttack);
        Movement.SetMovingTarget(Brain.ActiveHunt.gameObject);
    }

    /*Gets called by Parent*/
    protected override bool IsSameSpecies(Creature c)
    {
        if (!c.TryGetComponent<Human>(out _)) return false;
        return true;
    }

    protected override void GiveBirth(Creature partner)
    {
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
        Spawner.SpawnHumans(options);
    }

    protected override void OnDeath(DeathReason dr)
    {
        Spawner.MakeCorpse(this);
        Statistics.IncrementHumanDeathReason(dr);
    }

    public override Creature BuildGender(bool isMale)
    {
        if (isMale)
        {
            int matingCooldown = 1 * Gamevariables.MINUTES_PER_HOUR * Gamevariables.HOURS_PER_DAY;
            float daysUntilMaxDesire = 3f;
            this.Gender = new Male(this, matingCooldown, daysUntilMaxDesire);
        }
        else
        {
            int durationPregnancy = (int)(1 * (float)Gamevariables.HUMAN_PREGNANCY_TIME_DAYS * (float)Gamevariables.MINUTES_PER_HOUR * (float)Gamevariables.HOURS_PER_DAY);
            int cooldownPregnancy = durationPregnancy / 2;
            this.Gender = new Female(this, cooldownPregnancy, durationPregnancy);
        }
        return this;
    }
}
