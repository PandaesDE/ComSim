using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private Button _btn_Continue;
    [SerializeField] private Button _btn_Copy;
    [SerializeField] private Button _btn_NewSim;
    [SerializeField] private TMP_Text _display_GameOverText;

    private GameManager gameManager;
    private Statistics statistics;

    private void Awake()
    {
        gameManager = GameObject.Find("SystemNode").GetComponent<GameManager>();
        statistics = GameObject.Find("SystemNode").GetComponent<Statistics>();

        _btn_Continue.onClick.AddListener(delegate
        {
            gameManager.ContinueSimulation();
            gameObject.SetActive(false);
        });

        _btn_Copy.onClick.AddListener(delegate
        {
            GUIUtility.systemCopyBuffer = statistics.GetLog();
        });
        
        _btn_NewSim.onClick.AddListener(delegate
        {
            gameManager.InitNewSimulation();
            gameObject.SetActive(false);
        });
    }

    public void SetGameOverText(string gameOverReason)
    {
        _display_GameOverText.text =
            $"due to: {gameOverReason}\n" +
            $"{SpeciesSpecificStatistic("Human", Statistics.HumanCounts, Statistics.HumanDeathReasons)}\n" +
            $"{SpeciesSpecificStatistic("Lion", Statistics.LionCounts, Statistics.LionDeathReasons)}\n" +
            $"{SpeciesSpecificStatistic("Boar", Statistics.BoarCounts, Statistics.BoarDeathReasons)}\n" +
            $"{SpeciesSpecificStatistic("Rabbit", Statistics.RabbitCounts, Statistics.RabbitDeathReasons)}\n";
    }

    private string SpeciesSpecificStatistic(string species, List<Statistics.CountData> counts, Dictionary<Creature.DeathReason, int> drDict)
    {
        int startVal = 0;
        int endVal = 0;
        if (counts.Count > 0)
        {
            startVal = counts[0].Count;
            endVal = counts[^1].Count;
        }
        return  $"{species} amount: {startVal} - {endVal}, " +
                $"deathsBy: {DeathReasonDictToString(drDict)}";
    }

    private string DeathReasonDictToString(Dictionary<Creature.DeathReason, int> drDict)
    {
        string outp = "";
        foreach (KeyValuePair<Creature.DeathReason, int> kvp in drDict) 
        { 
            outp += $"{kvp.Key}: {kvp.Value}, "; 
        }
        return outp;
    }
}
