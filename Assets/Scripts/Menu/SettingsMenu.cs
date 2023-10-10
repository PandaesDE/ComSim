/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

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
    [SerializeField] private TMP_InputField ipt_X_Offset;
    [SerializeField] private TMP_InputField ipt_Y_Offset;

    [SerializeField] private TMP_InputField ipt_Humans;
    [SerializeField] private TMP_InputField ipt_Lions;
    [SerializeField] private TMP_InputField ipt_Boars;
    [SerializeField] private TMP_InputField ipt_Rabbits;

    // Start is called before the first frame update
    private void Awake()
    {
        ipt_X_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;
        ipt_Y_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;

        btn_Home.onClick.AddListener(delegate { toHome(); });
        btn_Editor.onClick.AddListener(delegate { toEditor(); });

        ipt_Seed.onValueChanged.AddListener(delegate { seedChanged(); });
        ipt_X_Offset.onValueChanged.AddListener(delegate { coordinatesChanged(); });
        ipt_Y_Offset.onValueChanged.AddListener(delegate { coordinatesChanged(); });

        displaySerializedSettings();
    }

    private void seedChanged()
    {
        if (!ipt_Seed.isFocused) return;

        Vector2 coords = Util.Conversion.SeedToCoordinates(ipt_Seed.text);
        ipt_X_Offset.text = "" + coords.x;
        ipt_Y_Offset.text = "" + coords.y;
    }

    private void coordinatesChanged()
    {
        if (!ipt_X_Offset.isFocused && !ipt_Y_Offset.isFocused) return;

        ipt_Seed.text = "";
    }

    private void displaySerializedSettings()
    {
        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        ipt_Seed.text = settings.Seed;
        ipt_Humans.text = "" + settings.Human_Amount_Start ;
        ipt_Lions.text = "" + settings.Lion_Amount_Start;
        ipt_Boars.text = "" + settings.Boar_Amount_Start;
        ipt_Rabbits.text = "" + settings.Rabbit_Amount_Start;
        ipt_X_Offset.text = "" + settings.Pso_Ground.xOrg;
        ipt_Y_Offset.text = "" + settings.Pso_Ground.yOrg;
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
        settings.Seed =                 Util.UI.preventNullOrEmptyInputString(ipt_Seed.text);
        settings.Human_Amount_Start =   int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Humans.text));
        settings.Lion_Amount_Start =    int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Lions.text));
        settings.Boar_Amount_Start =    int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Boars.text));
        settings.Rabbit_Amount_Start =  int.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Rabbits.text));
        settings.Pso_Ground.xOrg =      float.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_X_Offset.text));
        settings.Pso_Ground.yOrg =      float.Parse(Util.UI.preventNullOrEmptyInputNumber(ipt_Y_Offset.text));
        ConfigManager.SaveSettings(settings);
    }

}
