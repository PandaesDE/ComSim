/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - UI handling of MapGenerator
 *  
 *  References:
 *      Scene:
 *          - Mapgenerator
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

public class UI_MapGeneration : MonoBehaviour
{
    [SerializeField] private Button _btn_Back;

    [SerializeField] private TMP_Dropdown _drd_PerlinNoiseSettings;

    [SerializeField] private TMP_InputField _inp_Persistence;
    [SerializeField] private TMP_InputField _inp_Frequency;
    [SerializeField] private TMP_InputField _inp_Octaves;
    [SerializeField] private TMP_InputField _inp_Amplitude;
    [SerializeField] private TMP_InputField _inp_X_Offset;
    [SerializeField] private TMP_InputField _inp_Y_Offset;

    private NoiseTextureGenerator _ntg;
    private PerlinSettingsObject _active_pso;
    private EditorMapGenerator _emg;

    private void Awake()
    {
        //https://discussions.unity.com/t/making-a-input-field-only-accepting-numbers/117245
        _inp_Persistence.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        _inp_Frequency.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        _inp_Octaves.characterValidation = TMP_InputField.CharacterValidation.Integer;
        _inp_Amplitude.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        _ntg = GameObject.Find("Playground").GetComponent<NoiseTextureGenerator>();
        _emg = GameObject.Find("Playground").GetComponent<EditorMapGenerator>();

        _btn_Back.onClick.AddListener(delegate { ToSettingsMenu(); });

        _drd_PerlinNoiseSettings.onValueChanged.AddListener(delegate {
            ChangeActivePerlinSettings(_drd_PerlinNoiseSettings.options[_drd_PerlinNoiseSettings.value].text);
        });

        _inp_Persistence.onValueChanged.AddListener(delegate {
            ChangePersistence();
        });
        _inp_Frequency.onValueChanged.AddListener(delegate {
            ChangeFrequency();
        });
        _inp_Octaves.onValueChanged.AddListener(delegate {
            ChangeOctaves();
        });
        _inp_Amplitude.onValueChanged.AddListener(delegate {
            ChangeAmplitude();
        });

        _inp_X_Offset.onValueChanged.AddListener(delegate
        {
            ChangeXOffset();
        });

        _inp_Y_Offset.onValueChanged.AddListener(delegate
        {
            ChangeYOffset();
        });
    }

    private void Start()
    {
        ChangeActivePerlinSettings(_drd_PerlinNoiseSettings.options[_drd_PerlinNoiseSettings.value].text);
        SetValues();
    }

    private void ToSettingsMenu()
    {

        GameSettingsObject settings = ConfigManager.ReadSettings();

        settings.PSO_Ground = _ntg.PSO_Ground;
        settings.PSO_Bush = _ntg.PSO_Bush;

        ConfigManager.SaveSettings(settings);
        GameManager.LoadScene(GameManager.Scenes.SETTINGS_MENU);
    }

    private void SetValues()
    {
        _inp_Persistence.text = "" + _active_pso.persistence;
        _inp_Frequency.text = "" + _active_pso.frequency;
        _inp_Octaves.text = "" + _active_pso.octaves;
        _inp_Amplitude.text = "" + _active_pso.amplitude;
        _inp_X_Offset.text = "" + _active_pso.xOrg;
        _inp_Y_Offset.text = "" + _active_pso.yOrg;
    }

    private void ChangePersistence()
    {
        if (!Util.UI.IsValidNumericEntry(_inp_Persistence.text)) return;
        _active_pso.persistence = float.Parse(_inp_Persistence.text);
        _emg.RenderTexture();
    }

    private void ChangeFrequency()
    {

        if (!Util.UI.IsValidNumericEntry(_inp_Frequency.text)) return;
        _active_pso.frequency = float.Parse(_inp_Frequency.text);
        _emg.RenderTexture();
    }

    private void ChangeOctaves()
    {
        if (!Util.UI.IsValidNumericEntry(_inp_Octaves.text)) return;
        _active_pso.octaves = int.Parse(_inp_Octaves.text);
        _emg.RenderTexture();
    }

    private void ChangeAmplitude()
    {
        if (!Util.UI.IsValidNumericEntry(_inp_Amplitude.text)) return;
        _active_pso.amplitude = float.Parse(_inp_Amplitude.text);
        _emg.RenderTexture();
    }

    private void ChangeXOffset()
    {
        if (!Util.UI.IsValidNumericEntry(_inp_X_Offset.text)) return;
        _active_pso.xOrg = float.Parse(_inp_X_Offset.text);
        _emg.RenderTexture();
    }

    private void ChangeYOffset()
    {
        if (!Util.UI.IsValidNumericEntry(_inp_Y_Offset.text)) return;
        _active_pso.yOrg = float.Parse(_inp_Y_Offset.text);
        _emg.RenderTexture();
    }

    private void ChangeActivePerlinSettings(string txt)
    {
        if (txt.Equals("Ground"))
        {
            if (_ntg.PSO_Ground == null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            _active_pso = _ntg.PSO_Ground;
            SetValues();
            return;
        }
        if (txt.Equals("Bush"))
        {
            if (_ntg.PSO_Bush == null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            _active_pso = _ntg.PSO_Bush;
            SetValues();
            return;
        }
    }
}
