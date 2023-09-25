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

    //bool flags so that onValueChanged only happens with user input
    private bool seedInput_By_User = true;
    private bool xInput_By_User = true;
    private bool yInput_By_User = true;

    [SerializeField] private TMP_InputField ipt_Humans;
    [SerializeField] private TMP_InputField ipt_Lions;
    [SerializeField] private TMP_InputField ipt_Boars;
    [SerializeField] private TMP_InputField ipt_Rabbits;

    private string seed;

    // Start is called before the first frame update
    private void Awake()
    {
        ipt_X_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;
        ipt_Y_Offset.characterValidation = TMP_InputField.CharacterValidation.Digit;

        btn_Home.onClick.AddListener(delegate { toHome(); });
        btn_Editor.onClick.AddListener(delegate { toEditor(); });

        ipt_Seed.onValueChanged.AddListener(delegate { seedChanged(); });
        ipt_X_Offset.onValueChanged.AddListener(delegate { xOffsetChanged(); });
        ipt_Y_Offset.onValueChanged.AddListener(delegate { yOffsetChanged(); });

        displaySerializedSettings();
    }

    private void seedChanged()
    {
        if (seedInput_By_User)
        {
            xInput_By_User = false;
            yInput_By_User = false;
        } else
        {
            seedInput_By_User = true;
            return;
        }

        seedX = Util.SeedHelper.getSeedX(ipt_Seed.text);
        seedY = Util.SeedHelper.getSeedY(ipt_Seed.text);

        ipt_X_Offset.text = "" + Util.SeedHelper.convertSeedToCoordinate(seedX);
        ipt_Y_Offset.text = "" + Util.SeedHelper.convertSeedToCoordinate(seedY);
    }
    private void xOffsetChanged()
    {
        if (xInput_By_User)
        {
            seedInput_By_User = false;
        } else
        {
            xInput_By_User = true;
            return;
        }

        seedX = Util.SeedHelper.convertCoordinateToSeed(
            int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_X_Offset.text)));
        coordinatesChanged();
    }

    private void yOffsetChanged()
    {
        if (yInput_By_User)
        {
            seedInput_By_User = false;
        }
        else
        {
            yInput_By_User = true;
            return;
        }

        seedY = Util.SeedHelper.convertCoordinateToSeed(
            int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_Y_Offset.text)));
        coordinatesChanged();
    }


    private void coordinatesChanged()
    {
        ipt_Seed.text = seedX + seedY;
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
        settings.Seed = Util.UIHelper.preventNullOrEmptyInputString(ipt_Seed.text);
        settings.Human_Amount_Start = int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_Humans.text));
        settings.Lion_Amount_Start = int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_Lions.text));
        settings.Boar_Amount_Start = int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_Boars.text));
        settings.Rabbit_Amount_Start = int.Parse(Util.UIHelper.preventNullOrEmptyInputNumber(ipt_Rabbits.text));
        settings.Pso_Bush = 
        ConfigManager.SaveSettings(settings);
    }

}
