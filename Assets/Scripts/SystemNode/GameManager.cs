/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Manages The Game (main)
 *          - Initialize generic settings
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
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static class Scenes
    {
        public static readonly string SIMULATION =               "Simulation";
        public static readonly string SETTINGS_MENU =            "SettingsMenu";
        public static readonly string MAIN_MENU =                "MainMenu";
        public static readonly string EDITOR_MAP_GENERATION =    "EditorMapGeneration";
    }

    [SerializeField] private GameObject _gameOverBody;

    private static readonly int _S_MAX_CREATURES = 200; //200 DEFAULT
    private static bool _gameOver = false;

    private void Start()
    {
        ConfigManager.LoadSettings();
        Time.fixedDeltaTime = Gamevariables.TICKRATE;
        InitNewSimulation();
    }

    private void FixedUpdate()
    {
        CheckGameOverConditions();
    }

    public static void PauseGame()
    {
        Gamevariables.GamePaused = !Gamevariables.GamePaused;
        if (Gamevariables.GamePaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public static void LoadScene(string scene)
    {
        if (scene == Scenes.SIMULATION)
        {
            SceneManager.LoadScene(Scenes.SIMULATION);
            return;
        }

        if (scene == Scenes.SETTINGS_MENU)
        {
            SceneManager.LoadScene(Scenes.SETTINGS_MENU);
            return;
        }

        if (scene == Scenes.MAIN_MENU)
        {
            SceneManager.LoadScene(Scenes.MAIN_MENU);
            return;
        }

        if (scene == Scenes.EDITOR_MAP_GENERATION)
        {
            SceneManager.LoadScene(Scenes.EDITOR_MAP_GENERATION);
            return;
        }
    }

    public void InitNewSimulation()
    {
        GetComponent<Statistics>().ClearStatistics();
        InitGameSettings();

        ObjectManager.DeleteAllCreatures();
        ObjectManager.DeleteAllCorpses();
        Spawner.NewStartSpawn();
    }

    private void GameOver(string gameOverReason)
    {
        _gameOver = true;
        _gameOverBody.GetComponent<UI_GameOver>().SetGameOverText(gameOverReason);
        _gameOverBody.SetActive(true);
        Time.timeScale = 0;
    }

    private void CheckGameOverConditions()
    {
        if (_gameOver) return;

        int objCount = ObjectManager.AllCreatureCount;
        //extinct
        if (objCount <= 0 && Spawner.S_InitializedSpawns)
        {
            GameOver("all species extinct");
        }
        //overpopulation
        if (objCount >= _S_MAX_CREATURES)
        {
            GameOver("overpopulation");
        }
    }

    private static void InitGameSettings()
    {
        _gameOver = false;

        Time.timeScale = 1;

        Gamevariables.MinutesPassed = 0;
    }
}
