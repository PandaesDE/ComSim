using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Female : IGender
{
    private Creature creature;
    private StatusManager sm;
    private Spawner spawner;

    private static readonly int DURATION_PREGNANCY = 18 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int duration_Pregnancy_remaining = 0;

    private static readonly int COOLDOWN_PREGNANCY = 9 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    private int cooldown_Pregnancy_Remaining = 0;
    private bool isPregnant = false;

    public bool isReadyForMating
    {
        get
        {
            if (isPregnant ||
                cooldown_Pregnancy_Remaining > 0)
                return false;

            return true;
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
        spawner = GameObject.Find("SystemNode").GetComponent<Spawner>();
    }

    public void FixedUpdate()
    {
        sm.status = StatusManager.Status.GIVING_BIRTH;
        if (isPregnant)
        {
            if (duration_Pregnancy_remaining > 0)
            {
                duration_Pregnancy_remaining -= Gamevariables.MINUTES_PER_TICK;
            } else
            {
                duration_Pregnancy_remaining = 0;
                sm.status = StatusManager.Status.GIVING_BIRTH;
            }
        } else
        {
            if (cooldown_Pregnancy_Remaining > 0)
            {
                cooldown_Pregnancy_Remaining -= Gamevariables.MINUTES_PER_TICK;
            }

        }
    }

    public void mating()
    {
        
    }


}
