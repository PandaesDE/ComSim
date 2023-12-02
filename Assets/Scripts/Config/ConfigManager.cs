/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - This class handles the persistend read and write serialization operations of needed values in between simulation sessions
 *  
 *  References:
 *      Scene:
 *          - Any scene
 *      Script:
 *          - Can be used by any class statically
 *          - Usually in between Scene Scene changing
 *          
 *  Notes:
 *      - 
 *  
 *  Sources:
 *      - 
 */


using System.IO;
using UnityEngine;

public static class ConfigManager
{
    //private static bool _s_isRead = false;

    //Chat-GPT
    public static void LoadSettings()
    {
        LoadSettings(ReadSettings());
    }

    public static void LoadSettings(GameSettingsObject settings)
    {
        Gamevariables.Seed = settings.seed;
        Gamevariables.HumanAmountStart = settings.startAmountHuman;
        Gamevariables.LionAmountStart = settings.startAmountLion;
        Gamevariables.BoarAmountStart = settings.startAmountBoar;
        Gamevariables.RabbitAmountStart = settings.startAmountRabbit;
        Gamevariables.PSO_Ground = settings.PSO_Ground;
        Gamevariables.PSO_Bush = settings.PSO_Bush;
    }

    public static void SaveSettings(GameSettingsObject settings)
    {
        //if (areSettingsUnchanged(settings))
        //{
        //    return;
        //}
        //
        //isRead = false;
        string json = JsonUtility.ToJson(settings, true);

        //persistentDataPath Windows: "...\AppData\LocalLow\DefaultCompany\ComSim"
        System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "Settings.json"), json);
    }

    public static GameSettingsObject ReadSettings()
    {
        //if (isRead)
        //{
        //    //no need for reading from disc multiple times
        //    return getStaticConfig();
        //}

        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
        if (!File.Exists(filePath))
        {
            //default config
            return GetStaticConfig();
        }

        string json = File.ReadAllText(filePath);
        //isRead = true;
        return JsonUtility.FromJson<GameSettingsObject>(json);
    }

    /* getStaticConfig reads the serializable values from Gamevariables.
     * if this method gets called before ReadSettings() is called once, this will return the default config
     * if this method gets called after ReadSettings() was called, this will return the settings read.
     */
    private static GameSettingsObject GetStaticConfig()
    {
        GameSettingsObject settings = new();
        settings.seed = Gamevariables.Seed;
        settings.startAmountHuman = Gamevariables.HumanAmountStart;
        settings.startAmountLion = Gamevariables.LionAmountStart;
        settings.startAmountBoar = Gamevariables.BoarAmountStart;
        settings.startAmountRabbit = Gamevariables.RabbitAmountStart;
        settings.PSO_Ground = Gamevariables.PSO_Ground;
        settings.PSO_Bush = Gamevariables.PSO_Bush;
        return settings;
    }

    /*private static bool AreSettingsUnchanged(GameSettingsObject settings)
    {
        return settings.Equals(getStaticConfig());
    }*/
}