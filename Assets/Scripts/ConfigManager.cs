/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *      - This Class Handles the persistend serialization of needed Values in between Simulation Sessions
 *  
 *  Class Infos:
 *      - This Script is not tied to any Gameobject in any Scene
 *      
 *  Class References:
 *      
 */


using System.IO;
using UnityEngine;

public class ConfigManager
{
    [System.Serializable]
    public class SettingsData
    {
        public string Seed = "";
        public int Human_Amount_Start = 0;
        public int Lion_Amount_Start = 0;
        public int Boar_Amount_Start = 0;
        public int Rabbit_Amount_Start = 0;
        public PerlinSettingsObject Pso_Ground;
        public PerlinSettingsObject Pso_Bush;
    }

    //Chat-GPT
    public static void LoadSettings()
    {
        LoadSettings(ReadSettings());
    }

    public static void LoadSettings(SettingsData settings)
    {
        Gamevariables.SEED = settings.Seed;
        Gamevariables.HUMAN_AMOUNT_START = settings.Human_Amount_Start;
        Gamevariables.LION_AMOUNT_START = settings.Lion_Amount_Start;
        Gamevariables.BOAR_AMOUNT_START = settings.Boar_Amount_Start;
        Gamevariables.RABBIT_AMOUNT_START = settings.Rabbit_Amount_Start;
        Gamevariables.PSO_GROUND = settings.Pso_Ground;
        Gamevariables.PSO_BUSH = settings.Pso_Bush;
    }

    public static void SaveSettings(SettingsData settings)
    {
        string json = JsonUtility.ToJson(settings, true);

        //persistentDataPath Windows: "...\AppData\LocalLow\DefaultCompany\ComSim"
        System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "Settings.json"), json);
    }

    public static SettingsData ReadSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Config file not found!");
            return new SettingsData(); // Return default settings
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<SettingsData>(json);

    }
}