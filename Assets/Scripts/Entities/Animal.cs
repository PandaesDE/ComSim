using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Creature
{
    List<System.Type> foodTypes = new()
    {
        typeof(Human)
    };

    protected override void Awake()
    {
        weight = 130;

        base.Awake();
    }

    protected void FixedUpdate()
    {
        if (Util.isDestinationReached(transform.position, target))
        {
            target = Util.getRandomCoordinateInPlayground();
        }
        MoveTowards(target);

        //needs
        needSubtractor();
        drink();
    }

    protected override void initFoodTypes()
    {
        Senses s = transform.GetChild(0).GetComponent<Senses>();
        s.setFoodTypes(foodTypes);
    }

}
