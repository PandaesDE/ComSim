/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule N�rnberg
 *  
 *  Description:
 *      - Consumable Creature after it passed
 *  
 *  References:
 *      Scene: 
 *          - Attached on any Creature GameObject after Death
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;

public class Corpse : MonoBehaviour, IConsumable
{
    private static readonly int _s_decayDays = 3;

    private int _weightStart;
    private int _weight = 0;
    private bool _isConsumed = false;

    [SerializeField] private int _decayMinutes;

    public bool IsConsumed
    {
        get
        {
            return _isConsumed;
        }
    }

    public bool IsMeat
    {
        get
        {
            return true;
        }
    }

    public bool HasFood
    {
        get
        {
            return _weight > 0;
        }
    }

    private void Awake()
    {
        _decayMinutes = _s_decayDays * Gamevariables.HOURS_PER_DAY * Gamevariables.MINUTES_PER_HOUR;
    }

    private void FixedUpdate()
    {
        if (_decayMinutes <= 0 || _weight <= 0)
        {
            _isConsumed = true;
            ObjectManager.DeleteCorpse(this);
        }
        _decayMinutes -= Gamevariables.MinutesPerTick;
    }

    public float Consume()
    {
        if (_weight <= 0) return 0;

        int amount = _weightStart / 10;

        if (_weight - amount <= 0) {
            return _weight;
        }

        _weight -= amount;

        return amount;
    }

    //getter & setter
    public void SetWeight(int w)
    {
        this._weight = w;
        this._weightStart = w;
    }


}
