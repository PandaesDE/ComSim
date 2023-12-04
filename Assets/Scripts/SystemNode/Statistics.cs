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
    private FileLogger _logger;
    private Timer _timeBetweenValues = new(Gamevariables.MINUTES_PER_HOUR);

    //Count History
    public static List<int> HumanCounts {get; private set;}= new();
    public static List<int> LionCounts {get; private set;}= new();
    public static List<int> BoarCounts {get; private set;}= new();
    public static List<int> RabbitCounts {get; private set;}= new();

    //Death Reasons
    public static Dictionary<Creature.DeathReason, int> HumanDeathReasons {get; private set;}= new();
    public static Dictionary<Creature.DeathReason, int> LionDeathReasons {get; private set;}= new();
    public static Dictionary<Creature.DeathReason, int> BoarDeathReasons {get; private set;}= new();
    public static Dictionary<Creature.DeathReason, int> RabbitDeathReasons { get; private set; } = new();

    private void Awake()
    {
        _logger = new(Path.Combine(Application.dataPath, "LogData"));
    }

    void FixedUpdate()
    {
        if (!_timeBetweenValues.Finished())
        {
            _timeBetweenValues.Tick();
            return;
        }
        _timeBetweenValues.Reset();

        HumanCounts.Add(ObjectManager.AllHumans.Count);
        LionCounts.Add(ObjectManager.AllLions.Count);
        BoarCounts.Add(ObjectManager.AllBoars.Count);
        RabbitCounts.Add(ObjectManager.AllRabbits.Count);
    }

    public string GetLog()
    {
        _logger.ClearLog();
        FillLog();
        return _logger.GetLog();
    }

    public void ClearStatistics()
    {
        HumanCounts.Clear();
        LionCounts.Clear();
        BoarCounts.Clear();
        RabbitCounts.Clear();

        HumanDeathReasons.Clear();
        LionDeathReasons.Clear();
        BoarDeathReasons.Clear();
        RabbitDeathReasons.Clear();
    }

    public static void IncrementHumanDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(HumanDeathReasons, reason);
    }

    public static void IncrementLionDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(LionDeathReasons, reason);
    }

    public static void IncrementBoarDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(BoarDeathReasons, reason);
    }

    public static void IncrementRabbitDeathReason(Creature.DeathReason reason)
    {
        IncrementDeathReason(RabbitDeathReasons, reason);
    }

    private static void IncrementDeathReason(Dictionary<Creature.DeathReason,int> drList, Creature.DeathReason dr)
    {
        if (!drList.ContainsKey(dr))
        {
            drList[dr] = 0;
        }
        drList[dr]++;
    }

    private void FillLog()
    {
        _logger.AddLogEntry_Count("Human Amount History", HumanCounts);
        _logger.AddLogEntry_Count("Lion Amount History", LionCounts);
        _logger.AddLogEntry_Count("Boar Amount History", BoarCounts);
        _logger.AddLogEntry_Count("Rabbit Amount History", RabbitCounts);

        _logger.AddLogEntry_DeathReaons("Human Death Reasons", HumanDeathReasons);
        _logger.AddLogEntry_DeathReaons("Lion Death Reasons", LionDeathReasons);
        _logger.AddLogEntry_DeathReaons("Boar Death Reasons", BoarDeathReasons);
        _logger.AddLogEntry_DeathReaons("Rabbit Death Reasons", RabbitDeathReasons);
    }

    private void OnApplicationQuit()
    {
        if (!Gamevariables.LOGGING_ENABLED) return;
        _logger.ClearLog();
        FillLog();
        _logger.Log();
    }
}
