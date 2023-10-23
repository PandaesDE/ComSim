using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDietary
{
    public bool isEdibleFoodSource(IConsumable food);
    public bool isInDangerZone(Creature creature);
    public Creature.Status onNoFood();
    public Creature.Status onAttacked();
    public Creature.Status onApproached();

}
