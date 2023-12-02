/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Class to produce Berries for Bush GameObjects
 *  
 *  References:
 *      Scene: 
 *          - Attatched to Bush GameObject Tiles
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

public class Bush : MonoBehaviour, IConsumable
{
    private static readonly float NUTRITION_PER_BERRY = .5f;
    private static readonly int MAX_BERRIES = 80;
    private static readonly int BERRY_GROW_TIME_MINUTES = 288; //5 Berries per Day

    public bool IsConsumed
    {
        get
        {
            return false;
        }
    }
    public bool IsMeat
    {
        get
        {
            return false;
        }
    }

    public bool HasFood
    {
        get
        {
            return _berries > 0;
        }
    }

    [SerializeField] private int _berry_grow_minutes = 0;
    [SerializeField] private int _berries;


    private void Awake()
    {
        _berries = Util.Random.Int(MAX_BERRIES);
    }

    private void Start()
    {
        AdjustColor();
    }

    private void FixedUpdate()
    {
        if (_berries >= MAX_BERRIES) return;
        GrowBerries();
    }

    private void GrowBerries()
    {
        if (_berry_grow_minutes >= BERRY_GROW_TIME_MINUTES)
        {
            _berry_grow_minutes -= BERRY_GROW_TIME_MINUTES;
            _berries++;
            AdjustColor();
        }

        _berry_grow_minutes += Gamevariables.MinutesPerTick;
    }

    private void AdjustColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f - ((float)_berries / (float)MAX_BERRIES), 0, 1f);
    }


    public float Consume()
    {
        if (_berries <= 0) return 0;

        int berries_eaten = Gamevariables.MinutesPerTick; // 1 Berry per Minute
        if (_berries - berries_eaten < 0)
        {
            berries_eaten = _berries;
        }

        _berries -= berries_eaten;
        AdjustColor();
        return (float)berries_eaten * NUTRITION_PER_BERRY;
    }
}
