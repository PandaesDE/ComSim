using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Herbivore : IDietary
{
    public void evaluateCreature(GameObject g)
    {

        //AddPotentialDanger(g);

    }

    public bool isEdibleFoodSource(IConsumable food)
    {
        return !food.isMeat;
    }
}
