/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Input handling of start settings
 *  
 *  References:
 *      Scene:
 *          - Settings Menu
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Settings : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Button _btn_Home;
    [SerializeField] private Button _btn_Editor;

    [SerializeField] private TMP_InputField _ipt_Seed;
    [SerializeField] private TMP_InputField _ipt_X_Offset;
    [SerializeField] private TMP_InputField _ipt_Y_Offset;

    [SerializeField] private TMP_InputField _ipt_Humans;
    [SerializeField] private TMP_InputField _ipt_Lions;
    [SerializeField] private TMP_InputField _ipt_Boars;
    [SerializeField] private TMP_InputField _ipt_Rabbits;

    // Start is called before the first frame update
    private void Awake()
    {
        _ipt_X_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;
        _ipt_Y_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;

        _btn_Home.onClick.AddListener(delegate { ToHome(); });
        _btn_Editor.onClick.AddListener(delegate { ToEditor(); });

        _ipt_Seed.onValueChanged.AddListener(delegate { SeedChanged(); });
        _ipt_X_Offset.onValueChanged.AddListener(delegate { CoordinatesChanged(); });
        _ipt_Y_Offset.onValueChanged.AddListener(delegate { CoordinatesChanged(); });

        DisplaySerializedSettings();
    }

    private void SeedChanged()
    {
        if (!_ipt_Seed.isFocused) return;

        Vector2 coords = Util.Conversion.SeedToCoordinates(_ipt_Seed.text);
        _ipt_X_Offset.text = "" + coords.x;
        _ipt_Y_Offset.text = "" + coords.y;
    }

    private void CoordinatesChanged()
    {
        if (!_ipt_X_Offset.isFocused && !_ipt_Y_Offset.isFocused) return;

        _ipt_Seed.text = "";
    }

    private void DisplaySerializedSettings()
    {
        GameSettingsObject settings = ConfigManager.ReadSettings();
        _ipt_Seed.text = settings.seed;
        _ipt_Humans.text = "" + settings.startAmountHuman;
        _ipt_Lions.text = "" + settings.startAmountLion;
        _ipt_Boars.text = "" + settings.startAmountBoar;
        _ipt_Rabbits.text = "" + settings.startAmountRabbit;
        _ipt_X_Offset.text = "" + settings.PSO_Ground.xOrg;
        _ipt_Y_Offset.text = "" + settings.PSO_Ground.yOrg;
    }

    private void ToEditor()
    {
        SaveSettings();
        SceneManager.LoadScene("EditorMapGeneration");
    }

    private void ToHome()
    {
        SaveSettings();
        SceneManager.LoadScene("MainMenu");
    }

    private void SaveSettings()
    {
        GameSettingsObject settings = ConfigManager.ReadSettings();
        settings.seed =                 Util.UI.PreventNullOrEmptyInputString(_ipt_Seed.text);
        settings.startAmountHuman =   int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Humans.text));
        settings.startAmountLion =    int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Lions.text));
        settings.startAmountBoar =    int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Boars.text));
        settings.startAmountRabbit =  int.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Rabbits.text));
        settings.PSO_Ground.xOrg =      float.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_X_Offset.text));
        settings.PSO_Ground.yOrg =      float.Parse(Util.UI.PreventNullOrEmptyInputNumber(_ipt_Y_Offset.text));
        ConfigManager.SaveSettings(settings);
    }

}
