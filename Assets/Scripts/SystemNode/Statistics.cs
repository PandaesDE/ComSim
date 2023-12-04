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
using System.IO;
using UnityEngine;

public class Statistics : MonoBehaviour
{
    private static FileLogger _s_logger;
    private Timer _timeBetweenValues = new(Gamevariables.MINUTES_PER_HOUR);

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

    private void Awake()
    {
        _s_logger = new(Path.Combine(Application.dataPath, "LogData"));
    }

    void FixedUpdate()
    {
        if (!_timeBetweenValues.Finished())
        {
            _timeBetweenValues.Tick();
            return;
        }
        _timeBetweenValues.Reset();

        _humanCounts.Add(ObjectManager.AllHumans.Count);
        _lionCounts.Add(ObjectManager.AllLions.Count);
        _boarCounts.Add(ObjectManager.AllBoars.Count);
        _rabbitCounts.Add(ObjectManager.AllRabbits.Count);
    }

    public static string GetLog()
    {
        _s_logger.ClearLog();
        FillLog();
        return _s_logger.GetLog();
    }

    public static void IncrementHumanDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(_humanDeathReasons, reason);
    }

    public static void IncrementLionDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(_lionDeathReasons, reason);
    }

    public static void IncrementBoarDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(_boarDeathReasons, reason);
    }

    public static void IncrementRabbitDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(_rabbitDeathReasons, reason);
    }

    private static void IncrementDeathReason(Dictionary<Creature.DeathReason,int> drList, Creature.DeathReason dr)
    {
        if (!drList.ContainsKey(dr))
        {
            drList[dr] = 0;
        }
        drList[dr]++;
    }

    private static void FillLog()
    {
        _s_logger.AddLogEntry_Count("Human Amount History", _humanCounts);
        _s_logger.AddLogEntry_Count("Lion Amount History", _lionCounts);
        _s_logger.AddLogEntry_Count("Boar Amount History", _boarCounts);
        _s_logger.AddLogEntry_Count("Rabbit Amount History", _rabbitCounts);

        _s_logger.AddLogEntry_DeathReaons("Human Death Reasons", _humanDeathReasons);
        _s_logger.AddLogEntry_DeathReaons("Lion Death Reasons", _lionDeathReasons);
        _s_logger.AddLogEntry_DeathReaons("Boar Death Reasons", _boarDeathReasons);
        _s_logger.AddLogEntry_DeathReaons("Rabbit Death Reasons", _rabbitDeathReasons);
    }

    private void OnApplicationQuit()
    {
        if (!Gamevariables.LOGGING_ENABLED) return;
        _s_logger.ClearLog();
        FillLog();
        _s_logger.Log();
    }
}
