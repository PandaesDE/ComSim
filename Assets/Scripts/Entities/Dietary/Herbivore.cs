using UnityEngine;

public class Herbivore : IDietary
{
    private static readonly float dangerZone = 7;

    private Creature creature;

    public Herbivore(Creature creature)
    {
        this.creature = creature;
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return !food.isMeat;
    }

    Creature.Status IDietary.onAttacked()
    {
        return Creature.Status.FLEEING;
    }

    public bool isInDangerZone(Creature approacher)
    {
        return Util.inRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public Creature.Status onApproached()
    {
        return Creature.Status.FLEEING;
    }
}
