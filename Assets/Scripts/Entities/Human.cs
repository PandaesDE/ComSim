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
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        health = 100;
        weight = 80;
        foodTypes = Util.getFoodList(foodType.CARNIVORE, typeof(Human));


        base.Awake();
    }

    protected override void FixedUpdate()
    {
        getDestinationIfReached();
        MoveTowards(target);

        //needs
        needSubtractor();
        drink();

        base.FixedUpdate();
    }

    protected override void initFoodTypes()
    {
        Senses s = transform.GetChild(0).GetComponent<Senses>();
        s.setFoodTypes(foodTypes);
    }




}
