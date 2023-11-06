using UnityEngine;

public class Omnivore : IDietary
{
    private static readonly float dangerZone = 5;
    private Creature creature;

    public Omnivore(Creature creature)
    {
        this.creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.OMNIVORE;
        }
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return true;
    }

    public bool isInDangerZone(Creature approacher)
    {
        return Util.inRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public StatusManager.Status onAttacked()
    {
        if (creature.health / creature.MAX_HEALTH <= .5f)
        {
            return StatusManager.Status.FLEEING;
        }

        return StatusManager.Status.HUNTING;
    }

    public StatusManager.Status onApproached()
    {
        if (creature.health / creature.MAX_HEALTH <= .8f)
        {
            return StatusManager.Status.FLEEING;
        }
        if (creature.hunger < 60)
        {
            return StatusManager.Status.HUNTING;
        }
        return StatusManager.Status.WANDERING;
    }

    StatusManager.Status IDietary.onNoFood()
    {
        return StatusManager.Status.HUNTING;
    }
}
