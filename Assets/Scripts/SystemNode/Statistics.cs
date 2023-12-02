/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - This Class keeps track of all many statistical numbers
 *  
 *  References:
 *      Scene:
 *          - Simulation scene(s)
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using System.Collections.Generic;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    //Count History
    [SerializeField] private static List<int> _humanCounts = new();
    [SerializeField] private static List<int> _lionCounts = new();
    [SerializeField] private static List<int> _boarCounts = new();
    [SerializeField] private static List<int> _rabbitCounts = new();

    //Death Reasons
    [SerializeField] private static Dictionary<Creature.DeathReason, int> _humanDeathReasons= new();
    [SerializeField] private static Dictionary<Creature.DeathReason, int> _lionDeathReasons= new();
    [SerializeField] private static Dictionary<Creature.DeathReason, int> _boarDeathReasons= new();
    [SerializeField] private static Dictionary<Creature.DeathReason, int> _rabbitDeathReasons= new();

    void FixedUpdate()
    {
        _humanCounts.Add(ObjectManager.AllHumans.Count);
        _lionCounts.Add(ObjectManager.AllLions.Count);
        _boarCounts.Add(ObjectManager.AllBoars.Count);
        _rabbitCounts.Add(ObjectManager.AllRabbits.Count);
    }

    public static void incrementHumanDeathReason(Creature.DeathReason reason)
    {
        incrementDeathReason(_humanDeathReasons, reason);
    }

    public static void incrementLionDeathReason(Creature.DeathReason reason)
    {
        incrementDeathReason(_lionDeathReasons, reason);
    }

    public static void incrementBoarDeathReason(Creature.DeathReason reason)
    {
        incrementDeathReason(_boarDeathReasons, reason);
    }

    public static void incrementRabbitDeathReason(Creature.DeathReason reason)
    {
        incrementDeathReason(_rabbitDeathReasons, reason);
    }

    private static void incrementDeathReason(Dictionary<Creature.DeathReason,int> drList, Creature.DeathReason dr)
    {
        if (!drList.ContainsKey(dr))
        {
            drList[dr] = 0;
        }
        drList[dr]++;
    }
}
