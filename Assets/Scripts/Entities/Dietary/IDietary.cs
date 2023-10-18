using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDietary
{
    public bool isEdibleFoodSource(IConsumable food);
    public void evaluateCreature(GameObject g);
}
