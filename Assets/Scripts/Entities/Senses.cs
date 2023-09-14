using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*  *** INFO ***
 *  This Script only Works under following conditions:
 *  - The Layer of the gameObject with the Script needs to be "Vision"
 *  - The Script is on the Child gameObject of the actual Entity
 */

/*  *** Notes ***
 *  https://www.youtube.com/watch?v=xp37Hz1t1Q8
 */

public class Senses : MonoBehaviour
{
    private Creature creature;
    private System.Type myType;
    private List<System.Type> edibleFoodSources;


    private void Awake()
    {
        creature = GetComponentInParent<Creature>();
        myType = getType(transform.parent.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /* If self, or Another Vision Collidor -> do nothing*/
        if (other.gameObject == gameObject || other.gameObject.layer == LayerMask.NameToLayer("Vision")) return;


        /* Potential Partner */
        if (myType == getType(other.gameObject))
        {
            creature.AddPotentialMate(other.gameObject);
            return;
        }

        /* Potential Food */
        if (isEdibleFoodSource(other.gameObject))
        {
            creature.AddFoodSource(other.gameObject);
            return;
        }

        /* Water Source */
        //TODO
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        /* If self, or Another Vision Collidor -> do nothing*/
        if (other.gameObject == gameObject || other.gameObject.layer == LayerMask.NameToLayer("Vision")) return;

        if (true)
        {

        }
    }

    public void setFoodTypes(List<System.Type> foodTypes)
    {
        edibleFoodSources = foodTypes;
    }

    private System.Type getType(GameObject g)
    {
        if (g.GetComponent<Human>() != null)
            return typeof(Human);
            

        if (g.GetComponent<Boar>() != null)
            return typeof(Boar);

        return null;
    }



    private bool isEdibleFoodSource(GameObject g)
    {
        System.Type gType = getType(g);
        if (edibleFoodSources == null)
        {
            Debug.Log("TEST");
        }
        foreach (System.Type efs in edibleFoodSources)
        {
            if (gType == efs)
            {
                return true;
            }
        }
        return false;
    }

}
