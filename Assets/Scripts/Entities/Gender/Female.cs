using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Female : IGender
{
    private Creature creature;
    private StatusManager sm;

    private static readonly int DURATION_PREGNANCY = 18 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int duration_Pregnancy_Remaining = 0;

    private static readonly int COOLDOWN_PREGNANCY = 9 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int cooldown_Pregnancy_Remaining = 0;
    private bool isPregnant = false;

    public bool isReadyForMating
    {
        get
        {
            return  !isPregnant &&
                    cooldown_Pregnancy_Remaining <= 0;

        }
    }

    public bool isMale
    {
        get
        {
            return false;
        }
    }

    public Female(Creature creature, StatusManager sm)
    {
        this.creature = creature;
        this.sm = sm;
    }

    public void FixedUpdate()
    {
        if (isPregnant)
        {
            if (duration_Pregnancy_Remaining > 0)
            {
                duration_Pregnancy_Remaining = Mathf.Clamp(duration_Pregnancy_Remaining - Gamevariables.MINUTES_PER_TICK, 0, DURATION_PREGNANCY);
            } else
            {
                sm.status = StatusManager.Status.GIVING_BIRTH;
            }
        } else
        {
            if (cooldown_Pregnancy_Remaining > 0)
            {  
                cooldown_Pregnancy_Remaining = Mathf.Clamp(cooldown_Pregnancy_Remaining - Gamevariables.MINUTES_PER_TICK, 0, COOLDOWN_PREGNANCY);
            }
        }
    }

    public void mating(IGender partner)
    {
        if (!isSuitable(partner)) return;
        //chance of failure ?
        isPregnant = true;
        duration_Pregnancy_Remaining = DURATION_PREGNANCY;
        cooldown_Pregnancy_Remaining = COOLDOWN_PREGNANCY;
    }

    private bool isSuitable(IGender partner)
    {
        return isMale != partner.isMale;
    }


}
