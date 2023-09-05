using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : Creature
{
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        setHealth(150);
        setWeight(130);
        setSpeed(.2f);
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
