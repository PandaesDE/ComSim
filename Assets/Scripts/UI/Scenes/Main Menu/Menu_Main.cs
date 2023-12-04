/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Description:
 *      - Game startscreen
 *      - Navigation to wanted scenes
 *  
 *  References:
 *      Scene:
 *          - Main Menu
 *      Script:
 *          - 
 *          
 *  Notes:
 *      -
 *  
 *  Sources:
 *      - 
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_Main : MonoBehaviour
{
    [SerializeField] private Button _btn_Start;
    [SerializeField] private Button _btn_Settings;

    // Start is called before the first frame update
    void Start()
    {
        _btn_Start.onClick.AddListener(ToSimulation);
        _btn_Settings.onClick.AddListener(ToSettings);
    }

    private void ToSimulation()
    {
        GameManager.LoadScene(GameManager.Scenes.SIMULATION);
    }

    private void ToSettings()
    {
        GameManager.LoadScene(GameManager.Scenes.SETTINGS_MENU);
    }


}
