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

    private void Awake()
    {
        creature = GetComponentInParent<Creature>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject != gameObject && other.gameObject.layer != LayerMask.NameToLayer("Vision"))
        {
            //Debug.Log("Trigger-Enter:" + other.name);

        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject != gameObject && other.gameObject.layer != LayerMask.NameToLayer("Vision"))
        {
            //Debug.Log("Trigger-Exit:" + other.name);
        }
    }

}
