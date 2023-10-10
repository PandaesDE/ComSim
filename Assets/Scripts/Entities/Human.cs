/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

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

    protected override void Awake()
    {
        gender gender = Util.Random.Gender();
        foodType dietary = foodType.OMNIVORE;
        int health = 80;
        int weight = 80;
        float speed = .2f;

        initAttributes(gender, dietary, health, weight, speed);
        initSprite();

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

            /*if (hunger <= 20)
            {
                IConsumable food = getNearestFoodSource();
                target = food.gameObject.transform.position;
                if (Util.isDestinationReached(transform.position, target))
                {
                    food.Consume();
                }
            }*/
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
        if (g.GetComponent<Human>() != null) return false;
        return isDietaryFitting(g);
    }
}
