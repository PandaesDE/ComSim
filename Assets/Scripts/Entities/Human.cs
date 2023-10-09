using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creature
{
    //[SerializeField] private int age = 0;

    /*
        Idea:
            - int Groupbias: [0,100]
                - chance to stay close to other people (gets inhereted to children)
                - expectation: people in groups are more likely to survive a predator

        TODO:
            - MAKE FEMALE/ MALE CLASS FOR ALL FOR SEARCHING MATES, PREGNANCY ETC
            - ONE HUMAN PREFAB
            - ON INSTANTIATION SETS SPRITE
     */

    [SerializeField] private Sprite spr_Male;
    [SerializeField] private Sprite spr_Female;

    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        gender gender = Util.getRandomGender();
        int health = 80;
        int weight = 80;
        float speed = .2f;

        initAttributes(gender, health, weight, speed);
        initSprite();
        foodTypes = Util.getFoodList(foodType.OMNIVORE, GetType());

        base.Awake();

        void initSprite()
        {
            if (gender == gender.MALE)
            {
                GetComponent<SpriteRenderer>().sprite = spr_Male;
            }
            else
            {
                GetComponent<SpriteRenderer>().sprite = spr_Female;
            }
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        if (awake)
        {
            MoveToTarget();
            evaluateVision();
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
