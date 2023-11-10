using UnityEngine;

public class Carnivore : IDietary
{
    private static readonly float dangerZone = 7;
    private Creature creature;



    public Carnivore(Creature creature)
    {
        this.creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.CARNIVORE;
        }
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return food.isMeat;
    }

    public StatusManager.Status onAttacked()
    {
        if (creature.health / creature.MAX_HEALTH <= .2f)
        {
            return StatusManager.Status.FLEEING;
        }

        return StatusManager.Status.HUNTING;
    }

    public bool isInDangerZone(Creature approacher)
    {
        return Util.inRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public StatusManager.Status onApproached()
    {
        if (creature.health / creature.MAX_HEALTH <= .3f)
        {
            return StatusManager.Status.FLEEING;
        }
        if (creature.hunger < 90)
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
