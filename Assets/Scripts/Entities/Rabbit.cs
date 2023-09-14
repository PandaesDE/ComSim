using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Creature
{
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        initAttributes(150, 130, .2f);
        foodTypes = Util.getFoodList(foodType.CARNIVORE, typeof(Rabbit));

        base.Awake();
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (awake)
        {
            MoveToTarget();

            drink();
        }
    }

    protected override void initFoodTypes()
    {
        Senses s = transform.GetChild(0).GetComponent<Senses>();
        s.setFoodTypes(foodTypes);
    }

}
