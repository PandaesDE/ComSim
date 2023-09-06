using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button btn_Home;

    [SerializeField] private TMP_InputField ipt_Seed;
    [SerializeField] private TMP_InputField ipt_Humans;
    [SerializeField] private TMP_InputField ipt_Animals;

    // Start is called before the first frame update
    void Start()
    {
        btn_Home.onClick.AddListener(toHome);

        displaySerializedSettings();
    }

    private void displaySerializedSettings()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        ipt_Seed.text = settings.Seed;
        ipt_Humans.text = "" + settings.Human_Amount_Start ;
        ipt_Animals.text = "" + settings.Animal_Amount_Start;
    }

    private void toHome()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        preventNullOrEmptyInputs();
        settings.Seed = ipt_Seed.text;
        settings.Human_Amount_Start = int.Parse(ipt_Humans.text);
        settings.Animal_Amount_Start = int.Parse(ipt_Animals.text);
        ConfigManager.SaveSettings(settings);
        SceneManager.LoadScene("MainMenu");
    }

    private void preventNullOrEmptyInputs()
    {
        if (string.IsNullOrEmpty(ipt_Seed.text)) ipt_Seed.text = "";
        if (string.IsNullOrEmpty(ipt_Humans.text)) ipt_Humans.text = "0";
        if (string.IsNullOrEmpty(ipt_Animals.text)) ipt_Animals.text = "0";
    }
}
