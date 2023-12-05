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

public delegate void StatisticUpdateEvent();

public class Statistics : MonoBehaviour
{
    private FileLogger _logger;
    private Timer _timeBetweenValues = new(Gamevariables.MINUTES_PER_HOUR);

    public event StatisticUpdateEvent StatisticUpdateEvent;

    public class CountData
    {
        public int Count = 0;
        public int Males = 0;
        public int Females = 0;
    }

    //Count History
    public static List<CountData> HumanCounts {get; private set;}= new();
    public static List<CountData> LionCounts {get; private set;}= new();
    public static List<CountData> BoarCounts {get; private set;}= new();
    public static List<CountData> RabbitCounts {get; private set;}= new();

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
        if (!Spawner.S_InitializedSpawns) return;

        if (!_timeBetweenValues.Finished())
        {
            _timeBetweenValues.Tick();
            return;
        }
        _timeBetweenValues.Reset();

        HumanCounts.Add(SetCountData(ObjectManager.AllHumans));
        LionCounts.Add(SetCountData(ObjectManager.AllLions));
        BoarCounts.Add(SetCountData(ObjectManager.AllBoars));
        RabbitCounts.Add(SetCountData(ObjectManager.AllRabbits));
        StatisticUpdateEvent();
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

    private CountData SetCountData<T>(Dictionary<int, T> creatureDict) where T : Creature
    {
        int males = 0;
        int female = 0;
        foreach(KeyValuePair<int, T> kvp in creatureDict)
        {
            if (kvp.Value.Gender.IsMale) males++;
            else female++;
        }
        return new CountData()
        {
            Count = creatureDict.Count,
            Males = males,
            Females = female
        };
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
