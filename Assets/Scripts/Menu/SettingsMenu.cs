using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btn_Home;
    [SerializeField] private Button btn_Editor;

    [SerializeField] private TMP_InputField ipt_Seed;
    [SerializeField] private TMP_InputField ipt_Humans;
    [SerializeField] private TMP_InputField ipt_Lions;
    [SerializeField] private TMP_InputField ipt_Boars;
    [SerializeField] private TMP_InputField ipt_Rabbits;

    // Start is called before the first frame update
    void Start()
    {
        btn_Home.onClick.AddListener(delegate { toHome(); });
        btn_Editor.onClick.AddListener(delegate { toEditor(); });

        displaySerializedSettings();
    }

    private void displaySerializedSettings()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        ipt_Seed.text = settings.Seed;
        ipt_Humans.text = "" + settings.Human_Amount_Start ;
        ipt_Lions.text = "" + settings.Lion_Amount_Start;
        ipt_Boars.text = "" + settings.Boar_Amount_Start;
        ipt_Rabbits.text = "" + settings.Rabbit_Amount_Start;
    }

    private void toEditor()
    {
        saveSettings();
        SceneManager.LoadScene("EditorMapGeneration");
    }
    private void toHome()
    {
        saveSettings();
        SceneManager.LoadScene("MainMenu");
    }

    private void saveSettings()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        preventNullOrEmptyInputs();
        settings.Seed = ipt_Seed.text;
        settings.Human_Amount_Start = int.Parse(ipt_Humans.text);
        settings.Lion_Amount_Start = int.Parse(ipt_Lions.text);
        settings.Boar_Amount_Start = int.Parse(ipt_Boars.text);
        settings.Rabbit_Amount_Start = int.Parse(ipt_Rabbits.text);
        ConfigManager.SaveSettings(settings);
    }

    private void preventNullOrEmptyInputs()
    {
        if (string.IsNullOrEmpty(ipt_Seed.text)) ipt_Seed.text = "";
        if (string.IsNullOrEmpty(ipt_Humans.text)) ipt_Humans.text = "0";
        if (string.IsNullOrEmpty(ipt_Lions.text)) ipt_Lions.text = "0";
        if (string.IsNullOrEmpty(ipt_Boars.text)) ipt_Lions.text = "0";
        if (string.IsNullOrEmpty(ipt_Rabbits.text)) ipt_Lions.text = "0";
    }
}
