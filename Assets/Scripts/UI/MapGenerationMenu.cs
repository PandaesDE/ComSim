using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MapGenerationMenu : MonoBehaviour
{
    [SerializeField] private Button btn_Back;

    [SerializeField] private TMP_Dropdown drd_PerlinNoiseSettings;

    [SerializeField] private TMP_InputField inp_Persistence;
    [SerializeField] private TMP_InputField inp_Frequency;
    [SerializeField] private TMP_InputField inp_Octaves;
    [SerializeField] private TMP_InputField inp_Amplitude;
    [SerializeField] private TMP_InputField inp_X_Offset;
    [SerializeField] private TMP_InputField inp_Y_Offset;

    private NoiseTextureGenerator ntg;
    private PerlinSettingsObject active_pso;
    private EditorMapGenerator emg;

    private void Awake()
    {
        //https://discussions.unity.com/t/making-a-input-field-only-accepting-numbers/117245
        inp_Persistence.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        inp_Frequency.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        inp_Octaves.characterValidation = TMP_InputField.CharacterValidation.Integer;
        inp_Amplitude.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        ntg = GameObject.Find("Playground").GetComponent<NoiseTextureGenerator>();
        emg = GameObject.Find("Playground").GetComponent<EditorMapGenerator>();

        btn_Back.onClick.AddListener(delegate { toSettingsMenu(); });

        drd_PerlinNoiseSettings.onValueChanged.AddListener(delegate {
            changeActivePerlinSettings(drd_PerlinNoiseSettings.options[drd_PerlinNoiseSettings.value].text);
        });

        inp_Persistence.onValueChanged.AddListener(delegate {
            changePersistence();
        });
        inp_Frequency.onValueChanged.AddListener(delegate {
            changeFrequency();
        });
        inp_Octaves.onValueChanged.AddListener(delegate {
            changeOctaves();
        });
        inp_Amplitude.onValueChanged.AddListener(delegate {
            changeAmplitude();
        });

        inp_X_Offset.onValueChanged.AddListener(delegate
        {
            changeXOffset();
        });

        inp_Y_Offset.onValueChanged.AddListener(delegate
        {
            changeYOffset();
        });
    }

    private void Start()
    {
        changeActivePerlinSettings(drd_PerlinNoiseSettings.options[drd_PerlinNoiseSettings.value].text);
        setValues();
    }

    private void toSettingsMenu()
    {

        ConfigManager.SettingsData settings = ConfigManager.ReadSettings();
        //preventNullOrEmptyInputs();

        settings.Pso_Ground = ntg.pso_ground;
        settings.Pso_Bush = ntg.pso_bush;

        ConfigManager.SaveSettings(settings);
        SceneManager.LoadScene("SettingsMenu");
    }

    private void setValues()
    {
        inp_Persistence.text = "" + active_pso.persistence;
        inp_Frequency.text = "" + active_pso.frequency;
        inp_Octaves.text = "" + active_pso.octaves;
        inp_Amplitude.text = "" + active_pso.amplitude;
        inp_X_Offset.text = "" + active_pso.xOrg;
        inp_Y_Offset.text = "" + active_pso.yOrg;
    }

    private void changePersistence()
    {
        if (!Util.UIHelper.isValidNumvericEntry(inp_Persistence.text)) return;
        active_pso.persistence = float.Parse(inp_Persistence.text);
        emg.renderTexture();
    }

    private void changeFrequency()
    {

        if (!Util.UIHelper.isValidNumvericEntry(inp_Frequency.text)) return;
        active_pso.frequency = float.Parse(inp_Frequency.text);
        emg.renderTexture();
    }

    private void changeOctaves()
    {
        if (!Util.UIHelper.isValidNumvericEntry(inp_Octaves.text)) return;
        active_pso.octaves = int.Parse(inp_Octaves.text);
        emg.renderTexture();
    }

    private void changeAmplitude()
    {
        if (!Util.UIHelper.isValidNumvericEntry(inp_Amplitude.text)) return;
        active_pso.amplitude = float.Parse(inp_Amplitude.text);
        emg.renderTexture();
    }

    private void changeXOffset()
    {
        if (!Util.UIHelper.isValidNumvericEntry(inp_X_Offset.text)) return;
        active_pso.xOrg = float.Parse(inp_X_Offset.text);
        emg.renderTexture();
    }

    private void changeYOffset()
    {
        if (!Util.UIHelper.isValidNumvericEntry(inp_Y_Offset.text)) return;
        active_pso.yOrg = float.Parse(inp_Y_Offset.text);
        emg.renderTexture();
    }

    private void changeActivePerlinSettings(string txt)
    {
        if (txt.Equals("Ground"))
        {
            if (ntg.pso_ground == null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            active_pso = ntg.pso_ground;
            setValues();
            return;
        }
        if (txt.Equals("Bush"))
        {
            if (ntg.pso_bush == null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            active_pso = ntg.pso_bush;
            setValues();
            return;
        }
    }
}
