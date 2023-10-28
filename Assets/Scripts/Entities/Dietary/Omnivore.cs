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

    public Creature.Status onAttacked()
    {
        if (creature.health / creature.MAX_HEALTH <= .5f)
        {
            return Creature.Status.FLEEING;
        }

        return Creature.Status.HUNTING;
    }

    public Creature.Status onApproached()
    {
        if (creature.health / creature.MAX_HEALTH <= .8f)
        {
            return Creature.Status.FLEEING;
        }
        if (creature.hunger < 60)
        {
            return Creature.Status.HUNTING;
        }
        return Creature.Status.WANDERING;
    }

    Creature.Status IDietary.onNoFood()
    {
        return Creature.Status.HUNTING;
    }
}
