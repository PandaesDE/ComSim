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

    private void Awake()
    {
        InitSimulation();
    }

    public static void LoadScene(string scene)
    {
        if (scene == Scenes.SIMULATION)
        {
            SceneManager.LoadScene(Scenes.SIMULATION);
            InitSimulation();
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

    private static void InitSimulation()
    {
        InitGameSettings();
        ConfigManager.LoadSettings();
    }

    private static void InitGameSettings()
    {
        Time.timeScale = 1;
        Time.fixedDeltaTime = Gamevariables.TICKRATE;
    }
}
