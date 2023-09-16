using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creature
{
    [SerializeField] private Sprite spr_Male;
    [SerializeField] private Sprite spr_Female;

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
    [SerializeField] List<System.Type> foodTypes;

    protected override void Awake()
    {
        gender gender = Util.getRandomGender();
        int health = 150;
        int weight = 130;
        float speed = .2f;

        initAttributes(gender, health, weight, speed);
        initSprite();
        foodTypes = Util.getFoodList(foodType.CARNIVORE, typeof(Human));

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
    protected override void initFoodTypes()
    {
        Senses s = transform.GetChild(0).GetComponent<Senses>();
        s.setFoodTypes(foodTypes);
    }

    private void initSprite()
    {
        if (gender == gender.MALE)
        {
            GetComponent<SpriteRenderer>().sprite = spr_Male;
        } else
        {
            GetComponent<SpriteRenderer>().sprite = spr_Female;
        }
    }



}
