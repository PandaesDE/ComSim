using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btn_Start;
    [SerializeField] private Button btn_Settings;

    // Start is called before the first frame update
    void Start()
    {
        btn_Start.onClick.AddListener(toSimulation);
        btn_Settings.onClick.AddListener(toSettings);

        ConfigManager.LoadSettings();
    }

    private void toSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    private void toSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }


}
