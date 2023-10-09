using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boar : Creature
{
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        gender gender = Util.getRandomGender();
        int health = 150;
        int weight = 130;
        float speed = .2f;
        initAttributes(gender, health, weight, speed);
        foodTypes = Util.getFoodList(foodType.OMNIVORE, GetType());

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

    /*Gets called by Parent*/
    protected override bool isValidPartner(GameObject g)
    {
        Human partner = g.GetComponent<Human>();

        if (partner == null) return false;
        return isGenericMate(partner);
    }

    protected override bool isEdibleFoodSource(GameObject g)
    {
        if (foodTypes == null)
        {
            Debug.LogError("Creature did not initialize FoodSources");
        }

        foreach (System.Type efs in foodTypes)
        {
            if (g.GetType() == efs)
            {
                return true;
            }
        }
        return false;
    }
}
