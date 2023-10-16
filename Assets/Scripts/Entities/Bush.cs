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

public class Bush : MonoBehaviour, IConsumable
{
    private static readonly float NUTRITION_PER_BERRY = .5f;
    private static readonly int MAX_BERRIES = 80;
    private static readonly int BERRY_GROW_TIME_MINUTES = 288; //5 Berries per Day

    public bool isMeat
    {
        get
        {
            return false;
        }
    }

    public bool hasFood
    {
        get
        {
            return berries > 0;
        }
    }

    [SerializeField] private int berry_grow_minutes = 0;
    [SerializeField] private int berries;


    private void Awake()
    {
        berries = Util.Random.Int(MAX_BERRIES);
    }

    private void Start()
    {
        adjustColor();
    }

    private void FixedUpdate()
    {
        if (berries >= MAX_BERRIES) return;
        growBerries();
    }

    private void growBerries()
    {
        if (berry_grow_minutes >= BERRY_GROW_TIME_MINUTES)
        {
            berry_grow_minutes -= BERRY_GROW_TIME_MINUTES;
            berries++;
            adjustColor();
        }

        berry_grow_minutes += Gamevariables.MINUTES_PER_TICK;
    }

    private void adjustColor()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f - ((float)berries / (float)MAX_BERRIES), 0, 1f);
    }


    public float Consume()
    {
        if (berries <= 0) return 0;

        int berries_eaten = Gamevariables.MINUTES_PER_TICK; // 1 Berry per Minute
        if (berries - berries_eaten < 0)
        {
            berries_eaten = berries;
        }

        berries -= berries_eaten;
        adjustColor();
        return (float)berries_eaten * NUTRITION_PER_BERRY;
    }
}
