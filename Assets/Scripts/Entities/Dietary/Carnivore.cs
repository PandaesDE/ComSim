using UnityEngine;

public class Carnivore : IDietary
{
    private static readonly float dangerZone = 7;

    private Creature creature;

    public Carnivore(Creature creature)
    {
        this.creature = creature;
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return food.isMeat;
    }

    Creature.Status IDietary.onAttacked()
    {
        if (creature.health / creature.MAX_HEALTH <= .2f)
        {
            return Creature.Status.FLEEING;
        }

        return Creature.Status.HUNTING;
    }

    public bool isInDangerZone(Creature approacher)
    {
        return Util.inRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public Creature.Status onApproached()
    {
        if (creature.health / creature.MAX_HEALTH <= .3f)
        {
            return Creature.Status.FLEEING;
        }
        if (creature.hunger < 90)
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
