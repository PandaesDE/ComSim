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

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (decayMinutes <= 0)
        {
            decayed();
        }
        decayMinutes -= Gamevariables.MINUTES_PER_TICK;
    }

    public void decayed()
    {
        Destroy(gameObject);
    }

    public void setWeight(int w)
    {
        this.weight = w;
    }
}
