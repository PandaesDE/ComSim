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

    private GameManager gameManager;
    private Statistics statistics;

    private void Awake()
    {
        gameManager = GameObject.Find("SystemNode").GetComponent<GameManager>();
        statistics = GameObject.Find("SystemNode").GetComponent<Statistics>();

        _btn_Continue.onClick.AddListener(delegate
        {
            gameManager.InitNewSimulation();
            gameObject.SetActive(false);
        });

        _btn_Copy.onClick.AddListener(delegate
        {
            GUIUtility.systemCopyBuffer = statistics.GetLog();
        });
        
        _btn_Quit.onClick.AddListener(delegate
        {
            GameManager.LoadScene(GameManager.Scenes.MAIN_MENU);
        });
    }
}
