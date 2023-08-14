using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creature
{

    //private float speed = 5f;

    //[SerializeField] private int age = 0;

    /*
        Idea:
            - int Groupbias: [0,100]
                - chance to stay close to other people (gets inhereted to children)
                - expectation: people in groups are more likely to survive a predator
     */

    protected new void FixedUpdate()
    {
        base.FixedUpdate();

    }


    

}
