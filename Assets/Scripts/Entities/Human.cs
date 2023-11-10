/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
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
    [SerializeField] private Sprite spr_Male;
    [SerializeField] private Sprite spr_Female;

    public Human(bool isMale)
    {
        if (isMale)
            addGender(new Male());
        else
            addGender(new Female(this, statusManager));

        base.Awake();
    }
    protected override void Awake()
    {
        base.Awake();

        addGender(Util.Random.Gender(this, statusManager));
        addDietary(new Omnivore(this));
        addHealth(80);
        addWeigth(80);
        addSpeed(.2f);

        initSprite();


        void initSprite()
        {
            if (gender.isMale)
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
        if (statusManager.status == StatusManager.Status.GIVING_BIRTH) return;
        if (statusManager.status != StatusManager.Status.SLEEPING)
        {
            movement.MoveToTarget();
            evaluateVision();
            makeStatusBasedMove();
        }
    }

    /*Gets called by Parent*/
    protected override bool isSameSpecies(Creature c)
    {
        Human partner = c.gameObject.GetComponent<Human>();
        if (partner == null) return false;
        return true;
    }

    protected override void giveBirth()
    {
        Spawner.spawnHumans(1, gameObject.transform.position);
    }
}
