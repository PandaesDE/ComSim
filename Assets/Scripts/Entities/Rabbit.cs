using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rabbit : Creature
{
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        gender gender = Util.getRandomGender();
        int health = 150;
        int weight = 130;
        float speed = .2f;
        initAttributes(gender, health, weight, speed);
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
