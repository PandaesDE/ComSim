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
    public int Amount { get; private set; } = 1;
    public bool IsMale { get; private set; } = Util.Random.IsMale();
    public bool RandomSpawn { get; private set; } = false;

    private Vector2 _position = Vector2.zero;
    public Vector2 Position
    {
        get
        {
            if (RandomSpawn)
            {
                return RandomPosition;
            }
            else
            {
                return _position;
            }
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
    public SpawnOptions(int amount, bool isRandom)
    {
        this.Amount = amount;
        this.RandomSpawn = isRandom;
    }

    public SpawnOptions SetAmount(int amount)
    {
        this.Amount = amount;
        return this;
    }

    public SpawnOptions SetIsMale(bool isMale)
    {
        this.IsMale = isMale;   
        return this;
    }

    public SpawnOptions SetIsRandom(bool isRandom)
    {
        this.RandomSpawn = isRandom;
        return this;
    }

    public SpawnOptions SetPosition(Vector2 position)
    {
        this._position = position;
        return this;
    }
}
