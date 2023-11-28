using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Female : IGender
{
    private Creature creature;

    private Timer duration_Pregnancy = new Timer(18 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);
    private Timer cooldown_Pregnancy = new Timer(9 * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR);
    private bool isPregnant = false;

    public bool isReadyForMating
    {
        get
        {
            return  !isPregnant &&
                    cooldown_Pregnancy.finished();

        }
    }

    public bool isMale
    {
        get
        {
            return false;
        }
    }

    public Female(Creature creature)
    {
        this.creature = creature;
    }

    public void FixedUpdate()
    {
        if (isPregnant)
        {
            if (!duration_Pregnancy.finished())
            {
                duration_Pregnancy.tick();
            } else
            {

                creature.statusManager.setState(StatusManager.Status.GIVING_BIRTH);
            }
        } else
        {
            if (!cooldown_Pregnancy.finished())
            {
                cooldown_Pregnancy.tick();
            }
        }
    }

    public void mating(IGender partner)
    {
        if (!isSuitable(partner)) return;
        //chance of failure ?
        isPregnant = true;
        duration_Pregnancy.reset();
        cooldown_Pregnancy.reset();
    }

    private bool isSuitable(IGender partner)
    {
        return isMale != partner.isMale;
    }


}
