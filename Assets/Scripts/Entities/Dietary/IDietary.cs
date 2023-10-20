using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDietary
{
    public bool isEdibleFoodSource(IConsumable food);
    public Creature.Status onAttacked();
    public bool isInDangerZone(Creature creature);
    public Creature.Status onApproached();

}
