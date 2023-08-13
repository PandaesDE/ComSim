using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Human : Creature
{

    private float speed = 5f;
    [SerializeField] private float hunger = 100f;
    [SerializeField] private float thirst = 100f;
    //[SerializeField] private int age = 0;

    /*
        Idea:
            - int Groupbias: [0,100]
                - chance to stay close to other people (gets inhereted to children)
                - expectation: people in groups are more likely to survive a predator
     */




    // Update is called once per frame
    protected virtual void Update()
    {
        /*  For Self:
         *  virtual: lets the child class know that this method can be overwritten
         *  has to be at least protected/ or public in both parent and child class
         */
        
        
    }

    protected new void FixedUpdate()
    {
        base.FixedUpdate();
        needSubtractor();
        LogTile(transform.position);
    }



    #region needSubtractor
    private void needSubtractor()
    {
        hungerSubtractor();
        thirstSubtractor();
    }

    protected void hungerSubtractor()
    {
        hunger -= Time.deltaTime;
        if (hunger <= 0) death();
    }

    protected void thirstSubtractor()
    {
        thirst -= Time.deltaTime;
        if (thirst <= 0) death();
    }
    #endregion

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("OTE");
        if (collision.CompareTag(tags.ANIMAL))
        {
            Debug.Log("OTE ANIMAL");
            takeDamage();
        }
    }

}
