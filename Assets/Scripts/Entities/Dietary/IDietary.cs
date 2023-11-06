using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDietary
{
    public enum Specification {
        OMNIVORE,
        CARNIVORE,
        HERBIVORE
    }
    public Specification specification { get; }
    public bool isEdibleFoodSource(IConsumable food);
    public bool isInDangerZone(Creature creature);
    public StatusManager.Status onNoFood();
    public StatusManager.Status onAttacked();
    public StatusManager.Status onApproached();

}
