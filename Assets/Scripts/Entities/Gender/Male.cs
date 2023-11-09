using UnityEngine;

public class Male : IGender
{
    private static readonly int COOLDOWN_MATING = 3 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int cooldown_Mating_Remaining = 0;

    public bool isReadyForMating
    {
        get
        {
            return cooldown_Mating_Remaining <= 0;
        }
    }
    
    public bool isMale
    {
        get
        {
            return true;
        }
    }

    public void FixedUpdate()
    {
        if (cooldown_Mating_Remaining > 0)
        {
            cooldown_Mating_Remaining = Mathf.Clamp(cooldown_Mating_Remaining - Gamevariables.MINUTES_PER_TICK, 0, COOLDOWN_MATING);
        }
    }

    public void mating(IGender partner)
    {
        partner.mating(this);
        cooldown_Mating_Remaining = COOLDOWN_MATING;
    }
}
