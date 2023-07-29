using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private Button Start_Game;

    // Start is called before the first frame update
    void Start()
    {
        Start_Game.onClick.AddListener(StartGame);

        
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Simulation");
    }



}
