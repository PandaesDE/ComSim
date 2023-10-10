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

public class Corpse : MonoBehaviour, IConsumable
{
    /*  This script is disabled by default and will be enabled once a Creature passed
     * 
     */

    private static readonly int decayDays = 5;

    private int WEIGHT_START;
    private int weight = 0;
    private bool consumed = false;

    [SerializeField] private int decayMinutes;

    public bool isMeat
    {
        get
        {
            return true;
        }
    }

    private void Awake()
    {
        decayMinutes = decayDays * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    }

    private void FixedUpdate()
    {
        if (decayMinutes <= 0 || weight <= 0 || consumed)
        {
            Destroy(gameObject);
        }
        decayMinutes -= Gamevariables.MINUTES_PER_TICK;
    }

    public int Consume()
    {
        if (consumed) return 0;

        int amount = WEIGHT_START / 10;

        if (weight - amount <= 0) {
            consumed = true;
            return weight;
        }

        weight -= amount;

        return amount;
    }

    //getter & setter
    public void setWeight(int w)
    {
        this.weight = w;
        this.WEIGHT_START = w;
    }


}
