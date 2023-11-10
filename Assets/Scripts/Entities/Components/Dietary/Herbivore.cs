public class Herbivore : IDietary
{
    private static readonly float dangerZone = 7;
    private Creature creature;

    public Herbivore(Creature creature)
    {
        this.creature = creature;
    }

    public IDietary.Specification specification
    {
        get
        {
            return IDietary.Specification.HERBIVORE;
        }
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return !food.isMeat;
    }

    StatusManager.Status IDietary.onAttacked()
    {
        return StatusManager.Status.FLEEING;
    }

    public bool isInDangerZone(Creature approacher)
    {
        return Util.inRange(creature.gameObject.transform.position, approacher.gameObject.transform.position, dangerZone);
    }

    public StatusManager.Status onApproached()
    {
        return StatusManager.Status.FLEEING;
    }

    public StatusManager.Status onNoFood()
    {
        return creature.statusManager.status;
    }
}
