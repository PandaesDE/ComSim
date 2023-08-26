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

    public void setFoodTypes(List<System.Type> foodTypes)
    {
        edibleFoodSources = foodTypes;
    }

    private System.Type getType(GameObject g)
    {
        if (g.GetComponent<Human>() != null)
            return typeof(Human);
            

        if (g.GetComponent<Animal>() != null)
            return typeof(Animal);

        return null;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        /* If self, or Another Vision Collidor -> do nothing*/
        if (other.gameObject == gameObject || other.gameObject.layer == LayerMask.NameToLayer("Vision")) return;

        if (myType == getType(other.gameObject))
        {
            creature.AddPotentialMate(other.gameObject);
            return;
        }

        if (isEdibleFoodSource(other.gameObject))
        {
            creature.AddFoodSource(other.gameObject);
            return;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != gameObject && other.gameObject.layer != LayerMask.NameToLayer("Vision"))
        {
            //Debug.Log("Trigger-Exit:" + other.name);
        }
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
