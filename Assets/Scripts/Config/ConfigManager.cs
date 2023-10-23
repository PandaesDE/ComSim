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
    private static bool isRead = false;

    //Chat-GPT
    public static void LoadSettings()
    {
        LoadSettings(ReadSettings());
    }

    public static void LoadSettings(GameSettingsObject settings)
    {
        Gamevariables.SEED = settings.Seed;
        Gamevariables.HUMAN_AMOUNT_START = settings.Human_Amount_Start;
        Gamevariables.LION_AMOUNT_START = settings.Lion_Amount_Start;
        Gamevariables.BOAR_AMOUNT_START = settings.Boar_Amount_Start;
        Gamevariables.RABBIT_AMOUNT_START = settings.Rabbit_Amount_Start;
        Gamevariables.PSO_GROUND = settings.Pso_Ground;
        Gamevariables.PSO_BUSH = settings.Pso_Bush;
    }

    public static void SaveSettings(GameSettingsObject settings)
    {
        if (areSettingsUnchanged(settings))
        {
            return;
        }

        isRead = false;
        string json = JsonUtility.ToJson(settings, true);

        //persistentDataPath Windows: "...\AppData\LocalLow\DefaultCompany\ComSim"
        System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "Settings.json"), json);
    }

    public static GameSettingsObject ReadSettings()
    {
        if (isRead)
        {
            //no need for reading from disc multiple times
            return getStaticConfig();
        }

        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
        if (!File.Exists(filePath))
        {
            //default config
            return getStaticConfig();
        }

        string json = File.ReadAllText(filePath);
        isRead = true;
        return JsonUtility.FromJson<GameSettingsObject>(json);
    }

    /* getStaticConfig reads the serializable values from Gamevariables.
     * if this method gets called before ReadSettings() is called once, this will return the default config
     * if this method gets called after ReadSettings() was called, this will return the settings read.
     */
    private static GameSettingsObject getStaticConfig()
    {
        GameSettingsObject settings = new();
        settings.Seed = Gamevariables.SEED;
        settings.Human_Amount_Start = Gamevariables.HUMAN_AMOUNT_START;
        settings.Lion_Amount_Start = Gamevariables.LION_AMOUNT_START;
        settings.Boar_Amount_Start = Gamevariables.BOAR_AMOUNT_START;
        settings.Rabbit_Amount_Start = Gamevariables.RABBIT_AMOUNT_START;
        settings.Pso_Ground = Gamevariables.PSO_GROUND;
        settings.Pso_Bush = Gamevariables.PSO_BUSH;
        return settings;
    }

    private static bool areSettingsUnchanged(GameSettingsObject settings)
    {
        return settings.Equals(getStaticConfig());
    }
}