using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corpse : MonoBehaviour
{
    /*  This script is disabled by default and will be enabled once a Creature passed
     * 
     */

    private int decayDays = 5;
    private int weight = 0;

    [SerializeField] private int decayMinutes;

    private void Awake()
    {
        decayMinutes = decayDays * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    }

    private void FixedUpdate()
    {
        if (decayMinutes <= 0 || weight <= 0)
        {
            destroyed();
        }
        decayMinutes -= Gamevariables.MINUTES_PER_TICK;
    }

    public int getsConsumed(int amount)
    {
        weight -= amount;

        if (weight <= 0) {
            StartCoroutine(destroyedBeforeNextFixedUpdate());
            return amount + weight;
        }

        return amount;
    }

    private void destroyed()
    {
        Destroy(gameObject);
    }

    private IEnumerator destroyedBeforeNextFixedUpdate()
    {
        /*1/(Gamevariables.TICKRATE+1) -> so that it waits just before the next fixedUpdate*/
        yield return new WaitForSeconds(1/(Gamevariables.TICKRATE+1));
        Destroy(gameObject);
    }


    //getter & setter
    public void setWeight(int w)
    {
        this.weight = w;
    }
}
