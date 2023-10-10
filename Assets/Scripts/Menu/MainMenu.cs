/*  Head
 *      Author:             Schneider Erik
 *      1st Supervisor:     Prof.Dr Ralph Lano
 *      2nd Supervisor:     Prof.Dr Matthias Hopf
 *      Project-Title:      ComSim
 *      Bachelor-Title:     "Erschaffung einer digitalen Evolutionssimulation mit Vertiefung auf Sozialverhalten"
 *      University:         Technische Hochschule Nürnberg
 *  
 *  Class Purposes:
 *  
 *  Class Infos:
 *      
 *  Class References:
 *      
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button btn_Start;
    [SerializeField] private Button btn_Settings;

    // Start is called before the first frame update
    void Start()
    {
        btn_Start.onClick.AddListener(toSimulation);
        btn_Settings.onClick.AddListener(toSettings);
    }

    private void toSimulation()
    {
        SceneManager.LoadScene("Simulation");
    }

    private void toSettings()
    {
        SceneManager.LoadScene("SettingsMenu");
    }


}
