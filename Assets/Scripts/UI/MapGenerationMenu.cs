using TMPro;
using UnityEngine;

public class MapGenerationMenu : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown drd_PerlinNoiseSettings;

    [SerializeField] private TMP_InputField inp_Persistence;
    [SerializeField] private TMP_InputField inp_Frequency;
    [SerializeField] private TMP_InputField inp_Octaves;
    [SerializeField] private TMP_InputField inp_Amplitude;

    private EditorMapGenerator editorMG;
    private PerlinSettingsObject active_pso;

    private void Awake()
    {
        //https://discussions.unity.com/t/making-a-input-field-only-accepting-numbers/117245
        inp_Persistence.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        inp_Frequency.characterValidation = TMP_InputField.CharacterValidation.Decimal;
        inp_Octaves.characterValidation = TMP_InputField.CharacterValidation.Integer;
        inp_Amplitude.characterValidation = TMP_InputField.CharacterValidation.Decimal;

        editorMG = GameObject.Find("Playground").GetComponent<EditorMapGenerator>();

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
    }

    private void Start()
    {
        changeActivePerlinSettings(drd_PerlinNoiseSettings.options[drd_PerlinNoiseSettings.value].text);
        setValues();
    }

    private void setValues()
    {
        inp_Persistence.text = "" + active_pso.persistence;
        inp_Frequency.text = "" + active_pso.frequency;
        inp_Octaves.text = "" + active_pso.octaves;
        inp_Amplitude.text = "" + active_pso.amplitude;
    }

    private void changePersistence()
    {
        if (inp_Persistence.text.Length <= 0) return;
        active_pso.persistence = float.Parse(inp_Persistence.text);
    }

    private void changeFrequency()
    {

        if (inp_Frequency.text.Length <= 0) return;
        active_pso.frequency = float.Parse(inp_Frequency.text);
    }

    private void changeOctaves()
    {
        if (inp_Octaves.text.Length <= 0) return;
        active_pso.octaves = int.Parse(inp_Octaves.text);
    }

    private void changeAmplitude()
    {
        if (inp_Amplitude.text.Length <= 0) return;
        active_pso.amplitude = float.Parse(inp_Amplitude.text);
    }

    private void changeActivePerlinSettings(string txt)
    {
        if (txt.Equals("Ground"))
        {
            if (editorMG.pso_ground == null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            active_pso = editorMG.pso_ground;
            setValues();
            return;
        }
        if (txt.Equals("Bush"))
        {
            if (editorMG.pso_bush== null)
            {
                Debug.LogError("Editor Map Generator has not yet initialized its Perlin-Noise Settings Object");
            }
            active_pso = editorMG.pso_bush;
            setValues();
            return;
        }
    }
}
