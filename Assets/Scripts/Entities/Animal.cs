using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Creature
{
    protected override void Awake()
    {
        base.Awake();
        weight = 130;
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

}
