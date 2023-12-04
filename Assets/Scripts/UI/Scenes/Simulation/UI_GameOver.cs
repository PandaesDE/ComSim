using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameOver : MonoBehaviour
{
    [SerializeField] private Button _btn_Continue;
    [SerializeField] private Button _btn_Copy;
    [SerializeField] private Button _btn_Quit;
    [SerializeField] private TMP_Text _display_GameOverText;

    private void Awake()
    {
        _btn_Continue.onClick.AddListener(delegate
        {
            GameManager.InitSimulation();
            gameObject.SetActive(false);
        });

        _btn_Copy.onClick.AddListener(delegate
        {
            GUIUtility.systemCopyBuffer = Statistics.GetLog();
        });
        
        _btn_Quit.onClick.AddListener(delegate
        {
            GameManager.LoadScene(GameManager.Scenes.MAIN_MENU);
        });
    }
}
