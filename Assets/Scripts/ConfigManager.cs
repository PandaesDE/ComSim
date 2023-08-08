using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    [System.Serializable]
    public class SettingsData
    {
        public string Seed = "";
        public int Human_Amount_Start = 0;
        public int Animal_Amount_Start = 0;
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
        Gamevariables.ANIMAL_AMOUNT_START = settings.Animal_Amount_Start;
    }

    public static void SaveSettings(SettingsData settings)
    {
        string json = JsonUtility.ToJson(settings, true);
        System.IO.File.WriteAllText(Path.Combine(Application.persistentDataPath, "Settings.json"), json);
    }

    public static SettingsData ReadSettings()
    {
        string filePath = Path.Combine(Application.persistentDataPath, "Settings.json");
        TextAsset jsonFile = Resources.Load<TextAsset>("Settings");
        if (!File.Exists(filePath))
        {
            Debug.LogWarning("Config file not found!");
            return new SettingsData(); // Return default settings
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<SettingsData>(json);

    }
}