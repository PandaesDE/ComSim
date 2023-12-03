/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Object Class to Store SpawnSettings
 *  
 *  References:
 *      Scene:
 *          - 
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

public class SpawnOptions
{
    public int Amount = 1;

    private bool _randomGender = true;
    private bool _isMale = false;
    public bool IsMale
    {
        get
        {
            if (_randomGender)
                return Util.Random.IsMale();
            else
                return _isMale;
        }
        set
        {
            _randomGender = false;
            _isMale = value;
        }
    }

    private bool _randomSpawn = true;
    private Vector2 _position = Vector2.zero;
    public Vector2 Position
    {
        get
        {
            if (_randomSpawn)
            {
                return Util.Random.CoordinateInPlayground();
            }
            else
            {
                return _position;
            }
        }
        set
        {
            _randomSpawn = false;
            _position = value;
        }

    }

    public Vector2 RandomPosition
    {
        get
        {
            return Util.Random.CoordinateInPlayground();
        }
    }

    public SpawnOptions() { }
    public SpawnOptions(int amount, bool randomSpawn, bool randomGender)
    {
        this.Amount = amount;
        this._randomSpawn = randomSpawn;
        this._randomGender = randomGender;
    }
}
