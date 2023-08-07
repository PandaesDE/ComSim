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


    }

    private void toHome()
    {
        Gamevariables.SEED = ipt_Seed.text;
        Gamevariables.AMOUNT_SPAWN_HUMAN = int.Parse(ipt_Humans.text);
        Gamevariables.AMOUNT_SPAWN_ANIMAL = int.Parse(ipt_Animals.text);
        SceneManager.LoadScene("MainMenu");
    }
}
