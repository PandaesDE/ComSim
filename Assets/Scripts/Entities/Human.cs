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
    List<System.Type> foodTypes = new()
    {
        typeof(Animal)
    };

    protected override void Awake()
    {
        weight = 80;
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
