using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Creature
{
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        health = 150;
        weight = 130;
        speed = .2f;
        foodTypes = Util.getFoodList(foodType.CARNIVORE, typeof(Animal));

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        MoveToTarget();

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
