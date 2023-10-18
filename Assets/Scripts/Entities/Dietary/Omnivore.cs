using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Omnivore : IDietary
{
    public void evaluateCreature(GameObject g)
    {
        //Add to hunting list
    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return true;
    }
}
