using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private Button _btn_Continue;
    [SerializeField] private Button _btn_Copy;
    [SerializeField] private Button _btn_Quit;
    [SerializeField] private TMP_Text _display_GameOverText;

    private GameManager gameManager;
    private Statistics statistics;

    private void Awake()
    {
        gameManager = GameObject.Find("SystemNode").GetComponent<GameManager>();
        statistics = GameObject.Find("SystemNode").GetComponent<Statistics>();

        _btn_Continue.onClick.AddListener(delegate
        {
            gameManager.InitNewSimulation();
            gameObject.SetActive(false);
        });

        _btn_Copy.onClick.AddListener(delegate
        {
            GUIUtility.systemCopyBuffer = statistics.GetLog();
        });
        
        _btn_Quit.onClick.AddListener(delegate
        {
            GameManager.LoadScene(GameManager.Scenes.MAIN_MENU);
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
        return  $"{species} amount: {counts[1].Count} - {counts[^1].Count}, " +
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
