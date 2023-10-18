using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carnivore : IDietary
{
    public void evaluateCreature(GameObject g)
    {
        
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return food.isMeat;
    }
}
