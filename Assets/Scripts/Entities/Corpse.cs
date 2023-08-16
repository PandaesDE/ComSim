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

    [SerializeField] private int decayTicks;

    private void Awake()
    {
        decayTicks = decayDays * Gamevariables.TICKS_PER_HOUR * Gamevariables.HOURS_PER_DAY;
    }

    void Start()
    {
        
    }

    private void FixedUpdate()
    {
        if (decayTicks <= 0)
        {
            decayed();
        }
        decayTicks--;
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
